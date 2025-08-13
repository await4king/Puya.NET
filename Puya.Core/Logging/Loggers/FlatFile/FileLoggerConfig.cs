namespace Puya.Logging
{
    public class FileLoggerConfig : FileLoggerConfigBase
    {
        public FileLoggerConfig() : this(null)
        { }
        public FileLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public FileLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        {
            FileExtension = ".txt";
        }
    }
}
