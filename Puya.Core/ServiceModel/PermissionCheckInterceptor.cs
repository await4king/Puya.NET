using Puya.Extensions;
using Puya.Service;
using Puya.ServiceModel;
using System.Threading.Tasks;

namespace Puya.Core.ServiceModel
{
    public class ServicePermissionCheckInterceptor : NoInterceptor
    {
        protected readonly IServiceContext serviceContext;
        protected readonly PermissionAttribute permission;

        public ServicePermissionCheckInterceptor(IServiceContext serviceContext, PermissionAttribute permission)
        {
            this.serviceContext = serviceContext;
            this.permission = permission;
        }
        public virtual bool HasAccess(string access)
        {
            return serviceContext.User.HasClaim("Permission", access);
        }
        public override Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            var result = false;

            do
            {
                if (response == null)
                {
                    break;
                }

                if (serviceContext == null)
                {
                    response.SetStatus("NoServiceContext");
                    break;
                }

                var user = serviceContext.User;

                if (user == null)
                {
                    response.SetStatus("NoUserContext");
                    break;
                }

                if (user.Identity == null)
                {
                    response.SetStatus("NoIdentity");
                    break;
                }

                if (!user.Identity.IsAuthenticated)
                {
                    response.NotAuthenticated();
                    break;
                }

                if (permission == null)
                {
                    response.SetStatus("NoPermissionSpecified");
                    break;
                }

                var username = user.Identity.Name;

                if (!string.IsNullOrEmpty(permission.Role))
                {
                    var roles = permission.Role.Split(',', MyStringSplitOptions.TrimAndRemoveEmptyEntries);

                    foreach (var role in roles)
                    {
                        if (!user.IsInRole(role))
                        {
                            response.NotAuthorized();
                            break;
                        }
                    }

                    if (response.HasStatus())
                    {
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(permission.Access) && !HasAccess(permission.Access))
                {
                    response.NotAuthorized();
                    break;
                }

                result = true;
            } while (false);

            return Task.FromResult(result);
        }
    }
}
