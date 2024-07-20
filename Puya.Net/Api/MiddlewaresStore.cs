using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Puya.Api
{
    public interface IMiddlewaresStore
    {
        List<IApiEngineMiddleware> GetMiddlewares(ApiEngineEvents engineEvent);
    }
    public class MiddlewaresStore: IMiddlewaresStore
    {
        public MiddlewaresStore()
        {
            Middlewares = new List<IApiEngineMiddleware>();
            
            Middlewares.Add(new AuthorizationMiddleware());
            Middlewares.Add(new SchemaBasedResponseMiddleware());
            Middlewares.Add(new SimpleCorsMiddleware());

            middlewaresCache = new ConcurrentDictionary<ApiEngineEvents, List<IApiEngineMiddleware>>();
        }
        private readonly ConcurrentDictionary<ApiEngineEvents, List<IApiEngineMiddleware>> middlewaresCache;
        public List<IApiEngineMiddleware> Middlewares { get; private set; }
        public void Use(IApiEngineMiddleware middleware)
        {
            lock (AppDomain.CurrentDomain)
            {
                Middlewares.Add(middleware);
            }
        }
        public List<IApiEngineMiddleware> GetMiddlewares(ApiEngineEvents engineEvent)
        {
            return middlewaresCache.GetOrAdd(engineEvent, e => Middlewares?.Where(m => (m?.Events ?? new ApiEngineEvents[0] { }).Contains(e))?.ToList() ?? new List<IApiEngineMiddleware>());
        }
    }
}
