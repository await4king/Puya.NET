using System;

namespace Puya.Base
{
    // requires a prop to be a string containing list of items where
    // each item can be chosen only from the items specified for this attribute.
    // items are assumed to be separated by default by comma or a custom separator
    // specified for this attribute
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyOfAttribute : ListAttribute
    {
        public string Items { get; set; }
        public ManyOfAttribute(string items, int minCount, int maxCount = -1, bool ignoreCase = false, string separator = ",") : base(minCount, maxCount, "", ignoreCase, separator)
        {
            Items = items;
        }
    }
}
