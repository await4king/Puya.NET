using System;

namespace Puya.Base
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MinValueAttribute : DataTypeAttribute
    {
        public decimal Value { get; set; }
        MinValueAttribute()
        {
            Type = DataType.Number | DataType.String;
        }
        public MinValueAttribute(float value) : this()
        {
            Value = (decimal)value;
        }
        public MinValueAttribute(double value) : this()
        {
            Value = (decimal)value;
        }
        public MinValueAttribute(byte value) : this()
        {
            Value = value;
        }
        public MinValueAttribute(short value) : this()
        {
            Value = value;
        }
        public MinValueAttribute(long value) : this()
        {
            Value = value;
        }
        public MinValueAttribute(sbyte value) : this()
        {
            Value = value;
        }
        public MinValueAttribute(ushort value) : this()
        {
            Value = value;
        }
        public MinValueAttribute(uint value) : this()
        {
            Value = value;
        }
        public MinValueAttribute(ulong value) : this()
        {
            Value = value;
        }
    }
}
