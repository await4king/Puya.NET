using System;

namespace Puya.Logging
{
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
