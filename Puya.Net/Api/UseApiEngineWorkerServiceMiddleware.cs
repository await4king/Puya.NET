using Microsoft.AspNetCore.Builder;

namespace Puya.Net.Api
{
    public static class ApiEngineWorkerServiceMiddlewareExtensions
    {
        public static IApplicationBuilder UsePuyaApiEngine(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiEngineWorkerServiceMiddleware>();
        }
    }
}
