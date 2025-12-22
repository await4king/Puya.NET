using Puya.Mapping;
using System.Data.Common;

namespace Puya.Data
{
    public abstract class DbBase : IDb
    {
        public abstract DbSpecification Specification { get; }
        public virtual bool PersistConnection { get; set; }
        public virtual bool AutoNullEmptyStrings { get; set; }
        public IMapper Mapper { get; set; }
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
        IDbContextInfoProvider _dbContextInfoProvider;
        public IDbContextInfoProvider DbContextInfoProvider
        {
            get
            {
                if (_dbContextInfoProvider == null)
                    _dbContextInfoProvider = new NullDbContextInfoProvider();

                return _dbContextInfoProvider;
            }
            set { _dbContextInfoProvider = value; }
        }
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
