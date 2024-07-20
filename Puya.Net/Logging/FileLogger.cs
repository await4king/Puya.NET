using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;
using Puya.Conversion;
using System.Diagnostics;

namespace Puya.Logging
{
    public class FileLoggerConfig : FileLoggerBaseConfigBase
    {
        #region ctor
        public FileLoggerConfig() : this(null, null)
        { }
        public FileLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public FileLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        {
            FileExtension = ".txt";
        }
        #endregion
    }
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
