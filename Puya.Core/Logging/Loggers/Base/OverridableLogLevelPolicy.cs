using Puya.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class OverridableLogLevelPolicy : ILoggingPolicy
    {
        protected virtual string GetOverridedLogLevel()
        {
            return null;
        }
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
        public virtual bool CanLog(ILogger logger, Log log)
        {
            var _logger = logger as IBaseLogger;

            if (_logger != null)
            {
                LogLevel? logLevel = null;

                string level = GetOverridedLogLevel();

                if (!string.IsNullOrEmpty(level))
                {
                    var _level = level.ToEnum<LogLevel>(LogLevel.None);

                    if (_level != LogLevel.None)
                    {
                        logLevel = _level;
                    }
                }

                if (logLevel.HasValue && logLevel.Value != LogLevel.None)
                {
                    return _logger.Config.Level == logLevel.Value;
                }
            }

            return true;
        }

        public virtual Task InitAsync(ILogger logger, Log log, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}
