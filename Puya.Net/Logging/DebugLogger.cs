using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class DebugLoggerConfig : BaseLoggerConfig
    {
        #region ctor
        public DebugLoggerConfig() : this(null, null)
        { }
        public DebugLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public DebugLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        {
            if (formatter == null)
            {
                Formatter = new StringLogFormatter();
            }
        }
        #endregion
    }
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
    }
}
