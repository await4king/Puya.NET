using System;

namespace Puya.Base
{
    // requires a prop to be a  string containing a numeric vlaue.
    // validation is done using Validation.IsNumeric() method.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumericAttribute : DataTypeAttribute
    {
        public NumericAttribute()
        {
            Type = DataType.Number | DataType.String;
        }
    }
}
