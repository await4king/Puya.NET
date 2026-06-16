using System;

namespace Puya.Base
{
    // requires a prop to be not-null value and a not-empty or zero-length string.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttribute
    {
        public bool IncludeEmptyStrings { get; set; }
        public bool IncludeWhiteStrings { get; set; }
        public RequiredAttribute(bool includeEmptyStrings = true, bool includeWhiteStrings = false)
        {
            IncludeEmptyStrings = includeEmptyStrings;
            IncludeWhiteStrings = includeWhiteStrings;
        }
    }
}
