using Puya.Base;
using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging
{
    public abstract class BaseLogger: ILogger
    {
        public abstract IBaseLoggerConfig Config { get; set; }
        public ILogger Next { get; set; }
        public BaseLogger() : this(null, null)
        { }
        public BaseLogger(ILogger next) : this(null, next)
        { }
        public BaseLogger(IBaseLoggerConfig config) : this(config, null)
        { }
        public BaseLogger(IBaseLoggerConfig config, ILogger next)
        {
            Next = next;
            Config = config;
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

            if (result && log.DataObject == null && log.GetData != null)
            {
                log.DataObject = log.GetData(this);
            }

            return result;
        }
        public virtual void Log(Log log)
        {
            try
            {
                if (CanLog(log))
                {
                    LogInternal(log);
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
                    await LogInternalAsync(log, cancellation);
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
    public abstract class BaseLogger<TConfig> : BaseLogger where TConfig: class, IBaseLoggerConfig, new()
    {
        private TConfig _config;
        public override IBaseLoggerConfig Config
        {
            get
            {
                if (_config == null)
                    _config = new TConfig();

                return _config;
            }
            set
            {
                if (value != null)
                {
                    var cfg = value as TConfig;

                    if (cfg != null)
                    {
                        _config = cfg;
                    }
                    else
                    {
                        throw new Exception($"Invalid Config. Expected {typeof(TConfig).Name} type.");
                    }
                }
            }
        }
        public virtual TConfig StrongConfig
        {
            get
            {
                if (_config == null)
                    _config = new TConfig();

                return _config;
            }
            set
            {
                if (value != null)
                {
                    var cfg = value as TConfig;

                    if (cfg != null)
                    {
                        _config = cfg;
                    }
                    else
                    {
                        throw new Exception($"Invalid Config. Expected {typeof(TConfig).Name} type.");
                    }
                }
            }
        }
        public BaseLogger() : this(null, null)
        { }
        public BaseLogger(ILogger next): this(null, next)
        { }
        public BaseLogger(IBaseLoggerConfig config) : this(config, null)
        { }
        public BaseLogger(IBaseLoggerConfig config, ILogger next): base(config, next)
        { }
    }
}
