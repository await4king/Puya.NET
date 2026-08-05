using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Puya.Api;
using Puya.Data;
using System.Threading.Tasks;

public class ApiGatewayWorkerServiceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ApiGatewayWorkerServiceMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var cancellationToken = context.RequestAborted;

           var apiGateway = scope.ServiceProvider.GetRequiredService<IApiGateway>();

            if (apiGateway == null)
            {
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync("Internal Server Error: IApiGateway is missing.");

                return;
            }

            var response = await apiGateway.ProcessAsync(context, cancellationToken);

            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(response);
       }
    }
}
