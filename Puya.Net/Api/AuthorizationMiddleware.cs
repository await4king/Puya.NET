using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Conversion;
using Puya.Extensions;

namespace Puya.Api
{
    public class AuthorizationMiddleware : IApiEngineMiddleware
    {
        public ApiEngineEvents[] Events => new ApiEngineEvents[] { ApiEngineEvents.Locating };

        public Task<ApiEngineMiddlewareResponse> RunAsync(ApiCallContext context, ApiEngineEvents @event, CancellationToken cancellation)
        {
            var result = new ApiEngineMiddlewareResponse();

            if (SafeClrConvert.ToBoolean(context.GetApiSetting("RequiresAuthentication")))
            {
                do
                {
                    if (!context.IsAuthenticated())
                    {
                        result.SetStatus("NotAuthenticated");
                        result.ShouldEndPipeline = true;

                        break;
                    }

                    var users = context.GetApiSetting("AllowedUsers");
                    
                    if (!string.IsNullOrEmpty(users))
                    {
                        if (!users.Split(",", MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries).Contains(context.GetUserName()))
                        {
                            result.SetStatus("UserAccessDenied");
                            result.ShouldEndPipeline = true;

                            break;
                        }
                    }

                    var roles = context.GetApiSetting("AllowedRoles");

                    if (!string.IsNullOrEmpty(roles))
                    {
                        var found = false;
                        var arrRoles = roles.Split(",", MyStringSplitOptions.TrimToLowerAndRemoveEmptyEntries);

                        foreach (var role in arrRoles)
                        {
                            if (context.HttpContext.User.IsInRole(role))
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            result.SetStatus("RoleAccessDenied");
                            result.ShouldEndPipeline = true;

                            break;
                        }
                    }
                } while (false);
            }

            return Task.FromResult(result);
        }
    }
}
