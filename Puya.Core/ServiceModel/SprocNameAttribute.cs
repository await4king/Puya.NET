using System;

namespace Puya.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SprocNameAttribute : Attribute
    {
        public string Name { get; set; }
        public SprocNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
