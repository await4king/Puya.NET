using Puya.Service;

namespace Puya.Api
{
    public interface IApiResponseSerializer
    {
        string Serialize(ServiceResponse response);
    }
}
