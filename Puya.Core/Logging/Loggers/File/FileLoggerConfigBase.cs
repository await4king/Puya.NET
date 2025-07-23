using System;

namespace Puya.Logging
{
    public abstract class FileLoggerConfigBase : BaseLoggerConfig
    {
        public string FileName { get; set; }
        public virtual string FileExtension { get; set; }
        public string Path { get; set; }
        public int MaxSize { get; set; }
        public int MaxChunk { get; set; }
        public bool Repeat { get; set; }
        #region ctor
        public FileLoggerConfigBase() : this(null)
        { }
        public FileLoggerConfigBase(ILogFormatter formatter) : base(formatter)
        {
            FileName = "log";
            FileExtension = ".log";
            Path = Environment.CurrentDirectory;
            MaxSize = -1;
            MaxChunk = -1;
            Repeat = false;
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new StringLogFormatter();
        }
        #endregion
    }
}
