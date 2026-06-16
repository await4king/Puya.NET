using System;

namespace Puya.Base
{
    // requires a prop to be a string whose value can only be chosen from a list
    // of items specified for this attribute.
    [AttributeUsage(AttributeTargets.Property)]
    public class OneOfAttribute : ListAttribute
    {
        public string Items { get; set; }
        public OneOfAttribute(string items, bool ignoreCase = false, string separator = ",") : base(0, 1, "", ignoreCase, separator)
        {
            Items = items;
        }
    }
}
