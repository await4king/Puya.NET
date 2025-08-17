using System;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public abstract class BaseLogger<TConfig>: ILogger, IBaseLogger
        where TConfig: BaseLoggerConfig, new()
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

        IBaseLoggerConfig IBaseLogger.Config
        {
            get => Config;
            set
            {
                if (value is TConfig)
                {
                    Config = (TConfig)value;
                }
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
            var result = (((byte)Config.Level) & log.Type) == log.Type && Config.Policy.CanLog(this, log);

            return result;
        }
        protected virtual Log Init(Log log)
        {
            if (log == null)
            {
                return default;
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

            Config.Policy.InitAsync(this, _log, CancellationToken.None).Wait();

            return _log;
        }
        protected virtual async Task<Log> InitAsync(Log log, CancellationToken cancellation)
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

            await Config.Policy.InitAsync(this, _log, cancellation);
            
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

                    if (!Config.Policy.Options.Persist)
                    {
                        Config.Policy.Options = null;
                    }
                }

                Next?.Log(log);
            }
            catch (Exception e)
            {
                if (Next == null)
                {
                    throw;
                }

                Next.Danger(e);
                Next.Log(log);
            }
        }
        public virtual async Task LogAsync(Log log, CancellationToken cancellation)
        {
            try
            {
                if (CanLog(log))
                {
                    var _log = await InitAsync(log, cancellation);

                    await LogInternalAsync(_log, cancellation);

                    if (!Config.Policy.Options.Persist)
                    {
                        Config.Policy.Options = null;
                    }
                }

                if (Next != null)
                {
                    await Next.LogAsync(log, cancellation);
                }
            }
            catch (Exception e)
            {
                if (Next == null)
                {
                    throw;
                }

                await Next.DangerAsync(e, null, cancellation);
                await Next.LogAsync(log, cancellation);
            }
        }
        protected abstract void ClearInternal();
        protected virtual Task ClearInternalAsync(CancellationToken cancellation)
        {
            ClearInternal();

            return Task.CompletedTask;
        }
        public void Clear()
        {
            ClearInternal();

            Next?.Clear();
        }
        public async Task ClearAsync(CancellationToken cancellation)
        {
            await ClearInternalAsync(cancellation);

            Next?.Clear();
        }
    }
}
