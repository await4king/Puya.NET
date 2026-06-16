using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EmailsAttribute : ListAttribute // requires a prop to be a string containing a comma separated list of email addresses
    {
        public EmailsAttribute(int minCount, int maxCount = -1, string pattern = ""): base(minCount, maxCount, pattern)
        { }
    }
}
