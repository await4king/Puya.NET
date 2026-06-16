using System;

namespace Puya.Base
{
    // requires a prop to be a  string containing a list of items separated
    // by comma or a custom separator and the number of them cannot be greater than {count}.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxCountAttribute : ListAttribute
    {
        public MaxCountAttribute(int maxCount, string separator = "") : base(0, maxCount, "", false, separator)
        { }
    }
}
