using Puya.Base;
using Puya.Data;
using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class SqlServerLogger : DbLogger
    {
        private ISqlServerLoggerConfig _config;
        public override IBaseLoggerConfig Config
        {
            get
            {
                return TypeHelper.EnsureInitialized<ISqlServerLoggerConfig, SqlServerLoggerConfig>(ref _config);
            }
            set
            {
                var cfg = value as ISqlServerLoggerConfig;

                if (cfg != null)
                    _config = cfg;
                else
                    _config = new SqlServerLoggerConfig
                    {
                        AppId = value?.AppId,
                        Formatter = value?.Formatter,
                        ConfigProvider = value?.ConfigProvider,
                        Level = value?.Level ?? LogLevel.All,
                        User = value?.User
                    };
            }
        }
        public ISqlServerLoggerConfig SqlServerConfig
        {
            get { return _config; }
        }
        private IDb _db;
        public override IDb Db
        {
            get
            {
                return TypeHelper.EnsureInitialized<IDb, SqlServerDb>(ref _db);
            }
            set
            {
                _db = value;
            }
        }
        #region ctor
        public SqlServerLogger() : this(null, null, null)
        { }
        public SqlServerLogger(IConnectionStringProvider constrProvider) : this(null, constrProvider, null)
        { }
        public SqlServerLogger(ISqlServerLoggerConfig config) : this(config, null, null)
        { }
        public SqlServerLogger(ISqlServerLoggerConfig config, IConnectionStringProvider constrProvider) : this(config, constrProvider, null)
        { }
        public SqlServerLogger(ILogger next) : this(null, null, next)
        { }
        public SqlServerLogger(IConnectionStringProvider constrProvider, ILogger next) : this(null, constrProvider, next)
        { }
        public SqlServerLogger(ISqlServerLoggerConfig config, ILogger next) : this(config, null, next)
        { }
        public SqlServerLogger(ISqlServerLoggerConfig config, IConnectionStringProvider constrProvider, ILogger next) : base(null, next)
        {
            _config = config;
            _db = new SqlServerDb(constrProvider);
        }
        protected override string GetClearQuery()
        {
            return "truncate table dbo.Logs";
        }
        #endregion
    }
}
