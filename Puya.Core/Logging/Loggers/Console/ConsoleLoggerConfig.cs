namespace Puya.Logging
{
    public class ConsoleLoggerConfig: BaseLoggerConfig
    {
        public ConsoleLoggerConfig() : this(null)
        { }
        public ConsoleLoggerConfig(ILoggingPolicy policy) : base(new ConsoleLogFormatter(), policy)
        { }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new ConsoleLogFormatter();
        }
    }
}
