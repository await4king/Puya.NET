using System;

namespace Puya.Base
{
    // requires a prop to be a string conforming to a regexp pattern
    [AttributeUsage(AttributeTargets.Property)]
    public class RegExpAttribute : JsonTypeAttribute
    {
        public string Pattern { get; set; }
        public RegExpAttribute(string pattern)
        {
            Pattern = pattern;
            Type = JsonType.String;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class PatternAttribute : RegExpAttribute
    {
        public PatternAttribute(string pattern): base(pattern)
        { }
    }
}
