using System;

namespace Puya.Base
{
    // requires a prop to be a string containing a comma separated list of values.
    // validating each item is done through a regexp pattern given to this attribute.
    // for validation.
    [AttributeUsage(AttributeTargets.Property)]
    public class RegExpsAttribute : ListAttribute // requires a prop to be a string containing a comma separated list of email addresses
    {
        public RegExpsAttribute(string pattern, int minCount, int maxCount = -1) : base(minCount, maxCount, pattern)
        { }
    }
}
