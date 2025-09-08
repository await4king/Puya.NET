namespace Puya.ServiceModel
{
    public interface IServiceChainInterceptor
    {
        IServiceInterceptor[] Interceptors { get; }
    }
}