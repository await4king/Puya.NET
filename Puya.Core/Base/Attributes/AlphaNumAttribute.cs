using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AlphaNumAttribute : DataTypeAttribute // requires a prop to be a string containing letters or digits characters only
    {
        public AlphaNumAttribute()
        {
            Type = DataType.String;
        }
    }
}
