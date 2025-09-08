using Puya.Service;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public interface IServiceInterceptor
    {
        Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response);
        Task OnRan(IServiceAction action, ServiceRequest request, ServiceResponse response);
    }
}
