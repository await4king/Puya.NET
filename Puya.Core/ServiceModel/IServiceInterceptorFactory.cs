using Puya.ServiceModel;
namespace Puya.ServiceModel
{
    public interface IServiceInterceptorFactory
    {
        IServiceInterceptor GetInterceptor(object attribute);
    }
}
