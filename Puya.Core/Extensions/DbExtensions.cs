using Puya.Conversion;
using Puya.Data;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Puya.Extensions
{
    public static class DbExtensions
    {
        static ConcurrentDictionary<string, bool> sprocExists = new ConcurrentDictionary<string, bool>();
        static ConcurrentDictionary<string, IList<SprocParameter>> sproc_params = new ConcurrentDictionary<string, IList<SprocParameter>>();
        public static bool SprocExists(this IDb db, string spName)
        {
            if (db.Specification.Product == DbProduct.SqlServer)
            {
                return sprocExists.GetOrAdd(spName, (name) =>
                {
                    var exists = db.ExecuteScalerSql("select case when exists (select 1 from sys.procedures where name = @name) then 1 else 0 end", new { name });

                    return SafeClrConvert.ToBoolean(exists);
                });
            }

            return false;
        }
        public static void ClearSprocCache(this IDb db)
        {
            sprocExists.Clear();
            sproc_params.Clear();
        }
        public static CommandOutputParameter Output(this SqlDbType sqlDbType, int size = 0)
        {
            return CommandParameter.Output(sqlDbType, "SqlDbType", size);
        }
        public static CommandInputOutputParameter InputOutput(this SqlDbType sqlDbType, object value, int size = 0)
        {
            var param = CommandParameter.InputOutput(sqlDbType, "SqlDbType", size);

            param.Value = value;

            return param;
        }
        public static IList<SprocParameter> GetSprocParameters(this IDb db, string sproc_or_udf)
        {
            return sproc_params.GetOrAdd(sproc_or_udf, _ =>
            {
                var query = @"
select
    p.parameter_id,
	p.name,
    p.max_length,
    p.precision,
    p.scale,
    p.is_output,
    t.name as type_name,
    t.max_length,
    p.system_type_id
from sys.parameters p
	inner join sys.types t on p.system_type_id = t.system_type_id and p.user_type_id = t.user_type_id
where object_id = OBJECT_ID(@sproc_or_udf)
order by p.parameter_id";

                return db.ExecuteReaderSql<SprocParameter>(query, new { sproc_or_udf });
            });
        }
    }
}
