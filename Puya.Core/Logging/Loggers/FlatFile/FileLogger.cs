using System;
using System.Collections.Generic;

namespace Puya.Logging
{
    public class FileLogger : FileLoggerBase<FileLoggerConfig>
    {
        public FileLogger() : this(null, null)
        { }
        public FileLogger(FileLoggerConfig config) : this(config, null)
        { }
        public FileLogger(FileLoggerConfig config, ILogger next) : base(config, next)
        { }

        public override List<Log> LoadLogFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
