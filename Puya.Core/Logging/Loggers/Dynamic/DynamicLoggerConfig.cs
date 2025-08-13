namespace Puya.Logging
{
    public class DynamicLoggerConfig : BaseLoggerConfig
    {
        public DynamicLoggerConfig() : this(null)
        { }
        public DynamicLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public DynamicLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        { }
        public bool ThrowOnInvalidLoggers { get; set; }
    }
}
