using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class XmlFileLoggerConfig : FileLoggerConfig
    {
        public string RootTag { get; set; }
        public override string FileExtension { get; set; }
        #region ctor
        public XmlFileLoggerConfig() : this(null, null)
        { }
        public XmlFileLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public XmlFileLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        {
            FileExtension = ".xml";
            RootTag = "Logs";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new XmlStringLogFormatter();
        }
        #endregion
    }
    public class XmlFileLogger: FileLoggerBase<XmlFileLoggerConfig>
    {
        public XmlFileLogger() : this(null, null)
        { }
        public XmlFileLogger(FileLoggerConfig config) : this(config, null)
        { }
        public XmlFileLogger(FileLoggerConfig config, ILogger next) : base(config, next)
        {
        }
        protected override void Write(string path, string data)
        {
            var lines = new string[]
                {
                    $"<{StrongConfig.RootTag}>",
                    "\t" + data,
                    $"</{StrongConfig.RootTag}>"
                };

            File.WriteAllLines(path, lines);
        }
        protected override void Append(string path, string data)
        {
            string[] all;

            if (File.Exists(path))
            {
                all = File.ReadAllLines(path);
            }
            else
            {
                all = new string[] { };
            }

            string[] lines;

            data = "\t" + data;

            if (all.Length > 0)
            {
                lines = new string[all.Length + 1];

                lines[0] = all[0];
                lines[lines.Length - 2] = data;
                lines[lines.Length - 1] = all[all.Length - 1];

                Array.Copy(all, 1, lines, 1, all.Length - 2);
            }
            else
            {
                lines = new string[]
                {
                    $"<{StrongConfig.RootTag}>",
                    data,
                    $"</{StrongConfig.RootTag}>"
                };
            }

            File.WriteAllLines(path, lines);
        }

        public override List<Log> LoadLogFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
