using System;

namespace Puya.Base
{
    // requires a prop to be a string containing a comma separated list of mobile numbers.
    // validating a mobile number is done using a Validation.IsMobile()
    // method if no pattern specified, otherwise the custom pattern will be used
    // for validation.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MobilesAttribute : ListAttribute // requires a prop to be a string containing a comma separated list of email addresses
    {
        public MobilesAttribute(int minCount, int maxCount = -1, string pattern = "") : base(minCount, maxCount, pattern)
        { }
    }
}
