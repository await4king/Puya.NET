using Puya.Mapping;
using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace Puya.Data.Ole
{
    public class OleDb : DbBase
    {
        public int MaxContextInfoSize { get; set; }
        public OleDb() : this(null)
        {
        }
        public OleDb(IConnectionStringProvider constrProvider) : this(constrProvider, null)
        { }
        public OleDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider) : this(constrProvider, dbContextInfoProvider, null)
        { }
        public OleDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider, IMapper mapper)
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
                var contextInfo = DbContextInfoProvider.GetContextInfo();
                var CONTEXT_SQL = $@"
                              declare @ctx varbinary({MaxContextInfoSize})
                              set @ctx = cast(@contextinfo as varbinary({MaxContextInfoSize}))
                              set context_info @ctx";

                var cmd = con.CreateCommand();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = CONTEXT_SQL;

                var p = new OleDbParameter("@contextinfo", OleDbType.VarWChar, MaxContextInfoSize);

                p.Value = string.IsNullOrEmpty(contextInfo) ? DBNull.Value : (object)contextInfo;

                cmd.Parameters.Add(p);

                cmd.ExecuteNonQuery();
            }
        }
        protected override DbConnection GetConnectionInternal(string conenctionString)
        {
            return new OleDbConnection(conenctionString);
        }
    }
}
