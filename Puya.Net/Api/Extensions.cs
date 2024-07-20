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
    }
}
