using Puya.Service;
using System;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public class ChainInterceptor: NoInterceptor
    {
        private readonly IServiceChainInterceptor chain;

        public ChainInterceptor(IServiceChainInterceptor chain)
        {
            if (chain == null)
            {
                throw new ArgumentNullException(nameof(chain));
            }

            this.chain = chain;
        }
        public override async Task OnRan(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            foreach (var interceptor in chain.Interceptors)
            {
                if (interceptor != null)
                {
                    await interceptor.OnRan(action, request, response);
                }
            }
        }
        public override async Task<bool> OnRunning(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            var result = true;

            foreach (var interceptor in chain.Interceptors)
            {
                if (interceptor != null)
                {
                    if (!await interceptor.OnRunning(action, request, response))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
