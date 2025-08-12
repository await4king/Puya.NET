namespace Puya.Logging
{
    public class ConsoleLoggerConfig: BaseLoggerConfig
    {
        public ConsoleLoggerConfig() : this(null)
        { }
        public ConsoleLoggerConfig(ILogFormatter formatter): base(formatter)
        { }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new ConsoleLogFormatter();
        }
    }
}
