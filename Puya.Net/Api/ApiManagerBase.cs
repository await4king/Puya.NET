using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Caching;
using Puya.Data;
using Puya.Logging;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;

namespace Puya.Api
{
    public abstract class ApiManagerBase : BaseService, IApiManager
    {
        protected ConcurrentDictionary<string, Type> typeCache;
        public ApiManagerBase(IDb db, ILogger logger, ICacheManager cache, ISettingService settings) : base(db, logger, cache, settings)
        {
            typeCache = new ConcurrentDictionary<string, Type>();
        }
        private string cacheName;
        public virtual string CacheName
        {
            get
            {
                if (string.IsNullOrEmpty(cacheName))
                {
                    cacheName = $"{this.GetType().FullName}";
                }

                return cacheName;
            }
            set { cacheName = value; }
        }
        public int CacheDuration { get; set; }
        private string CacheNameApi => $"{CacheName}.Apis";
        private string CacheNameApp => $"{CacheName}.Apps";
        public virtual List<Api> GetApis()
        {
            var result = null as List<Api>;
            var load = true;

            if (Cache.IsSet(CacheNameApi))
            {
                result = Cache.Get<List<Api>>(CacheNameApi);
                load = result == null || result.Count == 0;
            }
            
            if (load)
            {
                result = GetApisInternal() ?? new List<Api>();

                Cache.Set(CacheNameApi, result, CacheDuration);
            }

            return result;
        }
        protected abstract List<Api> GetApisInternal();
        public virtual List<Application> GetApps()
        {
            var result = null as List<Application>;
            var load = true;

            if (Cache.IsSet(CacheNameApp))
            {
                result = Cache.Get<List<Application>>(CacheNameApp);
                load = result == null || result.Count == 0;
            }

            if (load)
            {
                result = GetAppsInternal() ?? new List<Application>();

                Cache.Set(CacheNameApp, result, CacheDuration);
            }

            return result;
        }
        protected abstract List<Application> GetAppsInternal();
        public ApiManagerFindApiResponse FindApi(ApiManagerFindApiRequest request)
        {
            return Run<ApiManagerFindApiRequest, ApiManagerFindApiResponse>("FindApi", (req, res) =>
            {
                var apis = GetApis();

                res.Data = apis.FirstOrDefault(a => a.Id == req.Request.Id || string.Compare(req.Request.Api, a.Path, StringComparison.CurrentCultureIgnoreCase) == 0);

                if (res.Data == null)
                {
                    res.NotFound();
                }
                else
                {
                    res.Succeeded();
                }
            }, request);
        }
        public Task<ApiManagerFindApiResponse> FindApiAsync(ApiManagerFindApiRequest request, CancellationToken cancellation)
        {
            return RunAsync<ApiManagerFindApiRequest, ApiManagerFindApiResponse>("FindApi", (req, res, token) =>
            {
                var apis = GetApis();

                res.Data = apis.FirstOrDefault(a => a.Id == req.Request.Id || string.Compare(req.Request.Api, a.Path, StringComparison.CurrentCultureIgnoreCase) == 0);

                if (res.Data == null)
                {
                    res.NotFound();
                }
                else
                {
                    res.Succeeded();
                }

                return Task.CompletedTask;
            }, request, cancellation);
        }
        public ApiManagerFindAppByIdResponse FindAppById(ApiManagerFindAppByIdRequest request)
        {
            return Run<ApiManagerFindAppByIdRequest, ApiManagerFindAppByIdResponse>("FindApp", (req, res) =>
            {
                var apps = GetApps();

                res.Data = apps.FirstOrDefault(a => a.Id == req.Id);

                if (res.Data == null)
                {
                    res.NotFound();
                }
                else
                {
                    res.Succeeded();
                }
            }, request);
        }

        public Task<ApiManagerFindAppByIdResponse> FindAppByIdAsync(ApiManagerFindAppByIdRequest request, CancellationToken cancellation)
        {
            return RunAsync<ApiManagerFindAppByIdRequest, ApiManagerFindAppByIdResponse>("FindApp", (req, res, token) =>
            {
                var apps = GetApps();

                res.Data = apps.FirstOrDefault(a => a.Id == req.Id);

                if (res.Data == null)
                {
                    res.NotFound();
                }
                else
                {
                    res.Succeeded();
                }

                return Task.CompletedTask;
            }, request, cancellation);
        }
        public ApiManagerFindAppByPathResponse FindAppByPath(ApiManagerFindAppByPathRequest request)
        {
            return Run<ApiManagerFindAppByPathRequest, ApiManagerFindAppByPathResponse>("FindApp", (req, res) =>
            {
                var apps = GetApps();

                res.Data = apps.FirstOrDefault(a => string.Compare(a.BasePath, req.Path, StringComparison.OrdinalIgnoreCase) == 0);

                if (res.Data == null)
                {
                    res.NotFound();
                }
                else
                {
                    res.Succeeded();
                }
            }, request);
        }

        public Task<ApiManagerFindAppByPathResponse> FindAppByPathAsync(ApiManagerFindAppByPathRequest request, CancellationToken cancellation)
        {
            return RunAsync<ApiManagerFindAppByPathRequest, ApiManagerFindAppByPathResponse>("FindApp", (req, res, token) =>
            {
                var apps = GetApps();

                res.Data = apps.FirstOrDefault(a => string.Compare(a.BasePath, req.Path, StringComparison.OrdinalIgnoreCase) == 0);

                if (res.Data == null)
                {
                    res.NotFound();
                }
                else
                {
                    res.Succeeded();
                }

                return Task.CompletedTask;
            }, request, cancellation);
        }
        public ApiManagerFindServiceResponse FindService(ApiManagerFindServiceRequest request)
        {
            return Run<ApiManagerFindServiceRequest, ApiManagerFindServiceResponse>("FindService", (req, res) =>
            {
                try
                {
                    res.Data = typeCache.GetOrAdd(req.FullServiceName, fsn =>
                    {
                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            var type = assembly.GetType(fsn);

                            if (type != null)
                            {
                                return type;
                            }
                        }

                        return null;
                    });

                    if (res.Data != null)
                    {
                        res.Succeeded();
                    }
                    else
                    {
                        res.NotFound();
                    }
                }
                catch (Exception e)
                {
                    res.Failed(e);
                }
            }, request);
        }

        public Task<ApiManagerFindServiceResponse> FindServiceAsync(ApiManagerFindServiceRequest request, CancellationToken cancellation)
        {
            return RunAsync<ApiManagerFindServiceRequest, ApiManagerFindServiceResponse>("FindService", (req, res, token) =>
            {
                try
                {
                    res.Data = typeCache.GetOrAdd(req.FullServiceName, fsn =>
                    {
                        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            var type = assembly.GetType(fsn);

                            if (type != null)
                            {
                                return type;
                            }
                        }

                        return null;
                    });

                    if (res.Data != null)
                    {
                        res.Succeeded();
                    }
                    else
                    {
                        res.NotFound();
                    }
                }
                catch (Exception e)
                {
                    res.Failed(e);
                }

                return Task.CompletedTask;
            }, request, cancellation);
        }

        public abstract ApiManagerCreateStoreResponse CreateStore(ApiManagerCreateStoreRequest request);
        public abstract Task<ApiManagerCreateStoreResponse> CreateStoreAsync(ApiManagerCreateStoreRequest request, CancellationToken cancellation);
    }
}
