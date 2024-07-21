using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Base;
using Puya.Reflection;

namespace Puya.Extensions
{
    public static class ObjectHelper
    {
        public static void SetProperty(ref object instance, Type type, string propertyName, object value, bool ignoreCase = false)
        {
            if (instance != null)
            {
                var _type = instance?.GetType() ?? type;

                if (_type != null)
                {
                    var properties = ReflectionHelper.GetPublicInstanceProperties(_type);
                    var dotIndex = propertyName.IndexOf('.');

                    if (dotIndex < 0)
                    {
                        var prop = properties.FirstOrDefault(p => p.CanWrite && string.Compare(p.Name, propertyName, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0);

                        if (prop != null)
                        {
                            var _value = ObjectExtensions.ConvertTo(value, prop.PropertyType);

                            if (_value != null || !prop.PropertyType.IsSimpleType() || prop.PropertyType == TypeHelper.TypeOfString)
                            {
                                prop.SetValue(instance, _value);
                            }
                        }
                    }
                    else
                    {
                        var innerPropertyName = propertyName.Substring(0, dotIndex);

                        if (!string.IsNullOrEmpty(innerPropertyName))
                        {
                            var innerProp = properties.FirstOrDefault(p => string.Compare(p.Name, innerPropertyName, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0);

                            if (innerProp != null)
                            {
                                var innerObject = innerProp.GetValue(instance);

                                if (innerObject == null)
                                {
                                    try
                                    {
                                        innerObject = ObjectActivator.Instance.Activate(innerProp.PropertyType);
                                        innerProp.SetValue(instance, innerObject);
                                    }
                                    catch { }
                                }

                                if (innerObject != null)
                                {
                                    SetProperty(ref innerObject, innerProp.PropertyType, propertyName.Substring(dotIndex + 1), value, ignoreCase);
                                }
                            }
                        }
                    }
                }
            }
        }
        public static void SetProperty(object instance, string propertyName, object value, bool ignoreCase = false)
        {
            SetProperty(ref instance, instance?.GetType(), propertyName, value, ignoreCase);
        }
    }
}
