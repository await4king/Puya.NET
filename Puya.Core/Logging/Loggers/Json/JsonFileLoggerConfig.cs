namespace Puya.Logging
{
    public class JsonFileLoggerConfig : FileLoggerConfig
    {
        public JsonFileLoggerConfig() : this(null)
        { }
        public JsonFileLoggerConfig(ILoggingPolicy policy) : base(policy)
        {
            FileExtension = ".log";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new JsonLogFormatter();
        }
    }
}
