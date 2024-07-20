

using System;
using Puya.Base;

namespace Puya.Conversion
{
    // 
	public static partial class SafeClrConvert
	{
        static bool IsValid(object x)
        {
            var s = x as string;

            return !(x == null || DBNull.Value.Equals(x) || (s != null && string.IsNullOrWhiteSpace(s)));
        }
		public static System.Int64 ToInt64(object x, System.Int64 @default = default(System.Int64))
        {
            System.Int64 result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToInt64(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Int64? ToInt64Nullable(object x, bool useDefaultForNull = false, System.Int64 @default = default(System.Int64))
        {
            System.Int64? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToInt64(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Int32 ToInt32(object x, System.Int32 @default = default(System.Int32))
        {
            System.Int32 result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToInt32(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Int32? ToInt32Nullable(object x, bool useDefaultForNull = false, System.Int32 @default = default(System.Int32))
        {
            System.Int32? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToInt32(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Int16 ToInt16(object x, System.Int16 @default = default(System.Int16))
        {
            System.Int16 result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToInt16(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Int16? ToInt16Nullable(object x, bool useDefaultForNull = false, System.Int16 @default = default(System.Int16))
        {
            System.Int16? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToInt16(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.UInt64 ToUInt64(object x, System.UInt64 @default = default(System.UInt64))
        {
            System.UInt64 result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToUInt64(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.UInt64? ToUInt64Nullable(object x, bool useDefaultForNull = false, System.UInt64 @default = default(System.UInt64))
        {
            System.UInt64? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToUInt64(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.UInt32 ToUInt32(object x, System.UInt32 @default = default(System.UInt32))
        {
            System.UInt32 result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToUInt32(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.UInt32? ToUInt32Nullable(object x, bool useDefaultForNull = false, System.UInt32 @default = default(System.UInt32))
        {
            System.UInt32? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToUInt32(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.UInt16 ToUInt16(object x, System.UInt16 @default = default(System.UInt16))
        {
            System.UInt16 result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToUInt16(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.UInt16? ToUInt16Nullable(object x, bool useDefaultForNull = false, System.UInt16 @default = default(System.UInt16))
        {
            System.UInt16? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToUInt16(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Byte ToByte(object x, System.Byte @default = default(System.Byte))
        {
            System.Byte result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToByte(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Byte? ToByteNullable(object x, bool useDefaultForNull = false, System.Byte @default = default(System.Byte))
        {
            System.Byte? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToByte(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.SByte ToSByte(object x, System.SByte @default = default(System.SByte))
        {
            System.SByte result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToSByte(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.SByte? ToSByteNullable(object x, bool useDefaultForNull = false, System.SByte @default = default(System.SByte))
        {
            System.SByte? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToSByte(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Char ToChar(object x, System.Char @default = default(System.Char))
        {
            System.Char result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToChar(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Char? ToCharNullable(object x, bool useDefaultForNull = false, System.Char @default = default(System.Char))
        {
            System.Char? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToChar(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Single ToSingle(object x, System.Single @default = default(System.Single))
        {
            System.Single result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToSingle(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Single? ToSingleNullable(object x, bool useDefaultForNull = false, System.Single @default = default(System.Single))
        {
            System.Single? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToSingle(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Double ToDouble(object x, System.Double @default = default(System.Double))
        {
            System.Double result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToDouble(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Double? ToDoubleNullable(object x, bool useDefaultForNull = false, System.Double @default = default(System.Double))
        {
            System.Double? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToDouble(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.Decimal ToDecimal(object x, System.Decimal @default = default(System.Decimal))
        {
            System.Decimal result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToDecimal(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.Decimal? ToDecimalNullable(object x, bool useDefaultForNull = false, System.Decimal @default = default(System.Decimal))
        {
            System.Decimal? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToDecimal(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     		public static System.DateTime ToDateTime(object x, System.DateTime @default = default(System.DateTime))
        {
            System.DateTime result = @default;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToDateTime(x);
                }
                catch
                { }
            }

            return result;
        } 
		public static System.DateTime? ToDateTimeNullable(object x, bool useDefaultForNull = false, System.DateTime @default = default(System.DateTime))
        {
            System.DateTime? result = null;

            if (IsValid(x))
            {
                try
                {
                    result = System.Convert.ToDateTime(x);
                }
                catch
                { }
            }

			if (result == null && useDefaultForNull)
            {
                result = @default;
            }

            return result;
        }
     	}
}