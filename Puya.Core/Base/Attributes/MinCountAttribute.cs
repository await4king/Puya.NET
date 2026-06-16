using System;

namespace Puya.Base
{
    // requires a prop to be a  string containing a list of items separated
    // by comma or a custom separator and the number of them cannot be lower than {count}.
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinCountAttribute : ListAttribute
    {
        public MinCountAttribute(int minCount, string separator = "") : base(minCount, -1, "", false, separator)
        { }
    }
}
