using Puya.Service;
using System;
using System.Threading.Tasks;

namespace Puya.ServiceModel
{
    public class ChainInterceptorOptions
    {
        public bool ContinueOnFailure { get; set; } = true;
    }
    public class ChainInterceptor: NoInterceptor
    {
        private readonly IServiceChainInterceptor chain;

        public ChainInterceptor(IServiceChainInterceptor chain): this(chain, null)
        { }
        public ChainInterceptor(params IServiceInterceptor[] interceptors): this(null, interceptors)
        { }
        public ChainInterceptor(ChainInterceptorOptions options, params IServiceInterceptor[] interceptors)
        {
            this.chain = new ServiceChainInterceptor(interceptors);
        }
        public ChainInterceptor(IServiceChainInterceptor chain, ChainInterceptorOptions options)
        {
            if (chain == null)
            {
                throw new ArgumentNullException(nameof(chain));
            }

            this.chain = chain;

            Options = options;
        }

        ChainInterceptorOptions options;
        public ChainInterceptorOptions Options
        {
            get
            {
                if (options == null)
                {
                    options = new ChainInterceptorOptions();
                }

                return options;
            }
            set
            {
                options = value;
            }
        }

        public override async Task OnRan(IServiceAction action, ServiceRequest request, ServiceResponse response)
        {
            foreach (var interceptor in chain.Interceptors)
            {
                if (interceptor != null)
                {
                    await interceptor.OnRan(action, request, response);

                    if (!response.Success && !Options.ContinueOnFailure)
                    {
                        break;
                    }
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
