using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OrderAttribute : ValidationAttribute
    {
        public int Order { get; }
        public string Subject { get; }

        public OrderAttribute(int order, string subject = "")
        {
            Order = order;
            Subject = subject;
        }
    }
}
