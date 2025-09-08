using Puya.Service;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public class NoInterceptor : IServiceInterceptor
    {
        public virtual Task OnRan(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            return Task.CompletedTask;
        }

        public virtual Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            return Task.FromResult(true);
        }
    }
}
