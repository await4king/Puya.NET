using System;

namespace Puya.Base
{
    // requires a prop to be a string whose length must be {value}
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LenAttribute : DataTypeAttribute
    {
        public int Value { get; set; }
        public LenAttribute(int value)
        {
            Value = value;
            Type = DataType.String;
        }
    }
}
