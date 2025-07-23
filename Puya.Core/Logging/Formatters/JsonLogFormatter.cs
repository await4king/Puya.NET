using Newtonsoft.Json;
using System.Collections.Generic;

namespace Puya.Logging
{
    public class JsonLogFormatter : ILogFormatter
    {
        public bool Indented { get; set; }
        public bool IgnoreNulls { get; set; }
        public Dictionary<string, string> LogParts { get; set; }
        public string LogItems { get; set; }
        public ILogDataConverter DataConverter { get; set; }
        public string Format(Log log)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new JsonLogFormatterPropertyResolver(LogItems)
            };

            if (IgnoreNulls)
            {
                settings.NullValueHandling = NullValueHandling.Ignore;
            }

            var formatting = Indented ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(log, formatting, settings);
        }
        public JsonLogFormatter()
        {
            IgnoreNulls = true;
            Indented = true;
            LogItems = "*";
        }
    }
}
