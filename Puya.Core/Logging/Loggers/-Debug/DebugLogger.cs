using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public class DebugLogger: BaseLogger<DebugLoggerConfig>
    {
        public DebugLogger() : this(null, null)
        { }
        public DebugLogger(DebugLoggerConfig config) : this(config, null)
        { }
        public DebugLogger(DebugLoggerConfig config, ILogger next) : base(config, next)
        { }
        protected override void LogInternal(Log log)
        {
            var data = Config.Formatter.Format(log);

            Debug.WriteLine(data);
        }
        protected override void ClearInternal()
        {
            Debug.Flush();
        }
    }
}
