using System;

namespace Puya.Base
{
    // requires a prop to be a string whose length cannot be smaller than {value}
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinLenAttribute : JsonTypeAttribute
    {
        public int MinLen { get; set; }
        public MinLenAttribute(int minLen)
        {
            MinLen = minLen;
            RequiresNullCheck = true;
            Type = JsonType.String;
        }
    }
}
