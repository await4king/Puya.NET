using System;

namespace Puya.ServiceModel
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class AuditAttribute : Attribute
    { }
}
