using Puya.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Puya.Conversion;
using ClrConvertor = Puya.Conversion.SafeClrConvert;
using System.Collections.Concurrent;
using Puya.Reflection;
using Puya.Collections;
using System.Collections;

namespace Puya.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var result = new Dictionary<string, object>();

            if (obj != null)
            {
                var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead);

                foreach (var prop in props)
                {
                    if (prop.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0)
                    {
                        result.Add(prop.Name, prop.GetValue(obj));
                    }
                }
            }

            return result;
        }
        public static T ConvertTo<T>(this object source)
        {
            return (T)ConvertTo(source, typeof(T));
        }
        public static object ConvertTo(this object source, Type targetType)
        {
            var result = null as object;

            if (source != null && !DBNull.Value.Equals(source) && targetType != null)
            {
                var sourceType = source.GetType();

                if (targetType == sourceType || sourceType.DescendsFrom(targetType))
                {
                    result = source;
                }
                else if (targetType == TypeHelper.TypeOfByteArray)
                {
                    if (sourceType == TypeHelper.TypeOfString)
                    {
                        result = Encoding.UTF8.GetBytes(source.ToString());
                    }
                    else
                    {
                        result = (byte[])source;
                    }
                }
                else if (targetType == TypeHelper.TypeOfCharArray)
                {
                    if (sourceType == TypeHelper.TypeOfString)
                    {
                        result = source.ToString().ToArray();
                    }
                    else
                    {
                        result = (char[])source;
                    }
                }
                else if (targetType == TypeHelper.TypeOfGuid)
                {
                    result = new Guid(ClrConvertor.ToString(source));
                }
                else if (sourceType.IsNullableOrBasicType())
                {
                    if (targetType == TypeHelper.TypeOfBool || targetType == TypeHelper.TypeOfNullableBool)
                    {
                        result = ClrConvertor.ToBoolean(source);
                    }
                    else if (targetType == TypeHelper.TypeOfChar || targetType == TypeHelper.TypeOfNullableChar)
                    {
                        result = ClrConvertor.ToChar(source);
                    }
                    else if (targetType == TypeHelper.TypeOfDateTime || targetType == TypeHelper.TypeOfNullableDateTime)
                    {
                        result = ClrConvertor.ToDateTime(source);
                    }
                    else if (targetType == TypeHelper.TypeOfDateTimeOffset || targetType == TypeHelper.TypeOfNullableDateTimeOffset)
                    {
                        result = ClrConvertor.ToDateTime(source);
                    }
                    else if (targetType == TypeHelper.TypeOfDecimal || targetType == TypeHelper.TypeOfNullableDecimal)
                    {
                        result = ClrConvertor.ToDecimal(source);
                    }
                    else if (targetType == TypeHelper.TypeOfDouble || targetType == TypeHelper.TypeOfNullableDouble)
                    {
                        result = ClrConvertor.ToDouble(source);
                    }
                    else if (targetType == TypeHelper.TypeOfFloat || targetType == TypeHelper.TypeOfNullableFloat)
                    {
                        result = ClrConvertor.ToSingle(source);
                    }
                    else if (targetType == TypeHelper.TypeOfInt16 || targetType == TypeHelper.TypeOfNullableInt16)
                    {
                        result = ClrConvertor.ToInt16(source);
                    }
                    else if (targetType == TypeHelper.TypeOfInt32 || targetType == TypeHelper.TypeOfNullableInt32)
                    {
                        result = ClrConvertor.ToInt32(source);
                    }
                    else if (targetType == TypeHelper.TypeOfInt64 || targetType == TypeHelper.TypeOfNullableInt64)
                    {
                        result = ClrConvertor.ToInt64(source);
                    }
                    else if (targetType == TypeHelper.TypeOfByte || targetType == TypeHelper.TypeOfNullableByte)
                    {
                        result = ClrConvertor.ToByte(source);
                    }
                    else if (targetType == TypeHelper.TypeOfSByte || targetType == TypeHelper.TypeOfNullableSByte)
                    {
                        result = ClrConvertor.ToSByte(source);
                    }
                    else if (targetType == TypeHelper.TypeOfString)
                    {
                        result = ClrConvertor.ToString(source);
                    }
                    else if (targetType == TypeHelper.TypeOfTimeSpan || targetType == TypeHelper.TypeOfNullableTimeSpan)
                    {
                        result = ClrConvertor.ToTimeSpan(source);
                    }
                    else if (targetType == TypeHelper.TypeOfUInt16 || targetType == TypeHelper.TypeOfNullableUInt16)
                    {
                        result = ClrConvertor.ToUInt16(source);
                    }
                    else if (targetType == TypeHelper.TypeOfUInt32 || targetType == TypeHelper.TypeOfNullableUInt32)
                    {
                        result = ClrConvertor.ToUInt32(source);
                    }
                    else if (targetType == TypeHelper.TypeOfUInt64 || targetType == TypeHelper.TypeOfNullableUInt64)
                    {
                        result = ClrConvertor.ToUInt64(source);
                    }
                    else if (targetType.IsEnum)
                    {
                        result = source.ToEnum(targetType);
                    }
                }
                else if (!targetType.IsInterface && !targetType.IsAbstract && !targetType.IsNullableOrBasicType())
                {
                    result = ObjectActivator.Instance.Activate(targetType);

                    if (result != null)
                    {
                        var sourceProps = ReflectionHelper.GetPublicInstanceReadableProperties(sourceType);
                        var targetProps = ReflectionHelper.GetPublicInstanceWritableProperties(targetType);

                        if (sourceProps.Count() > 0 && targetProps.Count() > 0)
                        {
                            foreach (var sourceProp in sourceProps)
                            {
                                var targetProp = targetProps.FirstOrDefault(p => string.Compare(p.Name, sourceProp.Name, StringComparison.Ordinal) == 0);

                                if (targetProp != null)
                                {
                                    var sourceValue = sourceProp.GetValue(source);
                                    var targetValue = ConvertTo(sourceValue, targetProp.PropertyType);

                                    if (targetValue != null || !targetProp.PropertyType.IsSimpleType() || targetProp.PropertyType == TypeHelper.TypeOfString)
                                    {
                                        targetProp.SetValue(result, targetValue);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // nothing can be done
                }
            }

            return result;
        }
        public static object ToDynamicModel(this object obj, bool ignoreNulls = false)
        {
            if (obj == null)
            {
                return null;
            }

            var type = obj.GetType();

            if (type.IsNullableOrBasicType())
            {
                return obj;
            }

            var result = new DynamicModel();

            foreach (var prop in ReflectionHelper.GetPublicInstanceReadableProperties(type).Where(prop => prop.GetIndexParameters().Length == 0))
            {
                var value = prop.GetValue(obj);

                if (value == null && ignoreNulls)
                {
                    continue;
                }

                if (prop.PropertyType.IsNullableOrBasicType())
                {
                    result.Add(prop.Name, value);
                }
                else
                {
                    if (prop.PropertyType.Implements(typeof(IDictionary<,>)))
                    {
                        var dic = new DynamicModel();
                        var e = value as IEnumerable;
                        var en = e.GetEnumerator();
                        Type itemType = null;
                        PropertyInfo keyProp = null;
                        PropertyInfo valueProp = null;

                        while (en.MoveNext())
                        {
                            if (itemType == null)
                            {
                                itemType = en.Current.GetType();
                                keyProp = itemType.GetProperty("Key");
                                valueProp = itemType.GetProperty("Value");
                            }

                            var itemValue = valueProp.GetValue(en.Current);

                            if (itemValue != null || !ignoreNulls)
                            {
                                var key = keyProp.GetValue(en.Current)?.ToString();

                                if (key != null && !dic.ContainsKey(key))
                                {
                                    dic.Add(key, itemValue);
                                }
                            }
                        }

                        result.Add(prop.Name, dic);
                    }
                    else
                    {
                        if (prop.PropertyType.Implements<IEnumerable>())
                        {
                            var e = value as IEnumerable;

                            if (e != null)
                            {
                                var en = e.GetEnumerator();
                                var list = new List<object>();

                                while (en.MoveNext())
                                {
                                    var itemType = en.Current.GetType();

                                    if (itemType.DescendsFrom(typeof(KeyValuePair<,>)))
                                    {

                                    }
                                    else
                                    {
                                        list.Add(en.Current.ToDynamicModel(ignoreNulls));
                                    }
                                }

                                result.Add(prop.Name, list);
                            }
                        }
                        else
                        {
                            result.Add(prop.Name, value.ToDynamicModel(ignoreNulls));
                        }
                    }
                }
            }

            return result;
        }
    }
}
