using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ParentAttribute : Attribute
    {
        public string Name { get; set; }
        public ParentAttribute(string name)
        {
            Name = name;
        }
    }
}
