using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OutputAttribute : Attribute
    {
        public dynamic Type { get; set; }
        public int? Size { get; set; }
        public string TypeProp { get; set; }
        public OutputAttribute(int size)
        {
            Size = size;
        }
        public OutputAttribute(dynamic type)
        {
            Type = type;
        }
        public OutputAttribute(dynamic type, int size)
        {
            Type = type;
            Size = size;
        }
        public OutputAttribute(string size)
        {
            this.Size = string.Compare(size, "max", StringComparison.OrdinalIgnoreCase) == 0 ? -1 : System.Convert.ToInt32(size);
        }
        public OutputAttribute(dynamic type, string size)
        {
            Type = type;
            this.Size = string.Compare(size, "max", StringComparison.OrdinalIgnoreCase) == 0 ? -1 : System.Convert.ToInt32(size);
        }
        public OutputAttribute(dynamic type, string size, string typeProp)
        {
            Type = type;
            TypeProp = typeProp;
            this.Size = string.Compare(size, "max", StringComparison.OrdinalIgnoreCase) == 0 ? -1 : System.Convert.ToInt32(size);
        }
        public OutputAttribute(dynamic type, int size, string typeProp)
        {
            Type = type;
            TypeProp = typeProp;
            Size = size;
        }
    }
}
