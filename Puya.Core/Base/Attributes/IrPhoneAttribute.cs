using System;

namespace Puya.Base
{
    // requires a prop to be a string containing a phone number.
    // validation is done using Validation.IsPhone() or
    // through a custom regexp pattern.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IrPhoneAttribute : RegExpAttribute   // requires a prop to be a string containing a phone number
    {
        public IrPhoneAttribute() : this("")
        { }
        public IrPhoneAttribute(string pattern) : base(pattern)
        { }
    }
}
