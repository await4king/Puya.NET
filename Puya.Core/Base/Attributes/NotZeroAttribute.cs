using System;

namespace Puya.Base
{
    // requires a prop to be a non-zero number
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotZeroAttribute : JsonTypeAttribute
    {
        public NotZeroAttribute()
        {
            Type = JsonType.Number | JsonType.String;
        }
    }
}
