using System;

namespace Puya.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SprocPrefixAttribute : Attribute
    {
        public string Prefix { get; set; }
        public SprocPrefixAttribute(string prefix)
        {
            this.Prefix = prefix;
        }
    }
}
