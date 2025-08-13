using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

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
            File.WriteAllText(path, data);
        }
        protected override void Append(string path, string data)
        {
            File.AppendAllText(path, Environment.NewLine + data);
        }

        public override List<Log> LoadLogFile(string path)
        {
            var result = new List<Log>();

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                var serializer = new XmlSerializer(typeof(LogListWrapper));

                using (var reader = new StringReader("<Logs>" + content + "</Logs>"))
                {
                    var wrapper = (LogListWrapper)serializer.Deserialize(reader);

                    foreach (var l in wrapper.Items)
                    {
                        var log = l.ToLog();

                        result.Add(log);
                    }
                }
            }

            return result;
        }
    }
}
