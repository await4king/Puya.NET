using Puya.Base;
using Puya.Logging.Models;
using Puya.Logging.Web.Abstractions;
using Puya.Logging.Web.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging.Web.Abstractions
{
    public abstract class BaseWebLogger: BaseLogger, IWebLogger
    {
        public IBaseWebLoggerConfig WebConfig
        {
            get { return Config as IBaseWebLoggerConfig; }
        }
        public BaseWebLogger() : this(null)
        { }
        public BaseWebLogger(ILogger next): base(next)
        { }
        protected abstract void LogInternal(WebLog log);
        protected abstract Task LogInternalAsync(WebLog log, CancellationToken cancellation);
        protected virtual bool CanLog(WebLog log)
        {
            var result = (((byte)Config.Level) & log.Type) == log.Type;

            if (result && log.DataObject == null && log.GetData != null)
            {
                log.DataObject = log.GetData(this);
            }

            return result;
        }
        public virtual void Log(WebLog log)
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
        public virtual async Task LogAsync(WebLog log, CancellationToken cancellation)
        {
            try
            {
                if (CanLog(log))
                {
                    await LogInternalAsync(log, cancellation);
                }

                await Next?.LogAsync(log, cancellation);
            }
            catch (Exception e)
            {
                await Next?.DangerAsync(e, "", null, cancellation);
                await Next?.LogAsync(log, cancellation);
            }
        }
    }
}
