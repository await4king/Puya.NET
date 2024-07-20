using Puya.Base;
using Puya.Data;
using Puya.Logging.Models;
using Puya.Logging.Web.Abstractions;
using Puya.Logging.Web.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging.WebCore
{
    public class WebSqlServerLoggerCore : WebDbLogger
    {
        private IWebSqlServerLoggerConfigCore _config;
        public override IBaseLoggerConfig Config
        {
            get
            {
                return TypeHelper.EnsureInitialized<IWebSqlServerLoggerConfigCore, WebSqlServerLoggerConfigCore>(ref _config);
            }
            set
            {
                var cfg = value as IWebSqlServerLoggerConfigCore;

                if (cfg != null)
                    _config = cfg;
                else
                    _config = new WebSqlServerLoggerConfigCore
                    {
                        AppId = value?.AppId,
                        Formatter = value?.Formatter,
                        ConfigProvider = value?.ConfigProvider,
                        Level = value?.Level ?? LogLevel.All,
                        User = value?.User
                    };
            }
        }
        public IWebSqlServerLoggerConfigCore WebSqlServerConfig
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
        public WebSqlServerLoggerCore() : this(null, null, null)
        { }
        public WebSqlServerLoggerCore(IConnectionStringProvider constrProvider) : this(null, constrProvider, null)
        { }
        public WebSqlServerLoggerCore(IWebSqlServerLoggerConfigCore config) : this(config, null, null)
        { }
        public WebSqlServerLoggerCore(IWebSqlServerLoggerConfigCore config, IConnectionStringProvider constrProvider) : this(config, constrProvider, null)
        { }
        public WebSqlServerLoggerCore(ILogger next) : this(null, null, next)
        { }
        public WebSqlServerLoggerCore(IConnectionStringProvider constrProvider, ILogger next) : this(null, constrProvider, next)
        { }
        public WebSqlServerLoggerCore(IWebSqlServerLoggerConfigCore config, ILogger next) : this(config, null, next)
        { }
        public WebSqlServerLoggerCore(IWebSqlServerLoggerConfigCore config, IConnectionStringProvider constrProvider, ILogger next) : base(null, next)
        {
            _config = config;
            _db = new SqlServerDb(constrProvider);
        }
        #endregion
    }
}
