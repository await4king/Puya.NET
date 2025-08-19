using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;

namespace Puya.Extensions
{
    public static class WbCoreExtensions
    {
        public static IEnumerable<string> Roles(this ClaimsPrincipal principal)
        {
            var result = new List<string>();

            foreach (var identity in principal.Identities)
            {
                if (identity != null)
                {
                    foreach (var claim in identity.Claims)
                    {
                        if (claim.Type == identity.RoleClaimType)
                        {
                            result.Add(claim.Value);
                        }
                    }
                }
            }

            return result;
        }
        public static bool TryGetHttpContext(this IHttpContextAccessor httpcontextAccessor, out HttpContext httpContext, string options = "request,items,request.headers")
        {
            httpContext = httpcontextAccessor?.HttpContext;

            var result = httpContext != null;

            if (result && !string.IsNullOrWhiteSpace(options))
            {
                foreach (var part in options.Split(',', MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries))
                {
                    switch (part)
                    {
                        case "r":
                        case "rq":
                        case "req":
                        case "request":
                            result &= httpContext.Request != null;
                            break;
                        case "r.h":
                        case "rq.h":
                        case "req.headers":
                        case "request.headers":
                            result &= httpContext.Request?.Headers != null;
                            break;
                        case "i":
                        case "items":
                            result &= httpContext.Items != null;
                            break;
                        case "rs":
                        case "res":
                        case "response":
                            result &= httpContext.Response != null;
                            break;
                        case "rs.h":
                        case "res.headers":
                        case "response.headers":
                            result &= httpContext.Response?.Headers != null;
                            break;
                        case "s":
                        case "session":
                            result &= httpContext.Session != null;
                            break;
                        case "u":
                        case "user":
                            result &= httpContext.User != null;
                            break;
                        case "ui":
                        case "u.i":
                        case "user.identity":
                            result &= httpContext.User?.Identity != null;
                            break;
                    }

                    if (!result)
                    {
                        break;
                    }
                }
            }

            return result;
        }
    }
}
