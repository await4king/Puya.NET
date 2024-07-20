using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ServiceNameAttribute()
        {
            Name = "";
        }
        public ServiceNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
