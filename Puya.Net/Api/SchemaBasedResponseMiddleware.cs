using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Puya.Conversion;
using Puya.Extensions;
using Puya.Service;

namespace Puya.Api
{
    public class SchemaBasedResponseMiddleware : IApiGatewayMiddleware
    {
        public ApiGatewayEvents[] Events => new ApiGatewayEvents[] { ApiGatewayEvents.Serializing };

        public Task<ApiGatewayMiddlewareResponse> RunAsync(ApiCallContext context, ApiGatewayEvents @event, CancellationToken cancellation)
        {
            if (SafeClrConvert.ToBoolean(context.GetApiSetting("SchemaBasedResponse")))
            {
                var dataProp = context.ServiceCallResponse?.GetType()?.GetProperty("Data");

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

            var result = new ApiGatewayMiddlewareResponse();

            result.Succeeded();

            context.SetHeader(ApiGatewayConstants.SchemaListResponseHeader, "true");

            return Task.FromResult(result);
        }
    }
}
