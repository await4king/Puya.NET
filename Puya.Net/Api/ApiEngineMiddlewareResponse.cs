using Puya.Service;

namespace Puya.Api
{
    public class ApiEngineMiddlewareResponse: ServiceResponse
    {
        public bool ShouldEndPipeline { get; set; }
    }
}
