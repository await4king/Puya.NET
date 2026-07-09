using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Puya.Base;
using Puya.Collections;
using Puya.Conversion;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Mapping
{
    public class DefaultMapper : IMapper
    {
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

            if (type != null && source != null)
            {
                var targetProps = ReflectionHelper.GetPublicInstanceReadableProperties(type);

                if (type.IsConstructable())
                {
                    try
                    {
                        result = ObjectActivator.Instance.Activate(type);
                    }
                    catch
                    { }
                }
                else
                {
                    result = new DynamicModel();
                }
                
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
            if (reader == null || reader.IsClosed)
            {
                return;
            }

            if (target == null)
            {
                var _target = new DynamicModel();
                
                target = _target;

                for (var index = 0; index < reader.FieldCount; index++)
                {
                    var name = reader.GetName(index);
                    var value = reader.GetValue(index);

                    _target[name] = value;
                }

                return;
            }

            var type = target.GetType();

            if (type.IsDictionary<string, object>())
            {
                var _target = target as IDictionary<string, object>;

                for (var index = 0; index < reader.FieldCount; index++)
                {
                    var name = reader.GetName(index);
                    var value = reader.GetValue(index);

                    if (_target.ContainsKey(name))
                    {
                        _target[name] = value;
                    }
                    else
                    {
                        _target.Add(name, value);
                    }
                }

                return;
            }

            if (type.IsDictionary())
            {
                for (var index = 0; index < reader.FieldCount; index++)
                {
                    var name = reader.GetName(index);
                    var value = reader.GetValue(index);

                    target.TrySetDictionaryItem(name, value);
                }

                return;
            }

            if (type.IsNullableOrBasicType())
            {
                var value = reader[0];

                if (value != null && !DBNull.Value.Equals(value))
                {
                    if (type == TypeHelper.TypeOfString)
                    {
                        target = SafeClrConvert.ToString(value);
                    }
                    else
                    {
                        var _value = value.ConvertTo(type);

                        if (_value != null)
                        {
                            if (_value.GetType().IsBasicType())
                            {
                                target = _value;
                            }
                        }
                        else
                        {
                            // last chance.
                            // this is rare to occur though, since Object.ConvertTo() extension method
                            // is smart and able to successfully convert DataReader's current column value

                            target = SafeClrConvert.ChangeType(value, type);
                        }
                    }
                }

                return;
            }

            var properties = ReflectionHelper.GetPublicInstanceReadableProperties(type);
            
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