using Puya.Core.ServiceModel;
using Puya.Service;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public class PermissionCheckInterceptor : NoInterceptor
    {
        public PermissionCheckInterceptor(IServiceContext context, PermissionAttribute permission)
        {
            Context = context;
            Permission = permission;
        }

        public IServiceContext Context { get; }
        public PermissionAttribute Permission { get; }

        public override Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            var result = true;

            if (action != null && Permission != null && Context != null && Context.User != null)
            {
                if (!string.IsNullOrEmpty(Permission.Role))
                {
                    var roles = Permission.Role.Split(',');

                    foreach (var role in roles)
                    {
                        if (!Context.User.IsInRole(role))
                        {
                            result = false;
                            break;
                        }
                    }

                    if (result)
                    {
                        result = Context.User.HasClaim("Permission", Permission.Access);
                    }
                }
            }

            return Task.FromResult(result);
        }
    }
}
