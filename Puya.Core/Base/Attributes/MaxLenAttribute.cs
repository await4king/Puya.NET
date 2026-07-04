using System;

namespace Puya.Base
{
    // requires a prop to be a string whose length cannot be greater than {value}
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxLenAttribute : DataTypeAttribute
    {
        public int MaxLen { get; set; }
        public MaxLenAttribute(int value)
        {
            MaxLen = value;
            Type = DataType.String;
        }
    }
}
