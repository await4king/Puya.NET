using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class ChainLogger : ILogger
    {
        public ChainLogger(params ILogger[] loggers)
        {
            Loggers = loggers.Where(x => x != null).ToArray();
        }
        public ChainLogger(IEnumerable<ILogger> loggers) : this(loggers.ToArray())
        { }
        ILogger[] loggers;
        public ILogger[] Loggers
        {
            get
            {
                return loggers;
            }
            private set
            {
                loggers = value;

                if (loggers == null || loggers.Length == 0)
                {
                    loggers = new ILogger[] { new NullLogger() };
                }

                var last = loggers[loggers.Length - 1];
                var lastNull = last as NullLogger;

                if (lastNull == null)
                {
                    // adding a NullLogger at the end of the chain so that
                    // the chain never breaks due to unhandled exceptions.

                    var arr = new ILogger[loggers.Length + 1];

                    Array.Copy(loggers, arr, loggers.Length);

                    arr[loggers.Length] = new NullLogger();

                    loggers = arr;
                }
            }
        }

        public void Clear()
        {
            foreach (var logger in Loggers)
            {
                logger.Clear();
            }
        }

        public async Task ClearAsync(CancellationToken cancellation)
        {
            foreach (var logger in Loggers)
            {
                await logger.ClearAsync(cancellation);
            }
        }

        public void Log(Log log)
        {
            var i = 0;
            var errors = new List<Exception>();

            while (i < Loggers.Length)
            {
                var logger = Loggers[i];

                try
                {
                    foreach (var ex in errors)
                    {
                        logger.Danger(ex);
                    }

                    logger.Log(log);
                }
                catch (Exception e)
                {
                    errors.Add(e);
                }
                finally
                {
                    i++;
                }
            }
        }

        public async Task LogAsync(Log log, CancellationToken cancellation)
        {
            var i = 0;
            var errors = new List<Exception>();

            while (i < Loggers.Length)
            {
                var logger = Loggers[i];

                try
                {
                    foreach (var ex in errors)
                    {
                        await logger.DangerAsync(ex, cancellation);
                    }

                    await logger.LogAsync(log, cancellation);
                }
                catch (Exception e)
                {
                    errors.Add(e);
                }
                finally
                {
                    i++;
                }
            }
        }
    }
}
