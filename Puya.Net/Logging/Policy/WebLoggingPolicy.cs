using Microsoft.AspNetCore.Http;

namespace Puya.Logging
{
    public abstract class WebLoggingPolicy : IWebLoggingPolicy
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        protected WebLoggingPolicy(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public abstract void Prepare(WebLog log);
        public abstract LogLevel? GetOverridedLogLevel();
        public abstract bool CanLog(Log log);
    }
}
