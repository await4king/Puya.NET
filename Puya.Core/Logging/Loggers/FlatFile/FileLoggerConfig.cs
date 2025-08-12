namespace Puya.Logging
{
    public class FileLoggerConfig : FileLoggerConfigBase
    {
        public FileLoggerConfig() : this(null)
        { }
        public FileLoggerConfig(ILogFormatter formatter) : base(formatter)
        {
            FileExtension = ".txt";
        }
    }
}
