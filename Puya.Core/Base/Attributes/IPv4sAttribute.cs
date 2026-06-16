using System;

namespace Puya.Base
{
    // requires a prop to be a string containing an IPv4 address.
    // validation is done using Validation.IsIPv4() method.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IPv4sAttribute : ListAttribute // requires a prop to be a string containing a comma separated list of email addresses
    {
        public IPv4sAttribute(int minCount, int maxCount = -1) : base(minCount, maxCount)
        { }
    }
}
