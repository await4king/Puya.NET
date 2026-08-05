using Microsoft.AspNetCore.Builder;

namespace Puya.Net.Api
{
    public static class ApiGatewayWorkerServiceMiddlewareExtensions
    {
        public static IApplicationBuilder UsePuyaGateway(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiGatewayWorkerServiceMiddleware>();
        }
    }
}
