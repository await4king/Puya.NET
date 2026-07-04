using System;

namespace Puya.Base
{
    // requires a prop to be a  string containing an integer number vlaue.
    // validation is done using Validation.IsNumeric() method.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumericIntAttribute : DataTypeAttribute
    {
        public NumericIntAttribute()
        {
            Type = DataType.Number | DataType.String;
        }
    }
}
