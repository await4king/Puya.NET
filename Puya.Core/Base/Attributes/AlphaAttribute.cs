using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaAttribute : JsonTypeAttribute // requires a prop to be a string containing alphabetic characters only
    {
        public AlphaAttribute()
        {
            Type = JsonType.String;
        }
    }
}
