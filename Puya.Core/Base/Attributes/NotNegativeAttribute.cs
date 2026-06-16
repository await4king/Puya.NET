using System;

namespace Puya.Base
{
    // requires a prop to be a non-negative number
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotNegativeAttribute : JsonTypeAttribute
    {
        public NotNegativeAttribute()
        {
            Type = JsonType.Number | JsonType.String;
        }
    }
}
