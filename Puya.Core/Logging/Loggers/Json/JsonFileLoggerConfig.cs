namespace Puya.Logging
{
    public class JsonFileLoggerConfig : FileLoggerConfig
    {
        public override string FileExtension { get; set; }
        public JsonFileLoggerConfig() : this(null)
        { }
        public JsonFileLoggerConfig(ILogFormatter formatter) : base(formatter)
        {
            FileExtension = ".json";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new JsonLogFormatter();
        }
    }
}
