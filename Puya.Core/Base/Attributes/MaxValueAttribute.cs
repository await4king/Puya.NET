using System;

namespace Puya.Base
{
    // requires a prop whose value cannot be greater than {value}
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxValueAttribute : JsonTypeAttribute
    {
        public decimal MaxValue { get; set; }
        public MaxValueAttribute(decimal value)
        {
            MaxValue = value;
            Type = JsonType.Number | JsonType.String;
        }
    }
}
