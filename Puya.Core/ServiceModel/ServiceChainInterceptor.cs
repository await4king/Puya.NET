namespace Puya.ServiceModel
{
    public class ServiceChainInterceptor : IServiceChainInterceptor
    {
        public ServiceChainInterceptor(params IServiceInterceptor[] interceptors)
        {
            Interceptors = interceptors;
        }

        public IServiceInterceptor[] Interceptors { get; }
    }
}
