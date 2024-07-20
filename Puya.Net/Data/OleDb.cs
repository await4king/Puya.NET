using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text;

namespace Puya.Data.Ole
{
    public class OleDb : IDb
    {
        public int MaxContextInfoSize { get; set; }
        public virtual bool PersistConnection { get; set; }
        public virtual bool AutoNullEmptyStrings { get; set; }
        private IConnectionStringProvider _constrProvider;
        public IConnectionStringProvider ConnectionStringProvider
        {
            get
            {
                if (_constrProvider == null)
                    _constrProvider = new DefaultConnectionStringProvider();

                return _constrProvider;
            }
            set { _constrProvider = value; }
        }
        public IDbContextInfoProvider DbContextInfoProvider { get; set; }
        public OleDb() : this(null)
        {
        }
        public OleDb(IConnectionStringProvider constrProvider) : this(constrProvider, null)
        { }
        public OleDb(IConnectionStringProvider constrProvider, IDbContextInfoProvider dbContextInfoProvider)
        {
            ConnectionStringProvider = constrProvider;
            DbContextInfoProvider = dbContextInfoProvider;

            MaxContextInfoSize = 128;
        }
        protected virtual void SetContextInfo(OleDbConnection con)
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
        public virtual DbConnection GetConnection()
        {
            var constr = ConnectionStringProvider.GetConnectionString();

            if (DbContextInfoProvider != null)
            {
                var con = new OleDbConnection(constr);

                con.Open();

                SetContextInfo(con);

                return con;
            }
            else
            {
                return new OleDbConnection(constr);
            }
        }
    }
}
