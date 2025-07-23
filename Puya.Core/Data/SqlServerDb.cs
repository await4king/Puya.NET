using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Puya.Mapping;

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
        public SqlServerDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider) : this(constrProvider, dbContextInfoProvider, null)
        { }
        public SqlServerDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider, IMapper mapper)
        {
            ConnectionStringProvider = constrProvider;
            DbContextInfoProvider = dbContextInfoProvider;
            Mapper = mapper;
            MaxContextInfoSize = 128;
        }
        protected override void SetContextInfo(DbConnection con)
        {
            if (MaxContextInfoSize > 0)
            {
                var contextInfo = DbContextInfoProvider?.GetContextInfo();

                if (!string.IsNullOrEmpty(contextInfo))
                {
                    var CONTEXT_SQL = $@"
                                  declare @ctx varbinary({MaxContextInfoSize})
                                  set @ctx = cast(@contextinfo as varbinary({MaxContextInfoSize}))
                                  set context_info @ctx";

                    var cmd = con.CreateCommand();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = CONTEXT_SQL;

                    var p = new SqlParameter("@contextinfo", SqlDbType.NVarChar, MaxContextInfoSize);

                    p.Value = contextInfo;

                    cmd.Parameters.Add(p);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        protected override DbConnection GetConnectionInternal(string conenctionString)
        {
            return new SqlConnection(conenctionString);
        }
    }
}
