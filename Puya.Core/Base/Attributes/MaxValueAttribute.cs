using System;

namespace Puya.Base
{
    // requires a prop whose value cannot be greater than {value}
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxValueAttribute : DataTypeAttribute
    {
        public decimal Value { get; set; }
        MaxValueAttribute()
        {
            Type = DataType.Number | DataType.String;
        }
        public MaxValueAttribute(float value) : this()
        {
            Value = (decimal)value;
        }
        public MaxValueAttribute(double value) : this()
        {
            Value = (decimal)value;
        }
        public MaxValueAttribute(byte value) : this()
        {
            Value = value;
        }
        public MaxValueAttribute(short value) : this()
        {
            Value = value;
        }
        public MaxValueAttribute(long value) : this()
        {
            Value = value;
        }
        public MaxValueAttribute(sbyte value) : this()
        {
            Value = value;
        }
        public MaxValueAttribute(ushort value) : this()
        {
            Value = value;
        }
        public MaxValueAttribute(uint value) : this()
        {
            Value = value;
        }
        public MaxValueAttribute(ulong value) : this()
        {
            Value = value;
        }
    }
}
