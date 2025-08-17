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
            Config.Formatter.Format(log);

            Console.WriteLine(new string('-', 100));
        }
        protected override void ClearInternal()
        {
            Console.Clear();
        }
    }
}
