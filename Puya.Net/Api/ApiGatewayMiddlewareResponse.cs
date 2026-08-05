using Puya.Service;

namespace Puya.Api
{
    public class ApiGatewayMiddlewareResponse: ServiceResponse
    {
        public bool ShouldEndPipeline { get; set; }
    }
}
