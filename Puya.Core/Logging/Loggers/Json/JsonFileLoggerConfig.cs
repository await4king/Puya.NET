namespace Puya.Logging
{
    public class JsonFileLoggerConfig : FileLoggerConfig
    {
        public JsonFileLoggerConfig() : this(null)
        { }
        public JsonFileLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public JsonFileLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        {
            FileExtension = ".log";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new JsonLogFormatter();
        }
    }
}
