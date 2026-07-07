using Puya.Base;
using Puya.Collections;
using Puya.Extensions;
using Puya.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using ClrConvertor = Puya.Conversion.SafeClrConvert;

namespace Puya.Extensions
{
    public static class ObjectExtensions
    {
        static bool IsExcluded(string key, string[] arrExcludes, bool ignoreCase)
        {
            return arrExcludes.Length != 0 && Array.Exists(arrExcludes, ex => string.Compare(ex, key, ignoreCase) == 0);
        }
        static void Merge(IDictionary<string, object> result, object source, int index)
        {
            if (source == null) return;

            var sourceType = source.GetType();

            if (sourceType.IsDictionary())
            {
                source.IterateDictionary(kv =>
                {
                    result[kv.Key?.ToString()] = kv.Value;
                });
            }
            else if (sourceType.IsEnumerable())
            {
                var e = source as IEnumerable;
                var arr = new ArrayList();

                e.ForEach(item => arr.Add(item));

                result[index.ToString()] = arr;
            }
            else
            {
                var properties = ReflectionHelper.GetPublicInstanceReadableProperties(sourceType);

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(source);

                    result[prop.Name] = value;
                }
            }
        }
        public static object Merge(this object obj, params object[] others)
        {
            var result = new DynamicModel() as IDictionary<string, object>;

            Merge(result, obj, 0);

            var index = 1;

            foreach (var other in others)
            {
                Merge(result, other, index++);
            }

            return result;
        }
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            return ToDictionary(obj, false, null);
        }
        public static IDictionary<string, object> ToDictionary(this object obj, bool nested)
        {
            return ToDictionary(obj, nested, null);
        }
        public static IDictionary<string, object> ToDictionary(this object obj, string excludes, bool ignoreCase = false)
        {
            return ToDictionary(obj, false, excludes, ignoreCase);
        }
        public static IDictionary<string, object> ToDictionary(this object obj, bool nested, string excludes, bool ignoreCase = false)
        {
            var result = null as IDictionary<string, object>;

            if (obj != null)
            {
                var arrExcludes = new string[] { };

                if (!string.IsNullOrEmpty(excludes))
                {
                    arrExcludes = excludes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }

                var type = obj.GetType();

                if (type.Implements<IDictionary<string, object>>())
                {
                    result = (obj as IDictionary<string, object>).Where(item => !IsExcluded(item.Key, arrExcludes, ignoreCase)).ToDictionary();
                }
                else if (type.IsDictionary())
                {
                    result = new DynamicModel();

                    var enumerable = obj as IEnumerable;

                    foreach (var entry in enumerable)
                    {
                        if (entry != null)
                        {
                            var entryType = entry.GetType();
                            var keyProp = entryType.GetProperty("Key");
                            var valueProp = entryType.GetProperty("Value");

                            var key = keyProp?.GetValue(entry);
                            var value = valueProp?.GetValue(entry);

                            if (value != null && !value.GetType().IsSimpleType() && nested)
                            {
                                result.Add(key?.ToString(), value.ToDictionary());
                            }
                            else
                            {
                                result.Add(key?.ToString(), value);
                            }
                        }
                    }
                }
                else
                {
                    result = new DynamicModel();

                    ReflectionHelper.ForEachPublicInstanceReadableNotIgnorableProperty(type, prop =>
                    {
                        if (!IsExcluded(prop.Name, arrExcludes, ignoreCase))
                        {
                            var value = prop.GetValue(obj);

                            if (value != null && !value.GetType().IsSimpleType() && nested)
                            {
                                result.Add(prop.Name, value.ToDictionary());
                            }
                            else
                            {
                                result.Add(prop.Name, value);
                            }
                        }
                    });
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
        public static object Query(this Object obj, string path, bool ignoreCase = false)
        {
            TryQuery(obj, path, ignoreCase, out object result);

            return result;
        }
        public static bool TryQuery(this Object obj, string path, bool ignoreCase, out object result)
        {
            var _result = false;

            result = null as object;

            if (!string.IsNullOrWhiteSpace(path))
            {
                var cur = obj;
                var propNames = path.Split('.');
                var index = 0;

                foreach (var propName in propNames)
                {
                    if (cur == null)
                    {
                        break;
                    }

                    var type = cur.GetType();
                    Object[] args = null;
                    PropertyInfo prop;

                    if (type.IsDictionary())
                    {
                        prop = type.GetProperty("Item");
                        args = new object[] { propName };
                    }
                    else
                    {
                        var flags = BindingFlags.Instance | BindingFlags.Public;

                        if (ignoreCase)
                        {
                            flags |= BindingFlags.IgnoreCase;
                        }

                        prop = type.GetProperty(propName, flags);
                    }

                    if (prop == null || !prop.CanRead)
                    {
                        break;
                    }

                    cur = args == null ? prop.GetValue(cur) : prop.GetValue(cur, args);

                    index++;
                }

                if (index == propNames.Length)
                {
                    result = cur;
                    _result = true;
                }
            }

            return _result;
        }
        public static bool IsValidJsonType(this DataType type, object obj)
        {
            if (obj != null)
            {
                var objType = obj.GetType();
                var isString = (type & DataType.String) == DataType.String && objType == TypeHelper.TypeOfString;
                var isNumber = (type & DataType.Number) == DataType.Number && objType.IsNumeric();
                var isBoolean = (type & DataType.Boolean) == DataType.Boolean && objType == TypeHelper.TypeOfBool;
                var isObject = (type & DataType.Object) == DataType.Object && !objType.IsBasicType() && !objType.IsEnumerable();
                var isArray = (type & DataType.Array) == DataType.Array && objType.IsEnumerable();

                return isString || isNumber || isBoolean || isObject || isArray;
            }
            else
            {
                return true;
            }
        }
    }
}
