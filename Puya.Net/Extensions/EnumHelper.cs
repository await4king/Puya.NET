using System;
using System.Collections.Generic;
using System.Text;
using Puya.Conversion;

namespace Puya.Extensions
{
    public static class EnumHelper
    {
        public static object ToEnum(this object value, Type type, bool ignoreCase = true)
        {
            object result = null;

            if (value != null)
            {
                if (value is string)
                {
                    try
                    {
                        result = Enum.Parse(type, (string)value, ignoreCase);

                        return result;
                    }
                    catch
                    { }
                }

                result = SafeClrConvert.ToULong(value).ToEnum(type);
            }

            return result;
        }
        public static T ToEnum<T>(this object value, bool ignoreCase = true)
        {
            return (T)ToEnum(value, typeof(T), ignoreCase);
        }
        public static object ToEnum(this object value, Type type, object defaultValue, bool ignoreCase = true)
        {
            object result = null;

            if (value != null)
            {
                if (value is string)
                {
                    try
                    {
                        result = Enum.Parse(type, (string)value, ignoreCase);

                        return result;
                    }
                    catch
                    {
                        result = defaultValue;
                    }
                }

                result = SafeClrConvert.ToULong(value).ToEnum(type, defaultValue);
            }
            else
            {
                result = defaultValue;
            }

            return result;
        }
        public static T ToEnum<T>(this object value, object defaultValue, bool ignoreCase = true)
        {
            return (T)ToEnum(value, typeof(T), defaultValue, ignoreCase);
        }
    }
}
