using System;

namespace Puya.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionAttribute : Attribute
    {
        public string Access { get; set; }
        public string Role { get; set; }
        public PermissionAttribute()
        {
            Access = "";
        }
        public PermissionAttribute(string role, string access)
        {
            Role = role;
            Access = access;
        }
    }
}
