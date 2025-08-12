namespace Puya.Logging
{
    public class DynamicLoggerConfig : BaseLoggerConfig
    {
        public DynamicLoggerConfig() : this(null)
        { }
        public DynamicLoggerConfig(ILogFormatter formatter) : base(formatter)
        { }
        public bool ThrowOnInvalidLoggers { get; set; }
    }
}
