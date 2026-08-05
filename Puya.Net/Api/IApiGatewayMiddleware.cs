using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public interface IApiGatewayMiddleware
    {
        ApiGatewayEvents[] Events { get; }
        Task<ApiGatewayMiddlewareResponse> RunAsync(ApiCallContext context, ApiGatewayEvents @event, CancellationToken cancellation);
    }
}
