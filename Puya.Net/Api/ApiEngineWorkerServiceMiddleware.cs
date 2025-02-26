using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Puya.Api;
using Puya.Data;
using System.Threading.Tasks;

public class ApiEngineWorkerServiceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ApiEngineWorkerServiceMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var cancellationToken = context.RequestAborted;

           var apiEngine = scope.ServiceProvider.GetRequiredService<IApiEngine>();

            if (apiEngine == null)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal Server Error: IApiEngine is missing.");
                return;
            }

            var response = await apiEngine.Serve(context, cancellationToken);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response);
       }
    }
}
