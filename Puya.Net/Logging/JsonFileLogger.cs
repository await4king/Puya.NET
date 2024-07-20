using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class JsonFileLoggerConfig : FileLoggerConfig
    {
        public override string FileExtension { get; set; }
        #region ctor
        public JsonFileLoggerConfig() : this(null, null)
        { }
        public JsonFileLoggerConfig(ILogDataConverter dataConverter) : this(null, null)
        { }
        public JsonFileLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        {
            FileExtension = ".json";
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new JsonLogFormatter();
        }
        #endregion
    }
    public class JsonFileLogger : FileLoggerBase<JsonFileLoggerConfig>
    {
        public JsonFileLogger() : this(null, null)
        { }
        public JsonFileLogger(FileLoggerConfig config) : this(config, null)
        { }
        public JsonFileLogger(FileLoggerConfig config, ILogger next) : base(config, next)
        {
        }
        protected override void Write(string path, string data)
        {
            var lines = new string[]
                {
                    "[",
                    "\t" + data,
                    "]"
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

            if (all.Length > 0)
            {
                lines = new string[all.Length + 1];

                lines[0] = all[0];
                lines[lines.Length - 2] = "\t, " + data;
                lines[lines.Length - 1] = all[all.Length - 1];

                Array.Copy(all, 1, lines, 1, all.Length - 2);
            }
            else
            {
                lines = new string[]
                {
                    "[",
                    data,
                    "]"
                };
            }

            File.WriteAllLines(path, lines);
        }

        public override List<Log> LoadLogFile(string path)
        {
            var result = new List<Log>();

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);

                try
                {
                    result = JsonConvert.DeserializeObject<List<Log>>(content);
                }
                catch (Exception e)
                {
                    Next?.Danger(e);
                }
            }

            return result;
        }
    }
}
