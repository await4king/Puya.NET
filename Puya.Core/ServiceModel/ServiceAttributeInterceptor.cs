using Puya.Service;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public class ServiceAttributeInterceptor : IServiceInterceptor
    {
        public IServiceInterceptorFactory Factory { get; }
        public ServiceAttributeInterceptor(IServiceInterceptorFactory factory)
        {
            Factory = factory;
        }

        public async Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            var result = true;

            if (action != null && Factory != null)
            {
                foreach (var attribute in action.GetType().GetCustomAttributes(true))
                {
                    var interceptor = Factory.GetInterceptor(attribute);

                    if (interceptor != null)
                    {
                        if (!await interceptor.OnRunning(action, request, response))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        public async Task OnRan(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            if (action != null && Factory != null)
            {
                foreach (var attribute in action.GetType().GetCustomAttributes(true))
                {
                    var interceptor = Factory.GetInterceptor(attribute);

                    if (interceptor != null)
                    {
                        await interceptor.OnRan(action, request, response);
                    }
                }
            }
        }
    }
}
