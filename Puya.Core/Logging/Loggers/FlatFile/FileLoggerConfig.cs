namespace Puya.Logging
{
    public class FileLoggerConfig : FileLoggerConfigBase
    {
        public FileLoggerConfig() : this(null)
        { }
        public FileLoggerConfig(ILoggingPolicy policy) : base(policy)
        {
            FileExtension = ".txt";
        }
    }
}
