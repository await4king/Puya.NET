using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Puya.Api
{
    public static class Extensions
    {
        #region PuyaGateway middleware
        /*
// نحوه ستاپ کردن gateway در یک برنامه در Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IApiGateway, ApiGatewayDefault>();

var app = builder.Build();

app.MapPuyaGateway();

// یا با مسیر سفارشی
// app.MapPuyaGateway("/api/");

app.Run();
         */
        public static IEndpointRouteBuilder MapPuyaGateway(this IEndpointRouteBuilder endpoints)
        {
            endpoints.Map("{**slug}", async context =>
            {
                var gateway = context.RequestServices.GetRequiredService<IApiGateway>();
                var cancellation = context.RequestAborted;
                var content = await gateway.ProcessAsync(context, cancellation);

                if (context.Response.Headers.ContainsKey(ApiGatewayConstants.EncryptedResponseHeaderName))
                {
                    context.Response.ContentType = "text/plain";
                }
                else
                {
                    context.Response.ContentType = "application/json";
                }

                await context.Response.WriteAsync(content);
            })
            .WithMetadata(new PuyaGatewayAttribute())
            .WithDisplayName("Puya API Gateway")
            .WithGroupName("puya-gateway");

            return endpoints;
        }

        public static IEndpointRouteBuilder MapPuyaGateway(this IEndpointRouteBuilder endpoints, string pattern)
        {
            // اگر pattern با / تمام نشده بود، اضافه کن
            if (!pattern.EndsWith("/"))
                pattern += "/";

            endpoints.Map($"{pattern}{{**slug}}", async context =>
            {
                var gateway = context.RequestServices.GetRequiredService<IApiGateway>();
                var cancellation = context.RequestAborted;
                var content = await gateway.ProcessAsync(context, cancellation);

                if (context.Response.Headers.ContainsKey(ApiGatewayConstants.EncryptedResponseHeaderName))
                {
                    context.Response.ContentType = "text/plain";
                }
                else
                {
                    context.Response.ContentType = "application/json";
                }

                await context.Response.WriteAsync(content);
            })
            .WithMetadata(new PuyaGatewayAttribute())
            .WithDisplayName($"Puya API Gateway ({pattern})")
            .WithGroupName("puya-gateway");

            return endpoints;
        }
        #endregion
        public static string GetOrigins(this Application app)
        {
            if (app.Settings?.ContainsKey("origins") ?? false)
            {
                return app.Settings["origins"];
            }

            return "";
        }
        public static bool Allows(this Application app, string origin, out string acceptedOrigin)
        {
            var origins = app.GetOrigins();

            if (string.IsNullOrEmpty(origin))
            {
                acceptedOrigin = "";
            }
            else
            {
                acceptedOrigin = origins == "*" ? "*" : origins?.Split(',').FirstOrDefault(o => string.Compare(o, origin, StringComparison.CurrentCultureIgnoreCase) == 0);
            }

            return string.IsNullOrEmpty(origins) || origins == "*" || !string.IsNullOrEmpty(acceptedOrigin);
        }
        public static string GetApiSetting(this ApiCallContext context, string key)
        {
            if (context?.Api?.Settings?.ContainsKey(key) ?? false)
            {
                return context.Api.Settings[key];
            }

            return "";
        }
        public static bool IsAuthenticated(this ApiCallContext context)
        {
            return context?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
        public static string GetUserName(this ApiCallContext context)
        {
            return context?.HttpContext?.User?.Identity?.Name;
        }
        public static string GetHeader(this ApiCallContext context, string key)
        {
            return context?.HttpContext?.Request?.Headers[key];
        }
        public static void SetHeader(this ApiCallContext context, string key, string value)
        {
            if (context?.HttpContext?.Response?.Headers == null)
            {
                return;
            }

            if (!context.HttpContext.Response.Headers.ContainsKey(key))
            {
                context.HttpContext.Response.Headers.Add(key, value);

                return;
            }

            context.HttpContext.Response.Headers[key] = value;
        }
    }
}
