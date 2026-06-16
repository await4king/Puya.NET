using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class FullValidation : Attribute
    { }
}
