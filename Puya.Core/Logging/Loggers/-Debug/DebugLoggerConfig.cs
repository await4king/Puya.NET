namespace Puya.Logging
{
    public class DebugLoggerConfig : BaseLoggerConfig
    {
        public DebugLoggerConfig() : this(null)
        { }
        public DebugLoggerConfig(ILogFormatter formatter) : base(formatter)
        {
            if (formatter == null)
            {
                Formatter = new StringLogFormatter();
            }
        }
    }
}
