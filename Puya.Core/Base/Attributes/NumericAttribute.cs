using System;

namespace Puya.Base
{
    // requires a prop to be a  string containing a numeric vlaue.
    // validation is done using Validation.IsNumeric() method.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumericAttribute : JsonTypeAttribute
    {
        public NumericAttribute(decimal value)
        {
            Type = JsonType.Number | JsonType.String;
        }
    }
}
