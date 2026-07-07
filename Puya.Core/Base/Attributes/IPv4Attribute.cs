using System;

namespace Puya.Base
{
    // requires a prop to be a string containing an IPv4 address.
    // validation is done using Validation.IsIPv4() method.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IPv4Attribute : RegExpAttribute   // requires a prop to be a string containing an email address
    {
        public IPv4Attribute(bool mask = false) : base("")
        {
            Mask = mask;
        }

        public bool Mask { get; }
    }
}
