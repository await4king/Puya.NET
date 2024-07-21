using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Base;
using Puya.Caching;
using Puya.Data;
using Puya.Logging;
using Puya.Service;
using Puya.Settings;

namespace Puya.ServiceModel
{
    public interface IBaseService
    {
        IDb Db { get; }
        ILogger Logger { get; }
        ICacheManager Cache { get; }
        ISettingService Settings { get; }
    }
    public abstract class BaseService: IBaseService
    {
        #region properties
        private IDb _db;
        public virtual IDb Db
        {
            get
            {
                return TypeHelper.EnsureInitialized<IDb, NullDb>(ref _db);
            }
            set { _db = value; }
        }
        private ILogger _logger;
        public virtual ILogger Logger
        {
            get
            {
                return TypeHelper.EnsureInitialized<ILogger, NullLogger>(ref _logger);
            }
            set { _logger = value; }
        }
        private ICacheManager _cache;
        public virtual ICacheManager Cache
        {
            get
            {
                return TypeHelper.EnsureInitialized<ICacheManager, NullCacheManager>(ref _cache);
            }
            set { _cache = value; }
        }
        private ISettingService _settings;
        public virtual ISettingService Settings
        {
            get
            {
                return TypeHelper.EnsureInitialized<ISettingService, InMemorySettingService>(ref _settings);
            }
            set { _settings = value; }
        }
        private string name;
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    name = this.GetType().Name;

                return name;
            }
        }
        #endregion 
        public BaseService(IDb db, ILogger logger, ICacheManager cache, ISettingService settings)
        {
            _db = db;
            _logger = logger;
            _cache = cache;
            _settings = settings;
        }
        protected TRes Run<TReq, TRes>(string actionName, Action<TReq, TRes> action, TReq req)
            where TRes : ServiceResponse, new()
        {
            var response = new TRes();

            try
            {
                Logger.Debug(actionName, req);

                action(req, response);

            }
            catch (Exception e)
            {
                Logger.Danger(e, req);

                response.Flawed();
            }

            Logger.Debug(actionName, response);

            return response;
        }
        protected async Task<TRes> RunAsync<TReq, TRes>(string actionName, Func<TReq, TRes, CancellationToken, Task> action, TReq req, CancellationToken cancellation)
            where TRes : ServiceResponse, new()
        {
            var response = new TRes();

            try
            {
                await Logger.DebugAsync(actionName, req);

                await action(req, response, cancellation);
            }
            catch (Exception e)
            {
                await Logger.DangerAsync(e, req);

                response.Flawed();
            }

            await Logger.DebugAsync(actionName, response);

            return response;
        }
    }
}
