using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System;

namespace Puya.Data
{
    public class SqlServerDb : DbBase
    {
        public int MaxContextInfoSize { get; set; }
        public SqlServerDb(): this(null)
        {
        }
        public SqlServerDb(IConnectionStringProvider constrProvider): this(constrProvider, null)
        { }
        public SqlServerDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider)
        {
            ConnectionStringProvider = constrProvider;
            DbContextInfoProvider = dbContextInfoProvider;

            MaxContextInfoSize = 128;
        }
        protected override void SetContextInfo(DbConnection con)
        {
            if (MaxContextInfoSize > 0)
            {
                var contextInfo = DbContextInfoProvider.GetContextInfo();
                var CONTEXT_SQL = $@"
                              declare @ctx varbinary({MaxContextInfoSize})
                              set @ctx = cast(@contextinfo as varbinary({MaxContextInfoSize}))
                              set context_info @ctx";

                var cmd = con.CreateCommand();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = CONTEXT_SQL;

                var p = new SqlParameter("@contextinfo", SqlDbType.NVarChar, MaxContextInfoSize);

                p.Value = string.IsNullOrEmpty(contextInfo) ? DBNull.Value : (object)contextInfo;

                cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }
        }
        protected override DbConnection GetConnectionInternal(string conenctionString)
        {
            return new SqlConnection(conenctionString);
        }
    }
}
