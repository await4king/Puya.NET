using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaAttribute : DataTypeAttribute // requires a prop to be a string containing alphabetic characters only
    {
        public AlphaAttribute()
        {
            Type = DataType.String;
        }
    }
}
