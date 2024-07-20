using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Puya.Base;
using Puya.Conversion;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Mapping
{
    public class DefaultMapper : IMapper
    {
        static ConcurrentDictionary<Type, ConcurrentDictionary<BindingFlags, PropertyInfo[]>> propertyCache;
        static DefaultMapper()
        {
            propertyCache = new ConcurrentDictionary<Type, ConcurrentDictionary<BindingFlags, PropertyInfo[]>>();
        }
        public void Copy(object source, object target)
        {
            throw new NotImplementedException();
        }

        public object Map(IDataReader reader, Type type)
        {
            var result = type == TypeHelper.TypeOfString ? Activator.CreateInstance(TypeHelper.TypeOfString, "".ToCharArray()): Activator.CreateInstance(type);

            Map(reader, ref result);

            return result;
        }
        public object Map(Type type, object source)
        {
            var result = null as object;
            var targetProps = ReflectionHelper.GetProperties(type, System.Reflection.BindingFlags.Instance);

            if (source != null)
            {
                result = Activator.CreateInstance(type);

                ReflectionHelper.ForEachProperty(source.GetType(), prop =>
                {
                    if (prop.CanRead)
                    {
                        var targetProp = targetProps.FirstOrDefault(p => string.Compare(p.Name, prop.Name, StringComparison.Ordinal) == 0);

                        if (targetProp != null && targetProp.CanWrite)
                        {
                            object value;

                            if (targetProp.PropertyType.IsNullableOrBasicType())
                            {
                                value = prop.GetValue(source);

                                targetProp.SetValue(result, value);
                            }
                            else
                            {
                                if (!targetProp.PropertyType.IsEnumerable() && !targetProp.PropertyType.IsInterface)
                                {
                                    value = Map(targetProp.PropertyType, prop.GetValue(source));

                                    targetProp.SetValue(result, value);
                                }
                                else
                                {
                                    if (targetProp.PropertyType.IsEnumerable() && !targetProp.PropertyType.IsInterface && targetProp.PropertyType == prop.PropertyType)
                                    {
                                        value = prop.GetValue(source);

                                        targetProp.SetValue(result, value);
                                    }
                                }
                            }
                        }
                    }
                }, BindingFlags.Instance);
            }

            return result;
        }

        public void Map(IDataReader reader, ref object target)
        {
            if (reader.IsClosed)
            {
                return;
            }

            if (target == null)
            {
                target = reader[0];

                return;
            }

            var type = target.GetType();

            if (type.IsNullableOrBasicType())
            {
                var value = reader[0];

                if (value != null && !DBNull.Value.Equals(value))
                {
                    if (type == TypeHelper.TypeOfString)
                    {
                        target = Activator.CreateInstance(TypeHelper.TypeOfString, SafeClrConvert.ToString(value).ToCharArray());
                    }
                    else
                    {
                        target = SafeClrConvert.ChangeType(value, type);
                    }
                }

                return;
            }

            var props = propertyCache.GetOrAdd(type, new ConcurrentDictionary<BindingFlags, PropertyInfo[]>());
            var properties = props.GetOrAdd(BindingFlags.Instance, type.GetProperties(BindingFlags.Instance));

            for (var index = 0; index < reader.FieldCount; index++)
            {
                var name = reader.GetName(index);
                var prop = properties.FirstOrDefault(p => p.CanWrite && string.Compare(p.Name, name, StringComparison.OrdinalIgnoreCase) == 0);
                
                if (prop != null)
                {
                    var value = reader.GetValue(index);

                    if (value != null && !DBNull.Value.Equals(value))
                    {
                        var _value = value.ConvertTo(prop.PropertyType);

                        prop.SetValue(target, _value);
                    }
                }
            }
        }
    }
}