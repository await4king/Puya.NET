using Puya.Service;

namespace Puya.Api
{
    public class ApiConsumerManagerAuthenticateRequest : ServiceRequest
    { }
    public class ApiConsumerManagerAuthenticateResponse<T> : ServiceResponse<T>
        where T:IApiConsumer
    { }
    public interface IApiConsumerManager<T>
        where T:IApiConsumer
    {
        ApiConsumerManagerAuthenticateResponse<T> Authenticate(ApiConsumerManagerAuthenticateRequest request);
    }
}
