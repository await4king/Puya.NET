namespace Puya.Logging
{
    public class XmlFileLoggerConfig : FileLoggerConfig
    {
        public XmlFileLoggerConfig() : this(null)
        { }
        public XmlFileLoggerConfig(ILoggingPolicy policy) : base(policy)
        {
            FileExtension = ".xml";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new XmlLogFormatter();
        }
    }
}
