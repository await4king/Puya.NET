using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public class SimpleCorsMiddleware : IApiEngineMiddleware
    {
        public ApiEngineEvents[] Events => new ApiEngineEvents[] { ApiEngineEvents.Locating };

        public Task<ApiEngineMiddlewareResponse> RunAsync(ApiCallContext context, ApiEngineEvents @event, CancellationToken cancellation)
        {
            var result = new ApiEngineMiddlewareResponse();
            var origin = "";

            if (context != null && context.App != null && context.App.Allows(context.GetHeader("origin"), out origin))
            {
                if (!string.IsNullOrEmpty(origin))
                {
                    context.SetHeader("Access-Control-Allow-Origin", origin);

                    if (origin != "*")
                    {
                        context.SetHeader("Vary", "Origin");
                    }
                }
            }
            else
            {
                result.SetStatus("OriginDenied");
                result.ShouldEndPipeline = true;
            }

            return Task.FromResult(result);
        }
    }
}
