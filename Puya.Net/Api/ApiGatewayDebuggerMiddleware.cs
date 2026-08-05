using Puya.Service;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Api
{
    public class ApiGatewayDebuggerMiddleware : ApiGatewayMiddleware
    {
        public override ApiGatewayEvents[] Events => new ApiGatewayEvents[] { ApiGatewayEvents.Serializing };
        public ApiGatewayDebuggerMiddleware()
        {
        }
        public override Task<ApiGatewayMiddlewareResponse> RunAsync(ApiCallContext context, ApiGatewayEvents @event, CancellationToken cancellation)
        {
            var logProvider = GetService<ILogProvider>(context);
            var debugger = GetService<Puya.Debugging.IDebugger>(context);

            if (debugger.IsDebugging && logProvider.Logs?.Count > 0)
            {
                // Description:
                // since requests are added to LogProvider (in TapActionBasedService) we should
                // nullify those requests that implement IApiServiceRequest, because IApiServiceRequest.CallContext
                // cannot be serialized to string. If we do not this, serializing the response will fail.

                context.Response.Logs = new LogList();

                foreach (var log in logProvider.Logs)
                {
                    var apiRequest = log.Data as IApiServiceRequest;

                    if (apiRequest != null)
                    {
                        apiRequest.CallContext = null;
                    }
                }

                context.Response.Logs = logProvider.Logs;
            }

            return Task.FromResult<ApiGatewayMiddlewareResponse>(null);
        }
    }
}
