using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class ConsoleLoggerConfig: BaseLoggerConfig
    {
        #region ctor
        public ConsoleLoggerConfig() : this(null, null)
        { }
        public ConsoleLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public ConsoleLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider): base(formatter, logConfigProvider)
        {
            if (formatter == null)
            {
                Formatter = new StringLogFormatter();
            }
        }
        #endregion
    }
    public class ConsoleLogger : BaseLogger<ConsoleLoggerConfig>
    {
        public ConsoleLogger(): this(null, null)
        { }
        public ConsoleLogger(ConsoleLoggerConfig config): this(config, null)
        { }
        public ConsoleLogger(ConsoleLoggerConfig config, ILogger next): base(config, next)
        { }
        protected override void LogInternal(Log log)
        {
            var data = Config.Formatter.Format(log);

            Console.WriteLine(data);
        }
        public override void Clear()
        {
            Console.Clear();
        }
    }
}
