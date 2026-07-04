using System;

namespace Puya.Base
{
    [Flags]
    public enum DataType
    {
        Any = 31,
        String = 1,
        Number = 2,
        Boolean = 4,
        Array = 8,
        Object = 16
    }
    public class DataTypeAttribute: ValidationAttribute
    {
        public DataType Type { get; protected set; }
    }
}
