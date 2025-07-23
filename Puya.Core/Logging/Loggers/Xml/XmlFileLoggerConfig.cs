namespace Puya.Logging
{
    public class XmlFileLoggerConfig : FileLoggerConfig
    {
        public string RootTag { get; set; }
        public override string FileExtension { get; set; }
        public XmlFileLoggerConfig() : this(null)
        { }
        public XmlFileLoggerConfig(ILogFormatter formatter) : base(formatter)
        {
            FileExtension = ".xml";
            RootTag = "Logs";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new XmlStringLogFormatter();
        }
    }
}
