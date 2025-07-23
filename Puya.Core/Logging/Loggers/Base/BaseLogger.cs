using System;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging
{
    public abstract class BaseLogger<TConfig>: ILogger where TConfig: BaseLoggerConfig, new()
    {
        public ILogger Next { get; set; }
        public BaseLogger(): this(null, null)
        { }
        public BaseLogger(TConfig config) : this(config, null)
        { }
        public BaseLogger(TConfig config, ILogger next)
        {
            Next = next;
            Config = config;
        }
        private TConfig _config;
        public virtual TConfig Config
        {
            get
            {
                if (_config == null)
                    _config = new TConfig();

                return _config;
            }
            set
            {
                _config = value;
            }
        }
        protected abstract void LogInternal(Log log);
        protected virtual Task LogInternalAsync(Log log, CancellationToken cancellation)
        {
            LogInternal(log);

            return Task.CompletedTask;
        }
        protected virtual bool CanLog(Log log)
        {
            var result = (((byte)Config.Level) & log.Type) == log.Type;

            return result;
        }
        protected virtual Log Init(Log log)
        {
            if (log == null)
            {
                return null;
            }

            var _log = log.Clone();

            if (_log.AppId == null)
            {
                _log.AppId = Config.AppId;
            }

            if (string.IsNullOrEmpty(log.User))
            {
                _log.User = Config.User;
            }

            return _log;
        }
        public virtual void Log(Log log)
        {
            try
            {
                if (CanLog(log))
                {
                    var _log = Init(log);

                    LogInternal(_log);
                }

                Next?.Log(log);
            }
            catch (Exception e)
            {
                Next?.Danger(e);
                Next?.Log(log);
            }
        }
        public virtual async Task LogAsync(Log log, CancellationToken cancellation)
        {
            try
            {
                if (CanLog(log))
                {
                    var _log = Init(log);

                    await LogInternalAsync(_log, cancellation);
                }

                if (Next != null)
                {
                    await Next.LogAsync(log, cancellation);
                }
            }
            catch (Exception e)
            {
                if (Next != null)
                {
                    await Next.DangerAsync(e, null, cancellation);
                    await Next.LogAsync(log, cancellation);
                }
            }
        }
        public virtual void Clear()
        { }
        public virtual Task ClearAsync(CancellationToken cancellation)
        {
            return Task.Run(Clear);
        }
    }
}
