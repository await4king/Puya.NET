namespace Puya.Logging
{
    public class DynamicLoggerConfig : BaseLoggerConfig
    {
        public DynamicLoggerConfig() : this(null)
        { }
        public DynamicLoggerConfig(ILogFormatter formatter) : base(formatter)
        {
            if (formatter == null)
            {
                Formatter = new StringLogFormatter();
            }
        }
        public bool ThrowOnInvalidLoggers { get; set; }
    }
}
