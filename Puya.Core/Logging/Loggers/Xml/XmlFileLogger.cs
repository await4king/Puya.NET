using System;
using System.Collections.Generic;
using System.IO;

namespace Puya.Logging
{
    public class XmlFileLogger: FileLoggerBase<XmlFileLoggerConfig>
    {
        public XmlFileLogger() : this(null, null)
        { }
        public XmlFileLogger(XmlFileLoggerConfig config) : this(config, null)
        { }
        public XmlFileLogger(XmlFileLoggerConfig config, ILogger next) : base(config, next)
        {
        }
        protected override void Write(string path, string data)
        {
            var lines = new string[]
                {
                    $"<{Config.RootTag}>",
                    "\t" + data,
                    $"</{Config.RootTag}>"
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
                    $"<{Config.RootTag}>",
                    data,
                    $"</{Config.RootTag}>"
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
