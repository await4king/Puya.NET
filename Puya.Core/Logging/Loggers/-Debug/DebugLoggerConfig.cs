namespace Puya.Logging
{
    public class DebugLoggerConfig : BaseLoggerConfig
    {
        public DebugLoggerConfig() : this(null)
        { }
        public DebugLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public DebugLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        { }
    }
}
