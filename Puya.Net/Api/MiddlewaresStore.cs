using Puya.Net.Api;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Puya.Api
{
    public interface IMiddlewaresStore
    {
        List<IApiGatewayMiddleware> GetMiddlewares(ApiGatewayEvents gatewayEvent);
    }
    public class MiddlewaresStore: IMiddlewaresStore
    {
        public MiddlewaresStore()
        {
            Middlewares = new List<IApiGatewayMiddleware>
            {
                new AuthorizationMiddleware(),
                new SchemaBasedResponseMiddleware(),
                new SimpleCorsMiddleware(),
                new ApiGatewayDebuggerMiddleware(),
            };

            middlewaresCache = new ConcurrentDictionary<ApiGatewayEvents, List<IApiGatewayMiddleware>>();
        }
        private readonly ConcurrentDictionary<ApiGatewayEvents, List<IApiGatewayMiddleware>> middlewaresCache;
        public List<IApiGatewayMiddleware> Middlewares { get; private set; }
        public void Use(IApiGatewayMiddleware middleware)
        {
            lock (AppDomain.CurrentDomain)
            {
                Middlewares.Add(middleware);
            }
        }
        public List<IApiGatewayMiddleware> GetMiddlewares(ApiGatewayEvents gatewayEvent)
        {
            return middlewaresCache.GetOrAdd(gatewayEvent, e => Middlewares?.Where(m => (m?.Events ?? new ApiGatewayEvents[0] { }).Contains(e))?.ToList() ?? new List<IApiGatewayMiddleware>());
        }
    }
}
