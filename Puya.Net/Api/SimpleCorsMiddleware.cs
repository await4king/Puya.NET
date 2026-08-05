using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public class SimpleCorsMiddleware : IApiGatewayMiddleware
    {
        public ApiGatewayEvents[] Events => new ApiGatewayEvents[] { ApiGatewayEvents.Locating };

        public Task<ApiGatewayMiddlewareResponse> RunAsync(ApiCallContext context, ApiGatewayEvents @event, CancellationToken cancellation)
        {
            var result = new ApiGatewayMiddlewareResponse();
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
