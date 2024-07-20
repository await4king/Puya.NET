using System;

namespace Puya.Api
{
    public class ApiEngineException : Exception
    {
        public ApiEngineException()
        {
        }

        public ApiEngineException(string message) : base(message)
        {
        }
        public ApiEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
