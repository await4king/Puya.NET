using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Puya.Extensions;
using Puya.Logging.Models;

namespace Puya.Logging.WebCore
{
    public class ProtectedWebLogConfigProvider: PublicWebLogConfigProvider
    {
        public string Roles { get; set; }
        public string Users { get; set; }
        public ProtectedWebLogConfigProvider(IHttpContextAccessor httpContextAccessor, string roles, string users) : base(httpContextAccessor)
        {
            Roles = roles;
            Users = users;
        }
        public override LogLevel GetLogLevel()
        {
            var result = LogLevel.None;
            var context = this.HttpContextAccessor.HttpContext;

            if (context.User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(Users))
                {
                    var users = Users.Split(',', MyStringSplitOptions.TrimAndRemoveEmptyEntries);
                    var found = false;

                    foreach (var user in users)
                    {
                        if (string.Compare(context.User.Identity.Name, user, true) == 0)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        result = base.GetLogLevel();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Roles))
                    {
                        var roles = Roles.Split(',', MyStringSplitOptions.TrimAndRemoveEmptyEntries);
                        var found = false;

                        foreach (var role in roles)
                        {
                            if (context.User.IsInRole(role))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found)
                        {
                            result = base.GetLogLevel();
                        }
                    }
                    else
                    {
                        result = base.GetLogLevel();
                    }
                }
            }

            return result;
        }
    }
}
