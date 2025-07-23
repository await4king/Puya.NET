namespace Puya.Logging
{
    public class WebDbLoggerConfig : DbLoggerConfig, IBaseWebLoggerConfig
    {
        public WebDbLoggerConfig() : this(null, null)
        { }
        public WebDbLoggerConfig(ILogFormatter formatter, IWebLoggingPolicy webLoggingPolicy) : base(formatter)
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
