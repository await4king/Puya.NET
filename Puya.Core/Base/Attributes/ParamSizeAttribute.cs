using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ParamSizeAttribute : Attribute
    {
        public int? Value { get; set; }
        public ParamSizeAttribute(int size)
        {
            Value = size;
        }
        public ParamSizeAttribute(string value)
        {
            this.Value = string.Compare(value, "max", StringComparison.OrdinalIgnoreCase) == 0 ? -1 : System.Convert.ToInt32(value);
        }
    }
}
