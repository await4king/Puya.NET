namespace Puya.Logging
{
    public class ConsoleLoggerConfig: BaseLoggerConfig
    {
        public ConsoleLoggerConfig() : this(null)
        { }
        public ConsoleLoggerConfig(ILogFormatter formatter): base(formatter)
        {
            if (formatter == null)
            {
                Formatter = new StringLogFormatter();
            }
        }
    }
}
