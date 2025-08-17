using Microsoft.AspNetCore.Http;
using Puya.Extensions;

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
        public override bool CanLog(ILogger logger, Log log)
        {
            var result = base.CanLog(logger, log);

            do
            {
                if (!result)
                {
                    break;
                }

                result = false;

                var context = HttpContextAccessor?.HttpContext;

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
    }
}
