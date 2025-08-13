using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class NoLoggingPolicy : ILoggingPolicy
    {
        LoggingPolicyOptions _options;
        public LoggingPolicyOptions Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new LoggingPolicyOptions();
                }

                return _options;
            }
            set
            {
                _options = value;
            }
        }

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
