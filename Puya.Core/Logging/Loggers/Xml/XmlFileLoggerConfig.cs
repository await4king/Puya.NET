namespace Puya.Logging
{
    public class XmlFileLoggerConfig : FileLoggerConfig
    {
        public XmlFileLoggerConfig() : this(null)
        { }
        public XmlFileLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public XmlFileLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        {
            FileExtension = ".xml";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new XmlLogFormatter();
        }
    }
}
