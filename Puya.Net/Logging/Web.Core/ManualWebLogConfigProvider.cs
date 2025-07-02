using Puya.Extensions;
using Puya.Logging.Models;

namespace Puya.Logging.WebCore
{
    public class ManualWebLogConfigProvider : ILogConfigProvider
    {
        private readonly string username;
        private readonly string logLevel;

        public ManualWebLogConfigProvider(string username, string logLevel)
        {
            this.username = username;
            this.logLevel = logLevel;
        }
        public virtual LogLevel GetLogLevel()
        {
            return logLevel?.ToEnum<LogLevel>() ?? LogLevel.None;
        }
        public virtual string GetUser()
        {
            return username;
        }
    }
}
