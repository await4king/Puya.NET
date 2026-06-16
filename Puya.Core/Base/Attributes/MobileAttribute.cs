using System;

namespace Puya.Base
{
    // requires a prop to be a string containing a mobile number.
    // validating a mobile number is done using a Validation.IsMobile()
    // method if no pattern specified, otherwise the custom pattern will be used
    // for validation.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MobileAttribute : RegExpAttribute   // requires a prop to be a string containing a mobile number
    {
        public MobileAttribute() : this("")
        { }
        public MobileAttribute(string pattern) : base(pattern)
        { }
    }
}
