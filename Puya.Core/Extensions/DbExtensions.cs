using Puya.Conversion;
using Puya.Data;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Puya.Extensions
{
    public static class DbExtensions
    {
        static ConcurrentDictionary<string, bool> tableExists = new ConcurrentDictionary<string, bool>();
        static ConcurrentDictionary<string, bool> sprocExists = new ConcurrentDictionary<string, bool>();
        static ConcurrentDictionary<string, IList<SprocParameter>> sproc_params = new ConcurrentDictionary<string, IList<SprocParameter>>();
        public static bool SprocExists(this IDb db, string spName)
        {
            if (db?.Specification.Product == DbProduct.SqlServer && !string.IsNullOrEmpty(spName))
            {
                return sprocExists.GetOrAdd(spName, (name) =>
                {
                    var exists = db.ExecuteScalarSql("select case when exists (select 1 from sys.procedures where name = @name) then 1 else 0 end", new { name });

                    return SafeClrConvert.ToBoolean(exists);
                });
            }

            return false;
        }
        public static void ExtractObjectName(this IDb db, string objectname, out string schema, out string name)
        {
            name = null;
            schema = null;

            if (db?.Specification.Product == DbProduct.SqlServer && !string.IsNullOrEmpty(objectname))
            {
                var i = objectname.IndexOf('.');

                if (i >= 0)
                {
                    schema = objectname.Substring(0, i);
                    name = objectname.Substring(i + 1);
                }
                else
                {
                    name = objectname;
                }

                if (string.IsNullOrEmpty(schema))
                {
                    schema = "dbo";
                }

                if (schema[0] == '[' && schema[schema.Length - 1] == ']')
                {
                    schema = schema.Substring(1, schema.Length - 2);
                }

                if (name[0] == '[' && name[name.Length - 1] == ']')
                {
                    name = name.Substring(1, name.Length - 2);
                }
            }
        }
        public static bool TableExists(this IDb db, string name)
        {
            if (db?.Specification.Product == DbProduct.SqlServer && !string.IsNullOrEmpty(name))
            {
                db.ExtractObjectName(name, out string _schema, out string _name);

                return tableExists.GetOrAdd(name, (old) =>
                {
                    var exists = db.ExecuteScalarSql(@"
select case when exists
(
    select 1
    from            sys.tables  t
        inner join  sys.schemas s   on t.schema_id = s.schema_id
    where t.name = @_name and s.name = @_schema
) then 1 else 0 end", new { _name, _schema });

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
        public static void ClearTablesCache(this IDb db)
        {
            tableExists.Clear();
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
