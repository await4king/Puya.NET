using Microsoft.AspNetCore.Http;
using Puya.Extensions;
using System;
using UAParser;

namespace Puya.Logging
{
    public class WebLoggingUserRolePolicy : WebLoggingPolicy
    {
        public string Roles { get; set; }
        public string Users { get; set; }
        public bool AllowNotAuthenticatedUsers { get; set; }
        public WebLoggingUserRolePolicy(IHttpContextAccessor httpContextAccessor, string roles, string users) : base(httpContextAccessor)
        {
            Roles = roles;
            Users = users;
        }
        public override LogLevel? GetOverridedLogLevel()
        {
            LogLevel? result = null;
            
            var context = HttpContextAccessor?.HttpContext;

            if (!(context == null || context.Request == null || context.Request.Headers == null))
            {
                string level = context.Request.Headers[WebLoggingConstants.LogLevelHeaderName];

                if (!string.IsNullOrEmpty(level))
                {
                    var _level = level.ToEnum<LogLevel>(LogLevel.None);

                    if (_level != LogLevel.None)
                    {
                        result = _level;
                    }
                }
            }

            return result;
        }
        public override bool CanLog(Log log)
        {
            var result = false;
            var context = HttpContextAccessor?.HttpContext;

            do
            {
                if (context == null || context.User == null || context.User.Identity == null)
                {
                    break;
                }

                if (!(context.User.Identity.IsAuthenticated || AllowNotAuthenticatedUsers))
                {
                    break;
                }

                if (string.IsNullOrEmpty(Users) && string.IsNullOrEmpty(Roles))
                {
                    result = true;
                    break;
                }

                if (!string.IsNullOrEmpty(Users))
                {
                    var users = Users.Split(',', MyStringSplitOptions.TrimAndRemoveEmptyEntries);

                    foreach (var user in users)
                    {
                        if (string.Compare(context.User.Identity.Name, user, true) == 0)
                        {
                            result = true;
                            break;
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(Roles))
                {
                    var roles = Roles.Split(',', MyStringSplitOptions.TrimAndRemoveEmptyEntries);

                    foreach (var role in roles)
                    {
                        if (context.User.IsInRole(role))
                        {
                            result = true;
                            break;
                        }
                    }
                }
            } while (false);

            return result;
        }
        public override void Prepare(WebLog log)
        {
            var context = HttpContextAccessor.HttpContext;

            if (context != null && log != null)
            {
                if (string.IsNullOrEmpty(log.User))
                {
                    log.User = context.User?.Identity?.Name;
                }

                if (context.Request != null)
                {
                    log.Method = context.Request.Method;

                    if (context.Request.Headers != null)
                    {
                        log.Referrer = context.Request.Headers["Referer"];
                    }

                    if (string.Compare(log.Method, "POST", StringComparison.OrdinalIgnoreCase) == 0 && context.Request.HasFormContentType)
                    {
                        try
                        {
                            log.Form = context.Request.Form?.Join("\n");
                        }
                        catch
                        { }
                    }

                    if (context.Request.Headers != null)
                    {
                        try
                        {
                            log.Headers = context.Request.Headers?.Join("\n");
                        }
                        catch
                        { }
                    }

                    log.Url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);


                    if (context.Request.Cookies != null)
                    {
                        try
                        {
                            log.Cookies = context.Request.Cookies?.Join(";");
                        }
                        catch
                        { }
                    }

                    if (context.Request.Headers != null)
                    {
                        try
                        {
                            var uaParser = Parser.GetDefault();
                            var ci = uaParser.Parse(context.Request.Headers["User-Agent"]);

                            log.BrowserName = ci.UA.Family;
                            log.BrowserVersion = ci.UA.Major + "." + ci.UA.Minor + "." + ci.UA.Patch;
                        }
                        catch
                        { }
                    }
                }
            }
        }
    }
}
