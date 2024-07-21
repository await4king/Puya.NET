using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ActionNameAttribute : Attribute
    {
        public string Name { get; set; }
        public ActionNameAttribute()
        {
            Name = "";
        }
        public ActionNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}
