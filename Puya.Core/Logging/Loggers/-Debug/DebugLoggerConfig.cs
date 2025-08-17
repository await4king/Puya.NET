namespace Puya.Logging
{
    public class DebugLoggerConfig : BaseLoggerConfig
    {
        public DebugLoggerConfig() : this(null)
        { }
        public DebugLoggerConfig(ILoggingPolicy policy) : base(policy)
        { }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new StringLogFormatter();
        }
    }
}
