using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ApiLogging
{
    public static class Extensions
    {
        public static void Log(this IApiLogger logger,
                                ApiCallDirection direction,
                                ApiClient client,
                                ApiServer server,
                                ApiRequest request,
                                ApiResponse response)
        {
            logger?.Log(new ApiLog
            {
                Direction = direction,
                Client = client,
                Server = server,
                Request = request,
                Response = response
            });
        }
        public static void Log(this BaseApiLogger apiLogger, ApiCallDirection direction, ApiRequest request, ApiResponse response)
        {
            apiLogger?.Log(direction, apiLogger?.Client, apiLogger?.Server, request, response);
        }
        public static void LogIncoming(this BaseApiLogger apiLogger, ApiRequest request, ApiResponse response)
        {
            apiLogger?.Log(ApiCallDirection.Incoming, apiLogger?.Client, apiLogger?.Server, request, response);
        }
        public static void LogOutgoing(this BaseApiLogger apiLogger, ApiRequest request, ApiResponse response)
        {
            apiLogger?.Log(ApiCallDirection.Outgoing, apiLogger?.Client, apiLogger?.Server, request, response);
        }
    }
}
