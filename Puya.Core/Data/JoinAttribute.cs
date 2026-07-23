using System;

namespace Puya.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class JoinAttribute : Attribute
    {
        public string Separator { get; set; }
        public JoinAttribute()
        {
            Separator = ",";
        }
        public JoinAttribute(string separator)
        {
            this.Separator = separator;
        }
    }
}
