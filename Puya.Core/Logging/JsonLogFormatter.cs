using Puya.Logging.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace Puya.Logging
{
    internal class JsonLogFormatterPropertyResolver : DefaultContractResolver
    {
        internal JsonLogFormatterPropertyResolver(string logProps)
        {
            LogProps = logProps?.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
        internal string[] LogProps { get; set; }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var props = base.CreateProperties(type, memberSerialization);

            if (LogProps == null)
                return new List<JsonProperty>();

            if (Array.IndexOf(LogProps, "*") >= 0)
                return props;

            return props.Where(p => Array.IndexOf(LogProps, p.PropertyName.ToLower()) >= 0).ToList();
        }
    }
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
