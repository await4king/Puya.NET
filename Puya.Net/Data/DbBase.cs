using System.Data.Common;

namespace Puya.Data
{
    public abstract class DbBase : IDb
    {
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
        protected abstract DbConnection GetConnectionInternal(string connectionString);
        protected abstract void SetContextInfo(DbConnection con);
        protected virtual void OnBeforeConnection(DbConnection con) { }
        protected virtual void OnAfterConnection(DbConnection con) { }
        protected virtual void OnBeforeContextInfo(DbConnection con) { }
        protected virtual void OnAfterContextInfo(DbConnection con) { }
        public DbConnection GetConnection()
        {
            var constr = ConnectionStringProvider.GetConnectionString();
            var con = GetConnectionInternal(constr);

            OnBeforeConnection(con);

            con.Open();

            OnAfterConnection(con);
            OnBeforeContextInfo(con);

            if (DbContextInfoProvider != null)
            {
                SetContextInfo(con);

                OnAfterContextInfo(con);
            }

            return con;
        }
    }
}
