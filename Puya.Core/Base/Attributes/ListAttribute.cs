using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListAttribute : DataTypeAttribute // requires a prop to be a string containing a comma separated list of items
    {
        public ListAttribute(int minCount, int maxCount = -1, string pattern = "", bool ignoreCase = false, string separator = ",")
        {
            Pattern = pattern;

            if (string.IsNullOrEmpty(separator))
            {
                separator = ",";
            }

            Separator = separator;
            MinCount = minCount;
            MaxCount = maxCount;
            Type = DataType.String;
            IgnoreCase = ignoreCase;

            if (MinCount > 0)
            {
                RequiresNullCheck = true;
            }
        }
        public string Separator { get; set; }
        public string Pattern { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public bool IgnoreCase { get; set; }
    }
}
