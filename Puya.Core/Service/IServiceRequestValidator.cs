using System.Threading.Tasks;

namespace Puya.Service
{
    public interface IServiceRequestValidator
    {
        Task<bool> Validate<TRequest, TResponse>(TRequest req, TResponse res)
            where TRequest : class, ServiceRequest
            where TResponse : ServiceResponse, new();
    }
}
