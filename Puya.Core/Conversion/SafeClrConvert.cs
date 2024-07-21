using System;
using System.Text;
using Puya.Base;

namespace Puya.Conversion
{
    public static partial class SafeClrConvert
    {
        #region Non-Nullable
        public static System.Numerics.BigInteger ToBigInteger(object x, System.Numerics.BigInteger defaultValue = default(System.Numerics.BigInteger))
        {
            var result = defaultValue;

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
        public static string ToString(object x)
        {
            string result = "";

            try
            {
                if (!(x == null || DBNull.Value.Equals(x)))
                {
                    result = System.Convert.ToString(x);
                }
            }
            catch
            { }

            return result;
        }
        public static bool ToBoolean(object x, bool @default = default(bool))
        {
            bool result;

            try
            {
                for (;;)
                {
                    var s = x as string;
                    
                    if (!(DBNull.Value.Equals(x) || x == null || (s != null && string.IsNullOrWhiteSpace(s))))
                    {
                        if (s != null)
                        {
                            if (string.Compare(s, "true", true) == 0 || string.Compare(s, "false", true) == 0)
                            {
                                result = s[0] == 't' || s[0] == 'T';
                                break;
                            }
                            else
                            {
                                ulong a;
                                if (UInt64.TryParse(s, out a))
                                {
                                    result = (a != 0);
                                    break;
                                }

                                double d;
                                if (double.TryParse(s, out d))
                                {
                                    result = (d != 0);
                                    break;
                                }

                                decimal c;
                                if (decimal.TryParse(s, out c))
                                {
                                    result = (c != 0);
                                    break;
                                }

                                result = @default;
                                break;
                            }
                        }
                        if (x is Int64)
                        {
                            result = ((Int64)x) != 0;
                            break;
                        }
                        if (x is Int32)
                        {
                            result = ((Int32)x) != 0;
                            break;
                        }
                        if (x is Int16)
                        {
                            result = ((Int16)x) != 0;
                            break;
                        }
                        if (x is UInt64)
                        {
                            result = ((UInt64)x) != 0;
                            break;
                        }
                        if (x is UInt32)
                        {
                            result = ((UInt32)x) != 0;
                            break;
                        }
                        if (x is UInt16)
                        {
                            result = ((UInt16)x) != 0;
                            break;
                        }
                        if (x is Byte)
                        {
                            result = ((Byte)x) != 0;
                            break;
                        }
                        if (x is SByte)
                        {
                            result = ((SByte)x) != 0;
                            break;
                        }
                        if (x is Decimal)
                        {
                            result = ((Decimal)x) != 0;
                            break;
                        }
                        if (x is Double)
                        {
                            result = ((Double)x) != 0;
                            break;
                        }
                        if (x is Single)
                        {
                            result = ((Single)x) != 0;
                            break;
                        }
                        if (x is Char)
                        {
                            result = ((Char)x) == 'y' || ((Char)x) == 'Y';
                            break;
                        }
                        
                        result = System.Convert.ToBoolean(x);
                    }
                    else
                    {
                        result = @default;
                    }

                    break;
                }
            }
            catch
            {
                result = @default;
            }

            return result;
        }
        public static TimeSpan ToTimeSpan(object x, TimeSpan @default = default)
        {
            TimeSpan result = default;

            try
            {
                do
                {
                    if (x == null || DBNull.Value.Equals(x))
                    {
                        break;
                    }

                    var type = x.GetType();

                    if (type.IsArray)
                    {
                        var arr = (byte[])x;

                        if (arr.Length > 4)
                        {
                            result = new TimeSpan(arr[arr.Length - 5], arr[arr.Length - 4], arr[arr.Length - 3], arr[arr.Length - 2], arr[arr.Length - 1]);
                            break;
                        }

                        if (arr.Length > 3)
                        {
                            result = new TimeSpan(arr[arr.Length - 4], arr[arr.Length - 3], arr[arr.Length - 2], arr[arr.Length - 1]);
                            break;
                        }

                        if (arr.Length > 2)
                        {
                            result = new TimeSpan(arr[arr.Length - 3], arr[arr.Length - 2], arr[arr.Length - 1]);
                            break;
                        }

                        if (arr.Length > 1)
                        {
                            result = new TimeSpan(arr[arr.Length - 2], arr[arr.Length - 1], 0);
                            break;
                        }

                        if (arr.Length > 0)
                        {
                            result = new TimeSpan(arr[arr.Length - 1], 0, 0);
                            break;
                        }

                        result = @default;
                        break;
                    }

                    if (type == TypeHelper.TypeOfTimeSpan)
                    {
                        result = (TimeSpan)x;
                    }

                    if (type == TypeHelper.TypeOfDateTime || type == TypeHelper.TypeOfDateTimeOffset || type == TypeHelper.TypeOfNullableDateTime || type == TypeHelper.TypeOfNullableDateTimeOffset)
                    {
                        var date = ToDateTime(x);

                        result = new TimeSpan(0, date.Hour, date.Minute, date.Second, date.Millisecond);

                        break;
                    }

                    if (type == TypeHelper.TypeOfString)
                    {
                        var str = ToString(x)?.Trim();

                        if (!string.IsNullOrWhiteSpace(str))
                        {
                            var spaceIndex = str.IndexOf(' ');
                            var colonIndex1 = spaceIndex >= 0 ? str.IndexOf(':', spaceIndex + 1): str.IndexOf(':');
                            
                            if (colonIndex1 >= 0)
                            {
                                var first = spaceIndex >= 0 ? str.Substring(spaceIndex + 1, colonIndex1 - spaceIndex - 1) : str.Substring(0, colonIndex1);
                                var colonIndex2 = str.IndexOf(':', colonIndex1 + 1);

                                if (colonIndex2 < 0)
                                {
                                    var second = str.Substring(colonIndex1 + 1);

                                    result = new TimeSpan(ToByte(first), ToByte(second), 0);

                                    break;
                                }

                                if (colonIndex2 < str.Length)
                                {
                                    var second = str.Substring(colonIndex1 + 1, colonIndex2 - colonIndex1 - 1);

                                    if (colonIndex2 < str.Length - 1)
                                    {
                                        var dotIndex = str.IndexOf('.', colonIndex2 + 1);

                                        if (dotIndex < 0)
                                        {
                                            var third = str.Substring(colonIndex2 + 1);

                                            result = new TimeSpan(ToByte(first), ToByte(second), ToByte(third));
                                        }
                                        else
                                        {
                                            var third = str.Substring(colonIndex2 + 1, dotIndex - colonIndex2 - 1);
                                            var forth = str.Substring(dotIndex + 1);

                                            result = new TimeSpan(0, ToByte(first), ToByte(second), ToByte(third), ToInt(forth));
                                        }
                                    }
                                    else
                                    {
                                        result = new TimeSpan(ToByte(first), ToByte(second), 0);
                                    }
                                }
                                else
                                {
                                    var value = ToLong(first);

                                    if (value < 24)
                                    {
                                        result = new TimeSpan((byte)value, 0, 0);
                                    }
                                    else
                                    {
                                        result = TimeSpan.FromTicks(value);
                                    }
                                }
                            }
                            else
                            {
                                var value = ToLong(str);
                                
                                result = TimeSpan.FromTicks(value);
                            }
                        }

                        break;
                    }

                    var isNumeric = (type == TypeHelper.TypeOfInt64) || (type == TypeHelper.TypeOfInt32) || (type == TypeHelper.TypeOfInt16) || (type == TypeHelper.TypeOfByte) ||
                                    (type == TypeHelper.TypeOfUInt64) || (type == TypeHelper.TypeOfUInt32) || (type == TypeHelper.TypeOfUInt16) || (type == TypeHelper.TypeOfSByte) ||
                                    (type == TypeHelper.TypeOfSingle) || (type == TypeHelper.TypeOfDouble || type == TypeHelper.TypeOfDecimal);

                    if (isNumeric)
                    {
                        
                        result = TimeSpan.FromTicks(ToLong(x));
                        break;
                    }

                    result = @default;
                } while (false);
            }
            catch
            {
                result = @default;
            }

            return result;
        }
        public static Guid ToGuid(object x, Guid @default = default(Guid))
        {
            Guid result;

            try
            {
                var s = x?.ToString();

                if (!(DBNull.Value.Equals(x) || x == null || (s != null && string.IsNullOrWhiteSpace(s))))
                {
                    if (x.GetType() == TypeHelper.TypeOfGuid)
                    {
                        result = (Guid)x;
                    }
                    else
                    {
                        if (!System.Guid.TryParse(s, out result))
                        {
                            result = @default;
                        }
                    }
                }
                else
                {
                    result = @default;
                }
            }
            catch
            {
                result = @default;
            }

            return result;
        }
        public static UInt16 ToUShort(object x, UInt16 @default = default(UInt16))
        {
            return ToUInt16(x, @default);
        }
        public static UInt32 ToUInt(object x, UInt32 @default = default(UInt32))
        {
            return ToUInt32(x, @default);
        }
        public static UInt64 ToULong(object x, UInt64 @default = default(UInt64))
        {
            return ToUInt64(x, @default);
        }
        public static Int16 ToShort(object x, Int16 @default = default(Int16))
        {
            return ToInt16(x, @default);
        }
        public static Int32 ToInt(object x, Int32 @default = default(Int32))
        {
            return ToInt32(x, @default);
        }
        public static Int64 ToLong(object x, Int64 @default = default(Int64))
        {
            return ToInt64(x, @default);
        }
        public static Single ToFloat(object x, Single @default = default(Single))
        {
            return ToSingle(x, @default);
        }
        #endregion
        #region Nullable
        public static System.Numerics.BigInteger? ToNullableBigInteger(object x, bool useDefaultForNull = false, System.Numerics.BigInteger defaultValue = default(System.Numerics.BigInteger))
        {
            System.Numerics.BigInteger? result = null;

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

            if (result == null && useDefaultForNull)
            {
                result = defaultValue;
            }

            return result;
        }
        public static bool? ToBooleanNullable(object x, bool useDefaultForNull = false, bool defaultValue = false)
        {
            bool? result = null;

            try
            {
                for (;;)
                {
                    var s = x as string;

                    if (!(DBNull.Value.Equals(x) || x == null || (s != null && string.IsNullOrWhiteSpace(s))))
                    {
                        if (s != null)
                        {
                            if (string.Compare(s, "true", true) == 0 || string.Compare(s, "false", true) == 0)
                            {
                                result = s[0] == 't' || s[0] == 'T';
                                break;
                            }
                            else
                            {
                                ulong a;
                                if (UInt64.TryParse(s, out a))
                                {
                                    result = (a != 0);
                                    break;
                                }

                                double d;
                                if (double.TryParse(s, out d))
                                {
                                    result = (d != 0);
                                    break;
                                }

                                decimal c;
                                if (decimal.TryParse(s, out c))
                                {
                                    result = (c != 0);
                                    break;
                                }

                                break;
                            }
                        }
                        if (x is Int64)
                        {
                            result = ((Int64)x) != 0;
                            break;
                        }
                        if (x is Int32)
                        {
                            result = ((Int32)x) != 0;
                            break;
                        }
                        if (x is Int16)
                        {
                            result = ((Int16)x) != 0;
                            break;
                        }
                        if (x is UInt64)
                        {
                            result = ((UInt64)x) != 0;
                            break;
                        }
                        if (x is UInt32)
                        {
                            result = ((UInt32)x) != 0;
                            break;
                        }
                        if (x is UInt16)
                        {
                            result = ((UInt16)x) != 0;
                            break;
                        }
                        if (x is Byte)
                        {
                            result = ((Byte)x) != 0;
                            break;
                        }
                        if (x is SByte)
                        {
                            result = ((SByte)x) != 0;
                            break;
                        }
                        if (x is Decimal)
                        {
                            result = ((Decimal)x) != 0;
                            break;
                        }
                        if (x is Double)
                        {
                            result = ((Double)x) != 0;
                            break;
                        }
                        if (x is Single)
                        {
                            result = ((Single)x) != 0;
                            break;
                        }
                        if (x is Char)
                        {
                            result = ((Char)x) == 'y' || ((Char)x) == 'Y';
                            break;
                        }

                        result = System.Convert.ToBoolean(x);
                    }

                    break;
                }
            }
            catch
            { }

            if (result == null && useDefaultForNull)
            {
                result = defaultValue;
            }

            return result;
        }
        public static Guid? ToGuidNullable(object x, bool useDefaultForNull = false, Guid defaultValue = default(Guid))
        {
            Guid? result = null;
            try
            {
                var s = x?.ToString();

                if (!(DBNull.Value.Equals(x) || x == null || (s != null && string.IsNullOrWhiteSpace(s))))
                {
                    if (x.GetType() == TypeHelper.TypeOfGuid)
                    {
                        result = (Guid)x;
                    }
                    else
                    {
                        Guid g;

                        if (!System.Guid.TryParse(s, out g))
                        {
                            result = g;
                        }
                    }
                }
            }
            catch
            { }
            
            if (result == null && useDefaultForNull)
            {
                result = defaultValue;
            }

            return result;
        }
        public static TimeSpan? ToTimeSpanNullable(object x, bool useDefaultForNull = false, TimeSpan defaultValue = default(TimeSpan))
        {
            TimeSpan? result = null;

            if (x != null && !DBNull.Value.Equals(x))
            {
                result = ToTimeSpan(x, defaultValue);
            }

            if (result == null && useDefaultForNull)
            {
                result = defaultValue;
            }

            return result;
        }
        public static UInt16? ToUShortNullable(object x, bool useDefaultForNull = false, UInt16 defaultValue = default(UInt16))
        {
            return ToUInt16Nullable(x, useDefaultForNull, defaultValue);
        }
        public static UInt32? ToUIntNullable(object x, bool useDefaultForNull = false, UInt32 defaultValue = default(UInt32))
        {
            return ToUInt32Nullable(x, useDefaultForNull, defaultValue);
        }
        public static UInt64? ToULongNullable(object x, bool useDefaultForNull = false, UInt64 defaultValue = default(UInt64))
        {
            return ToUInt64Nullable(x, useDefaultForNull, defaultValue);
        }
        public static Int16? ToShortNullable(object x, bool useDefaultForNull = false, Int16 defaultValue = default(Int16))
        {
            return ToInt16Nullable(x, useDefaultForNull, defaultValue);
        }
        public static Int32? ToIntNullable(object x, bool useDefaultForNull = false, Int32 defaultValue = default(Int32))
        {
            return ToInt32Nullable(x, useDefaultForNull, defaultValue);
        }
        public static Int64? ToLongNullable(object x, bool useDefaultForNull = false, Int64 defaultValue = default(Int64))
        {
            return ToInt64Nullable(x, useDefaultForNull, defaultValue);
        }
        public static Single? ToFloatNullable(object x, bool useDefaultForNull = false, Single defaultValue = default(Single))
        {
            return ToSingleNullable(x, useDefaultForNull, defaultValue);
        }
        #endregion
        public static double Rad2Deg(double radians)
        {
            return (180 / Math.PI) * radians;
        }
        public static double Deg2Rad(double degrees)
        {
            return (Math.PI / 180) * degrees;
        }
        //https://stackoverflow.com/questions/18015425/invalid-cast-from-system-int32-to-system-nullable1system-int32-mscorlib/18015612
        public static object ChangeType(object value, Type type)
        {
            var t = type;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(TypeHelper.TypeOfNullable))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return System.Convert.ChangeType(value, t);
        }
        public static T ChangeType<T>(object value)
        {
            var t = typeof(T);

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return default(T);
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return (T)System.Convert.ChangeType(value, t);
        }
    }
}