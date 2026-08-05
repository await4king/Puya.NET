using System;

namespace Puya.Api
{
    public class ApiGatewayException : Exception
    {
        public ApiGatewayException()
        {
        }

        public ApiGatewayException(string message) : base(message)
        {
        }
        public ApiGatewayException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
