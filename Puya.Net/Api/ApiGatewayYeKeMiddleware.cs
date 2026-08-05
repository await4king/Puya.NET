using Puya.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public class ApiGatewayYeKeMiddleware : ApiGatewayMiddleware
    {
        public override ApiGatewayEvents[] Events => new ApiGatewayEvents[] { ApiGatewayEvents.Serializing };
        public override Task<ApiGatewayMiddlewareResponse> RunAsync(ApiCallContext context, ApiGatewayEvents @event, CancellationToken cancellation)
        {
            if (!string.IsNullOrEmpty(context.Response?.Message))
            {
                context.Response.Message = context.Response.Message.ChangeYeKe();
            }

            return Task.FromResult<ApiGatewayMiddlewareResponse>(null);
        }
    }
}