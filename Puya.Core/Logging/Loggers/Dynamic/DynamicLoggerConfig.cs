namespace Puya.Logging
{
    public class DynamicLoggerConfig : BaseLoggerConfig
    {
        public DynamicLoggerConfig() : this(null)
        { }
        public DynamicLoggerConfig(ILoggingPolicy policy) : base(policy)
        { }
        public bool ThrowOnInvalidLoggers { get; set; }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new JsonLogFormatter();
        }
    }
}
