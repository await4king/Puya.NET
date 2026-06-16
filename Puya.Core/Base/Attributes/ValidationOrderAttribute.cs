using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidationOrderAttribute : ValidationAttribute
    {
        public int Order { get; }

        public ValidationOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
