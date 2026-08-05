using System;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public abstract class ApiGatewayMiddleware: IApiGatewayMiddleware
    {
        public virtual ApiGatewayEvents[] Events { get; protected set; }
        public abstract Task<ApiGatewayMiddlewareResponse> RunAsync(ApiCallContext context, ApiGatewayEvents @event, CancellationToken cancellation);
        public object GetService(ApiCallContext context, Type type)
        {
            return context.Scope.ServiceProvider.GetService(type);
        }
        public T GetService<T>(ApiCallContext context)
        {
            return (T)GetService(context, typeof(T));
        }
    }
}
