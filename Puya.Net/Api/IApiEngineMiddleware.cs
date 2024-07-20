using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public interface IApiEngineMiddleware
    {
        ApiEngineEvents[] Events { get; }
        Task<ApiEngineMiddlewareResponse> RunAsync(ApiCallContext context, ApiEngineEvents @event, CancellationToken cancellation);
    }
}
