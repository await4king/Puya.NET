using Puya.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Conversion
{
    public class Convert : IConvert
    {
        public System.Numerics.BigInteger ToBigInteger(object x)
        {
            var result = new System.Numerics.BigInteger(0);

            if (x != null && !DBNull.Value.Equals(x))
            {
                try
                {
                    var type = x.GetType();

                    if (type == TypeHelper.TypeOfInt32 || type == TypeHelper.TypeOfNullableInt32)
                    {
                        result = new System.Numerics.BigInteger((System.Int32)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfInt16 || type == TypeHelper.TypeOfNullableInt16)
                    {
                        result = new System.Numerics.BigInteger((System.Int16)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfInt64 || type == TypeHelper.TypeOfNullableInt64)
                    {
                        result = new System.Numerics.BigInteger((System.Int64)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfUInt32 || type == TypeHelper.TypeOfNullableUInt32)
                    {
                        result = new System.Numerics.BigInteger((System.UInt32)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfUInt64 || type == TypeHelper.TypeOfNullableUInt64)
                    {
                        result = new System.Numerics.BigInteger((System.UInt64)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfUInt16 || type == TypeHelper.TypeOfNullableUInt16)
                    {
                        result = new System.Numerics.BigInteger((System.UInt16)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfSingle || type == TypeHelper.TypeOfNullableSingle)
                    {
                        result = new System.Numerics.BigInteger((System.Single)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfDouble || type == TypeHelper.TypeOfNullableDouble)
                    {
                        result = new System.Numerics.BigInteger((System.Double)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfDecimal || type == TypeHelper.TypeOfNullableDecimal)
                    {
                        result = new System.Numerics.BigInteger((System.Decimal)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfByte || type == TypeHelper.TypeOfNullableByte)
                    {
                        result = new System.Numerics.BigInteger((System.Byte)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfSByte || type == TypeHelper.TypeOfNullableSByte)
                    {
                        result = new System.Numerics.BigInteger((System.SByte)x);
                    }
                    else
                    if (type == TypeHelper.TypeOfByteArray)
                    {
                        result = new System.Numerics.BigInteger((byte[])x);
                    }
                    else
                    if (type == TypeHelper.TypeOfString)
                    {
                        result = new System.Numerics.BigInteger(Encoding.UTF8.GetBytes(x.ToString()));
                    }
                    else
                    if (type == TypeHelper.TypeOfCharArray)
                    {
                        result = new System.Numerics.BigInteger(Encoding.UTF8.GetBytes(new String((char[])x)));
                    }
                    else
                    if (type == TypeHelper.TypeOfBool || type == TypeHelper.TypeOfNullableBool)
                    {
                        result = new System.Numerics.BigInteger((bool)x ? 1 : 0);
                    }
                    else
                    if (type == TypeHelper.TypeOfChar || type == TypeHelper.TypeOfNullableChar)
                    {
                        var ch = (char)x;

                        if (ch >= '0' && ch <= '9')
                        {
                            result = new System.Numerics.BigInteger((int)ch - 48);
                        }
                    }
                    else
                    if (type == TypeHelper.TypeOfBigInteger || type == TypeHelper.TypeOfNullableBigInteger)
                    {
                        result = (System.Numerics.BigInteger)x;
                    }
                }
                catch
                { }
            }

            return result;
        }
        public long ToInt64(object x)
        {
            return System.Convert.ToInt64(x);
        }

        public int ToInt32(object x)
        {
            return System.Convert.ToInt32(x);
        }

        public short ToInt16(object x)
        {
            return System.Convert.ToInt16(x);
        }

        public decimal ToDecimal(object x)
        {
            return System.Convert.ToDecimal(x);
        }

        public string ToString(object x)
        {
            return System.Convert.ToString(x);
        }

        public char ToChar(object x)
        {
            return System.Convert.ToChar(x);
        }

        public byte ToByte(object x)
        {
            return System.Convert.ToByte(x);
        }

        public sbyte ToSByte(object x)
        {
            return System.Convert.ToSByte(x);
        }

        public bool ToBoolean(object x)
        {
            return System.Convert.ToBoolean(x);
        }

        public DateTime ToDateTime(object x)
        {
            return System.Convert.ToDateTime(x);
        }


        public ulong ToUInt64(object x)
        {
            return System.Convert.ToUInt64(x);
        }

        public uint ToUInt32(object x)
        {
            return System.Convert.ToUInt32(x);
        }

        public ushort ToUInt16(object x)
        {
            return System.Convert.ToUInt16(x);
        }

        public double ToDouble(object x)
        {
            return System.Convert.ToDouble(x);
        }

        public float ToSingle(object x)
        {
            return System.Convert.ToSingle(x);
        }

        public string ToBase64(object x)
        {
            return System.Convert.ToBase64String(ToByteArray(x));
        }

        public byte[] ToByteArray(object x)
        {
            return (byte[])x;
        }
        public List<T> ToList<T>(object x)
        {
            return (List<T>)x;
        }
        public T To<T>(object x) where T : IConvertible
        {
            T result = default(T);

            result = (T)System.Convert.ChangeType(x, typeof(T));

            return result;
        }


        public Guid ToGuid(object x)
        {
            if (x != null)
            {
                return System.Guid.Parse(x.ToString());
            }
            else
            {
                return new Guid();
            }
        }


        public long ToLong(object x)
        {
            return ToInt64(x);
        }

        public short ToShort(object x)
        {
            return ToInt16(x);
        }
        public long? ToInt64Nullable(object x, bool useDefaultForNull = false)
        {
            long? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToInt64(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public Int32? ToInt32Nullable(object x, bool useDefaultForNull = false)
        {
            int? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToInt32(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public Int16? ToInt16Nullable(object x, bool useDefaultForNull = false)
        {
            Int16? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToInt16(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public decimal? ToDecimalNullable(object x, bool useDefaultForNull = false)
        {
            decimal? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToDecimal(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public double? ToDoubleNullable(object x, bool useDefaultForNull = false)
        {
            double? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToDouble(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public Single? ToSingleNullable(object x, bool useDefaultForNull = false)
        {
            Single? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToSingle(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public char? ToCharNullable(object x, bool useDefaultForNull = false)
        {
            char? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToChar(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = default(char);
                    }
                }
            }
            return result;
        }
        public byte? ToByteNullable(object x, bool useDefaultForNull = false)
        {
            byte? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToByte(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public sbyte? ToSByteNullable(object x, bool useDefaultForNull = false)
        {
            sbyte? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToSByte(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }
        public bool? ToBooleanNullable(object x, bool useDefaultForNull = false)
        {
            bool? result = null;
            if (x != null)
            {
                if (x.GetType() == TypeHelper.TypeOfString)
                {
                    var s = ToString(x).Trim();
                    int a;
                    if (Int32.TryParse(s, out a))
                    {
                        if (a != 0)
                            result = true;
                    }
                    else
                    {
                        double d;
                        if (double.TryParse(s, out d))
                        {
                            if (d != 0)
                                result = true;
                        }
                        else
                        {
                            decimal c;
                            if (decimal.TryParse(s, out c))
                            {
                                if (c != 0)
                                    result = true;
                            }
                            else
                            {
                                if (string.Compare(s, "true", true) == 0)
                                    result = true;
                            }
                        }
                    }
                }
                else
                {
                    result = System.Convert.ToBoolean(x);
                }
            }
            else
            {
                if (useDefaultForNull)
                {
                    result = false;
                }
            }
            return result;
        }
        public DateTime? ToDateTimeNullable(object x, bool useDefaultForNull = false)
        {
            DateTime? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToDateTime(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = default(DateTime);
                    }
                }
            }
            return result;
        }
        public Guid? ToGuidNullable(object x, bool useDefaultForNull = false)
        {
            Guid? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    Guid g;
                    if (System.Guid.TryParse(x.ToString(), out g))
                    {
                        result = g;
                    }
                    else
                    {
                        result = default(Guid);
                    }
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = default(Guid);
                    }
                }
            }
            return result;
        }
        public long? ToLongNullable(object x, bool useDefaultForNull = false)
        {
            return ToInt64Nullable(x, useDefaultForNull);
        }

        public short? ToShortNullable(object x, bool useDefaultForNull = false)
        {
            return ToInt16Nullable(x, useDefaultForNull);
        }

        public ulong? ToUInt64Nullable(object x, bool useDefaultForNull = false)
        {
            ulong? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToUInt64(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        public uint? ToUInt32Nullable(object x, bool useDefaultForNull = false)
        {
            uint? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToUInt32(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        public ushort? ToUInt16Nullable(object x, bool useDefaultForNull = false)
        {
            ushort? result = null;
            if (!DBNull.Value.Equals(x))
            {
                if (x != null)
                {
                    result = System.Convert.ToUInt16(x);
                }
                else
                {
                    if (useDefaultForNull)
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        public T ChangeType<T>(object value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }
        public object ChangeType(object value, Type type)
        {
            return System.Convert.ChangeType(value, type);
        }
    }
}
