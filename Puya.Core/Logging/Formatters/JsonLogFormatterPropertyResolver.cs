using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
