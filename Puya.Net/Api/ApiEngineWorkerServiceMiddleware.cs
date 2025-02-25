using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading;
using Puya.Api;

namespace Puya.Net.Api
{
    public class ApiEngineWorkerServiceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApiEngine _apiEngine;

        public ApiEngineWorkerServiceMiddleware(RequestDelegate next, IApiEngine apiEngine)
        {
            _next = next;
            _apiEngine = apiEngine;
        }

        public async Task InvokeAsync(HttpContext context, CancellationToken cancellationToken)
        {
            var response = await _apiEngine.Serve(context, cancellationToken);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response);
        }
    }
}
