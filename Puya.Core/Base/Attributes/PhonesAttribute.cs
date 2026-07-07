using System;

namespace Puya.Base
{
    // requires a prop to be a string containing a comma separated list of phone numbers.
    // validating a mobile number is done using a Validation.IsPhone()
    // method if no pattern specified, otherwise the custom pattern will be used
    // for validation.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PhonesAttribute : ListAttribute // requires a prop to be a string containing a comma separated list of email addresses
    {
        public PhonesAttribute(int minCount = 0, int maxCount = -1, string pattern = "") : base(minCount, maxCount, pattern)
        { }
    }
}
