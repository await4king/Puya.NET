using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinValueAttribute : JsonTypeAttribute
    {
        public decimal MinValue { get; set; }
        public MinValueAttribute(decimal value)
        {
            MinValue = value;
            Type = JsonType.Number | JsonType.String;
        }
    }
}
