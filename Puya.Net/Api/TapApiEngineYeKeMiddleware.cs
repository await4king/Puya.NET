using Puya.Api;
using Puya.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Net.Api
{
    public class TapApiEngineYeKeMiddleware : ApiEngineMiddleware
    {
        public override ApiEngineEvents[] Events => new ApiEngineEvents[] { ApiEngineEvents.Serializing };
        public override Task<ApiEngineMiddlewareResponse> RunAsync(ApiCallContext context, ApiEngineEvents @event, CancellationToken cancellation)
        {
            if (!string.IsNullOrEmpty(context.Response?.Message))
            {
                context.Response.Message = context.Response.Message.ChangeYeKe();
            }

            return Task.FromResult<ApiEngineMiddlewareResponse>(null);
        }
    }
}