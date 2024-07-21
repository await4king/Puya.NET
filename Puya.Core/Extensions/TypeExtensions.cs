using Puya.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace Puya.Extensions
{
    public static class TypeExtensions
    {
        public static bool CanAccept(this Type target, object value)
        {
            var type = value?.GetType();

            if (value == null && target.CanAcceptNull())
                return true;

            if (type == target)
                return true;

            if (type.DescendsFrom(target))
                return true;

            return false;
        }
        public static bool CanAcceptNull(this Type type)
        {
            return !type.IsValueType || type.IsNullable();
        }
        public static bool IsBasicType(this Type type)
        {
            if (type == null)
                return false;

            return     type.IsPrimitive
                    || type.IsEnum
                    || type == TypeHelper.TypeOfDecimal
                    || type == TypeHelper.TypeOfDateTime
                    || type == TypeHelper.TypeOfDateTimeOffset
                    || type == TypeHelper.TypeOfTimeSpan
                    || type == TypeHelper.TypeOfString;
        }
        public static bool IsSimpleType(this Type type)
        {
            if (type == null)
                return false;

            return type.IsValueType || type.IsBasicType();
        }
        public static bool IsDate(this Type type)
        {
            var result = type == TypeHelper.TypeOfDateTime || type == TypeHelper.TypeOfDateTimeOffset;

            return result;
        }
        public static bool IsNumeric(this Type type)
        {
            var result = IsInteger(type) || IsFloat(type);

            return result;
        }
        public static bool IsFloat(this Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            var result = typeCode == TypeCode.Single || typeCode == TypeCode.Double || typeCode == TypeCode.Decimal;

            return result;
        }
        public static bool IsInteger(this Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            var result = (typeCode == TypeCode.Int64) || (typeCode == TypeCode.Int32) || (typeCode == TypeCode.Int16) ||
                         (typeCode == TypeCode.UInt64) || (typeCode == TypeCode.UInt32) || (typeCode == TypeCode.UInt16) ||
                         (typeCode == TypeCode.Byte) || (typeCode == TypeCode.SByte) || (type == TypeHelper.TypeOfBigInteger);

            return result;
        }
        public static bool IsNullableInteger(this Type type)
        {
            var result = (type == TypeHelper.TypeOfNullableInt64) || (type == TypeHelper.TypeOfNullableInt32) || (type == TypeHelper.TypeOfNullableInt16) || (type == TypeHelper.TypeOfNullableByte) ||
                         (type == TypeHelper.TypeOfNullableUInt64) || (type == TypeHelper.TypeOfNullableUInt32) || (type == TypeHelper.TypeOfNullableUInt16) || (type == TypeHelper.TypeOfNullableSByte) ||
                         (type == TypeHelper.TypeOfNullableBigInteger);

            return result;
        }
        public static bool IsNullableFloat(this Type type)
        {
            var result = (type == TypeHelper.TypeOfNullableSingle) || (type == TypeHelper.TypeOfNullableDouble || type == TypeHelper.TypeOfNullableDecimal);

            return result;
        }
        // https://docs.microsoft.com/en-us/dotnet/api/system.valuetype?view=netcore-3.1
        public static int Compare(ValueType value1, ValueType value2)
        {
            var type1 = value1.GetType();
            var type2 = value2.GetType();

            if (!IsNumeric(type1))
                throw new ArgumentException("value1 is not a number.");
            else if (!IsNumeric(type2))
                throw new ArgumentException("value2 is not a number.");

            // Use BigInteger as common integral type
            if (IsInteger(type1) && IsInteger(type2))
            {
                System.Numerics.BigInteger bigint1 = (System.Numerics.BigInteger)value1;
                System.Numerics.BigInteger bigint2 = (System.Numerics.BigInteger)value2;

                return System.Numerics.BigInteger.Compare(bigint1, bigint2);
            }
            // At least one value is floating point; use Double.
            else
            {
                Double dbl1 = 0;
                Double dbl2 = 0;

                try
                {
                    dbl1 = Convert.ToDouble(value1);
                }
                catch (OverflowException)
                {
                    Debug.WriteLine("value1 is outside the range of a Double.");
                }
                try
                {
                    dbl2 = Convert.ToDouble(value2);
                }
                catch (OverflowException)
                {
                    Debug.WriteLine("value2 is outside the range of a Double.");
                }

                return dbl1.CompareTo(dbl2);
            }
        }
        public static bool IsNullableNumeric(this Type type)
        {
            var result = IsNullableInteger(type) || IsNullableFloat(type);

            return result;
        }
        public static bool IsNullableOrNumeric(this Type type)
        {
            var result = IsNumeric(type) || IsNullableNumeric(type);

            return result;
        }
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types#how-to-identify-a-nullable-value-type
        public static bool IsNullable(this Type type)
        {
            // previous implementation
            // return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == TypeHelper.TypeOfNullable;
            return Nullable.GetUnderlyingType(type) != null;
        }
        public static bool IsNullable<T>(T someValue)
        {
            return IsNullable(typeof(T));
        }
        public static bool IsNullableOf<T>(T someValue, Type ofType)
        {
            return Nullable.GetUnderlyingType(typeof(T)) == ofType;
        }
        public static bool IsNullableOrBasicType(this Type type)
        {
            return type.IsNullable() || type.IsBasicType();
        }
        public static bool IsNullableOrBasicType<T>(T someValue)
        {
            var type = typeof(T);

            return type.IsNullable() || type.IsBasicType();
        }
        public static bool IsNullableOrSimpleType(this Type type)
        {
            return type.IsNullable() || type.IsSimpleType();
        }
        public static bool IsNullableOrSimpleType<T>(T someValue)
        {
            var type = typeof(T);

            return type.IsNullable() || type.IsSimpleType();
        }
        public static bool Implements(this Type type, Type interfaceType)
        {
            if (!interfaceType.IsInterface)
                return false;

            if (interfaceType.IsGenericType)
            {
                return type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == interfaceType || i == interfaceType));
            }
            else
            {
                return type.GetInterfaces().Contains(interfaceType);
            }
        }
        public static bool TryGetItemType(this Type type, out Type innerType)
        {
            //source: https://stackoverflow.com/questions/1043755/c-sharp-generic-list-t-how-to-get-the-type-of-t/13608408#13608408
            System.Diagnostics.Contracts.Contract.Requires(type != null);

            var interfaceTest = new Func<Type, Type>(i => i.IsGenericType && i.GetGenericTypeDefinition() == TypeHelper.TypeOfIEnumerableOfT ? i.GetGenericArguments().Single() : null);

            innerType = interfaceTest(type);

            if (innerType != null)
            {
                return true;
            }

            foreach (var i in type.GetInterfaces())
            {
                innerType = interfaceTest(i);

                if (innerType != null)
                {
                    return true;
                }
            }

            return false;
        }
        public static bool TryGetDictionaryItemType(this Type type, out Type keyType, out Type valueType)
        {
            //inspired from: https://stackoverflow.com/questions/1043755/c-sharp-generic-list-t-how-to-get-the-type-of-t/13608408#13608408
            System.Diagnostics.Contracts.Contract.Requires(type != null);

            Tuple<Type, Type> GetInnerType(Type interfaceType)
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                {
                    var args = interfaceType.GetGenericArguments();

                    return Tuple.Create(args[0], args[1]);
                }
                else
                {
                    return null;
                }
            }

            Tuple<Type, Type> tuple = GetInnerType(type);

            if (tuple != null)
            {
                keyType = tuple.Item1;
                valueType = tuple.Item2;

                return true;
            }

            foreach (var i in type.GetInterfaces())
            {
                tuple = GetInnerType(i);

                if (tuple != null)
                {
                    keyType = tuple.Item1;
                    valueType = tuple.Item2;

                    return true;
                }
            }

            keyType = null;
            valueType = null;

            return false;
        }
        public static bool IsConstructable(this Type type)
        {
            var result = !(type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition);

            if (result && !type.IsValueType)
            {
                result = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Length > 0;
            }

            return result;
        }
        public static bool DescendsFrom(this Type type, Type targetType)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");

            var result = type.IsSubclassOf(targetType);

            if (!result)
            {
                if (targetType.IsGenericTypeDefinition)
                {
                    var _type = type;

                    while (_type != TypeHelper.TypeOfObject && _type != null)
                    {
                        if (_type.IsGenericType && _type.GetGenericTypeDefinition() == targetType)
                        {
                            result = true;
                            break;
                        }

                        _type = _type.BaseType;
                    }
                }
            }

            return result;
        }
        public static bool DescendsFrom<T>(this Type type)
        {
            return type.DescendsFrom(typeof(T));
        }
        public static Dictionary<string, object> ToDictionary(this object x, string excludes = "")
        {
            var result = new Dictionary<string, object>();
            var arrExcludes = new string[] { };

            if (!string.IsNullOrEmpty(excludes))
            {
                excludes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (var prop in x.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.CanRead && (arrExcludes.Length == 0 || !Array.Exists(arrExcludes, ex => string.Compare(ex, prop.Name, true) == 0)))
                {
                    result.Add(prop.Name, prop.GetValue(x, null));
                }
            }

            return result;
        }
        public static bool Implements<TInterface>(this Type type)
        {
            return type.Implements(typeof(TInterface));
        }
        public static bool IsEnumerable(this Type type)
        {
            return type.Implements(TypeHelper.TypeOfIEnumerable);
        }
        public static bool IsDictionary(this Type type)
        {
            return type.Implements(typeof(IDictionary<,>));
        }
        public static Type[] GetGenericTypeArguments(this Type type, int parentOrder = -1)
        {
            var genericCount = 0;
            var _type = type;
            var result = null as Type[];

            while (true)
            {
                if (_type.IsGenericType)
                {
                    if (parentOrder == genericCount || parentOrder == -1)
                    {
                        result = _type.GenericTypeArguments;
                        break;
                    }

                    genericCount++;
                }

                _type = _type.BaseType;

                if (_type == typeof(Object))
                {
                    break;
                }
            }

            return result;
        }
        public static object GetDefault(this Type type)
        {
            var result = null as object;

            if (type != null && type.IsValueType)
            {
                result = Activator.CreateInstance(type);
            }

            return result;
        }
    }
}
