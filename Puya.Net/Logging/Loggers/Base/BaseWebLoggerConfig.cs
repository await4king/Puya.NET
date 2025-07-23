namespace Puya.Logging
{
    public abstract class BaseWebLoggerConfig : BaseLoggerConfig, IBaseWebLoggerConfig
    {
        public BaseWebLoggerConfig() : this(null, null)
        { }
        public BaseWebLoggerConfig(ILogFormatter formatter, IWebLoggingPolicy webLoggingPolicy) : base(formatter)
        {
            WebPolicy = webLoggingPolicy;
        }
        public IWebLoggingPolicy WebPolicy { get; set; }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new StringLogFormatter();
        }
    }
}
