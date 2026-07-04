using System;

namespace Puya.Base
{
    // requires a prop to be a string whose characters cannot fall
    // withing the characters in the {excludedCharacters} string.
    [AttributeUsage(AttributeTargets.Property)]
    public class PreventCharsAttribute : DataTypeAttribute
    {
        public string ExcludedCharacters { get; set; }
        public PreventCharsAttribute(string excludedCharacters)
        {
            ExcludedCharacters = excludedCharacters;
            Type = DataType.String;
        }
    }
}
