using Newtonsoft.Json;
using System.Collections.Generic;

namespace Puya.Logging
{
    public class JsonLogFormatter : ILogFormatter
    {
        public string Format(Log log)
        {
            var _log = new Log(log);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            _log.Data = log.GetData == null ? log.Data : log.GetData();

            return JsonConvert.SerializeObject(log, Formatting.Indented, settings);
        }
    }
}
