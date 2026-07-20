using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class PKAttribute : Attribute
    {
        public PKAttribute()
        { }
        public PKAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}
