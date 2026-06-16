using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ListAttribute : JsonTypeAttribute // requires a prop to be a string containing a comma separated list of items
    {
        public ListAttribute(int minCount, int maxCount = -1, string pattern = "", bool ignoreCase = false, string separator = ",")
        {
            Pattern = pattern;
            Separator = separator;
            MinCount = minCount;
            MaxCount = maxCount;
            Type = JsonType.String;
            IgnoreCase = ignoreCase;
        }
        public string Separator { get; set; }
        public string Pattern { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public bool IgnoreCase { get; set; }
    }
}
