using Microsoft.AspNetCore.Authorization;
using Puya.Core.ServiceModel;
using Puya.Extensions;
using Puya.Service;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public class ServiceRoleCheckInterceptor : NoInterceptor
    {
        private readonly IServiceContext serviceContext;
        private readonly AuthorizeAttribute authorize;

        public ServiceRoleCheckInterceptor(IServiceContext serviceContext, AuthorizeAttribute authorize)
        {
            this.serviceContext = serviceContext;
            this.authorize = authorize;
        }
        public override Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            var result = false;

            do
            {
                var user = serviceContext?.User;

                if (user == null)
                {
                    response.SetStatus("NoUserContext");
                    break;
                }

                if (!user.Identity.IsAuthenticated)
                {
                    response.NotAuthenticated();
                    break;
                }

                var username = user.Identity.Name;

                if (!string.IsNullOrEmpty(authorize.Roles))
                {
                    foreach (var role in authorize.Roles.Split(',', MyStringSplitOptions.TrimAndRemoveEmptyEntries))
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

                result = true;
            } while (false);

            return Task.FromResult(result);
        }
    }
}
