using System;

namespace Puya.Base
{
    public enum RangeType
    {
        Byte,
        Short,
        Integer,
        Long,
        Float,
        Double,
        Decimal
    }
    // requires a prop to be a numeric value ranging between a from/to values.
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : DataTypeAttribute
    {
        public decimal FromDec { get; set; }
        public decimal ToDec { get; set; }
        public RangeType RangeType { get; protected set; }
        public RangeAttribute(decimal from, decimal to)
        {
            FromDec = from;
            ToDec = to;
            RangeType = RangeType.Decimal;
            Type = DataType.Number | DataType.String;
        }
        public short FromShort { get; set; }
        public short ToShort { get; set; }
        public RangeAttribute(short from, short to)
        {
            FromShort = from;
            ToShort = to;
            RangeType = RangeType.Short;
            Type = DataType.Number | DataType.String;
        }
        public int FromInt { get; set; }
        public int ToInt { get; set; }
        public RangeAttribute(int from, int to)
        {
            FromInt = from;
            ToInt = to;
            RangeType = RangeType.Integer;
            Type = DataType.Number | DataType.String;
        }
        public byte FromByte { get; set; }
        public byte ToByte { get; set; }
        public RangeAttribute(byte from, byte to)
        {
            FromByte = from;
            ToByte = to;
            RangeType = RangeType.Byte;
            Type = DataType.Number | DataType.String;
        }
        public long FromLong { get; set; }
        public long ToLong { get; set; }
        public RangeAttribute(long from, long to)
        {
            FromLong = from;
            ToLong = to;
            RangeType = RangeType.Long;
            Type = DataType.Number | DataType.String;
        }
        public float FromFloat { get; set; }
        public float ToFloat { get; set; }
        public RangeAttribute(float from, float to)
        {
            FromFloat = from;
            ToFloat = to;
            RangeType = RangeType.Float;
            Type = DataType.Number | DataType.String;
        }
        public double FromDouble { get; set; }
        public double ToDouble { get; set; }
        public RangeAttribute(double from, double to)
        {
            FromDouble = from;
            ToDouble = to;
            RangeType = RangeType.Double;
            Type = DataType.Number | DataType.String;
        }
    }
}
