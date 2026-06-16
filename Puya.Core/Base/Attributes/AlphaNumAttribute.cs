using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaNumAttribute : JsonTypeAttribute // requires a prop to be a string containing letters or digits characters only
    {
        public AlphaNumAttribute()
        {
            Type = JsonType.String;
        }
    }
}
