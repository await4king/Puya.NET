using System;

namespace Puya.Base
{
    // requires a prop to be a non-zero number
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotZeroAttribute : DataTypeAttribute
    {
        public NotZeroAttribute()
        {
            Type = DataType.Number | DataType.String;
        }
    }
}
