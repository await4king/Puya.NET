using System;

namespace Puya.Base
{
    [Flags]
    public enum JsonType
    {
        Any = 0,
        String = 1,
        Number = 2,
        Boolean = 3,
        Array = 4,
        Object = 5
    }
    public class JsonTypeAttribute: ValidationAttribute
    {
        public JsonType Type { get; protected set; }
    }
}
