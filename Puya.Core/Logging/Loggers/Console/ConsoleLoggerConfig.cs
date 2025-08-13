namespace Puya.Logging
{
    public class ConsoleLoggerConfig: BaseLoggerConfig
    {
        public ConsoleLoggerConfig() : this(null)
        { }
        public ConsoleLoggerConfig(ILogFormatter formatter): this(formatter, null)
        { }
        public ConsoleLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        { }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new ConsoleLogFormatter();
        }
    }
}
