using System;

namespace Puya.Base
{
    // requires a prop to be a string containing an email address.
    // validating an email is done using a Validation.IsEmail()
    // method if no pattern specified, otherwise the custom pattern will be used
    // for validation.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAttribute : RegExpAttribute   // requires a prop to be a string containing an email address
    {
        public EmailAttribute(): this("")
        { }
        public EmailAttribute(string pattern) : base(pattern)
        { }
    }
}
