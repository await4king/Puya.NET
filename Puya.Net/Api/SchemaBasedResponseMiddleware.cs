using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Puya.Conversion;
using Puya.Extensions;
using Puya.Service;

namespace Puya.Api
{
    public class SchemaBasedResponseMiddleware : IApiEngineMiddleware
    {
        public ApiEngineEvents[] Events => new ApiEngineEvents[] { ApiEngineEvents.Serializing };

        public Task<ApiEngineMiddlewareResponse> RunAsync(ApiCallContext context, ApiEngineEvents @event, CancellationToken cancellation)
        {
            if (SafeClrConvert.ToBoolean(context.Api.Settings["SchemaBasedResponse"]))
            {
                var dataProp = context.ServiceCallResponse.GetType().GetProperty("Data");

                if (dataProp != null)
                {
                    var data = dataProp.GetValue(context.ServiceCallResponse);

                    if (data != null && data.GetType().IsEnumerable())
                    {
                        var enumerable = data as IEnumerable;

                        if (enumerable != null)
                        {
                            var newResponse = new ServiceResponse<SchemaList<object>>();

                            newResponse.Copy(context.Response);
                            newResponse.Data = enumerable.ToSchemaList();

                            context.Response = newResponse;
                        }
                    }
                }
            }

            var result = new ApiEngineMiddlewareResponse();

            result.Succeeded();

            if (!context.HttpContext.Response.Headers.ContainsKey(ApiEngineConstants.SchemaListResponseHeader))
            {
                context.HttpContext.Response.Headers.Add(ApiEngineConstants.SchemaListResponseHeader, "true");
            }

            return Task.FromResult(result);
        }
    }
}
