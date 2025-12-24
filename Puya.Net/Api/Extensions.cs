using System;
using System.Linq;

namespace Puya.Api
{
    public static class Extensions
    {
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
