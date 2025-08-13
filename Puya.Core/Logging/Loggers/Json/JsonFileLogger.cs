using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Puya.Logging
{
    public class JsonFileLogger : FileLoggerBase<JsonFileLoggerConfig>
    {
        public JsonFileLogger() : this(null, null)
        { }
        public JsonFileLogger(JsonFileLoggerConfig config) : this(config, null)
        { }
        public JsonFileLogger(JsonFileLoggerConfig config, ILogger next) : base(config, next)
        {
        }
        protected override void Write(string path, string data)
        {
            File.WriteAllText(path, data);
        }
        protected override void Append(string path, string data)
        {
            var noAppend = !File.Exists(path) || new FileInfo(path).Length == 0;

            if (noAppend)
            {
                Write(path, data);
            }
            else
            {
                data = "," + Environment.NewLine + data;
            }

            File.AppendAllText(path, data);
        }

        public override List<Log> LoadLogFile(string path)
        {
            var result = new List<Log>();

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);

                try
                {
                    result = JsonConvert.DeserializeObject<List<Log>>("[" + content + "]");
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
