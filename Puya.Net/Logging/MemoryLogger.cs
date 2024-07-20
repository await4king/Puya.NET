using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class MemoryLoggerConfig : BaseLoggerConfig
    {
        #region ctor
        public MemoryLoggerConfig() : this(null, null)
        { }
        public MemoryLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public MemoryLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        { }
        public int MaxLogCount { get; set; }
        #endregion
    }
    public class MemoryLogger : BaseLogger<MemoryLoggerConfig>
    {
        private List<Log> logs;
        public List<Log> Logs
        {
            get
            {
                if (logs == null)
                {
                    logs = new List<Log>();
                }

                return logs;
            }
        }
        public MemoryLogger() : this(null, null)
        { }
        public MemoryLogger(MemoryLoggerConfig config) : this(config, null)
        { }
        public MemoryLogger(MemoryLoggerConfig config, ILogger next) : base(config, next)
        { }
        protected override void LogInternal(Log log)
        {
            if (StrongConfig.MaxLogCount > 0 && Logs.Count >= StrongConfig.MaxLogCount)
            {
                Clear();
            }

            if (log.Id == 0)
            {
                log.Id = (Logs.Count > 0 ? Logs.Max(l => l.Id) : 0) + 1;
            }

            Logs.Add(log);
        }

        protected override Task LogInternalAsync(Log log, CancellationToken cancellation)
        {
            LogInternal(log);

            return Task.CompletedTask;
        }

        public override void Clear()
        {
            Logs.Clear();
        }
    }
}
