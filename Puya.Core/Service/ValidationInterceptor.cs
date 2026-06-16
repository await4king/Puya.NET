using Puya.Data;
using Puya.Service;
using Puya.ServiceModel;
using System.Threading.Tasks;

namespace Puya.Service
{
    public class ValidationInterceptor : IServiceInterceptor
    {
        private readonly IServiceRequestValidator validator;

        public ValidationInterceptor(IServiceRequestValidator validator)
        {
            this.validator = validator;
        }
        public Task OnRan(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            var result = validator.Validate(request, response);

            return result;
        }
    }
}
