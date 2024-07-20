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

            if (context.App.Allows(context.HttpContext.Request.Headers["origin"], out origin))
            {
                if (!string.IsNullOrEmpty(origin))
                {
                    context.HttpContext.Response.Headers["Access-Control-Allow-Origin"] = origin;

                    if (origin != "*")
                    {
                        context.HttpContext.Response.Headers["Vary"] = "Origin";
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
