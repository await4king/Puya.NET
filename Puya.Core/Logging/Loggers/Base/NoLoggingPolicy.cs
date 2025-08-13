using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class NoLoggingPolicy : ILoggingPolicy
    {
        public LoggingPolicyOptions Options { get; set; }

        public bool CanLog(ILogger logger, Log log)
        {
            return true;
        }

        public Task InitAsync(ILogger logger, Log log, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}
