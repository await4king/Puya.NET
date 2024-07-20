using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Sms
{
    public static class Extensions
    {
        public static object ToStrongConfig(this Dictionary<string, string> config, Type type, Action<Exception, string> onError)
        {
            if (type == null)
            {
                return null;
            }

            var ctor = type.GetConstructor(new Type[] { });

            if (ctor == null)
            {
                onError(new ApplicationException($"{type.Name} does not have a default constructor"), "");

                return null;
            }

            var result = ctor.Invoke(new object[] { });

            if (config?.Count > 0)
            {
                foreach (var prop in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (prop.CanRead && prop.CanWrite && config.ContainsKey(prop.Name))
                    {
                        try
                        {
                            var value = System.Convert.ChangeType(config[prop.Name], prop.PropertyType);

                            prop.SetValue(result, value);
                        }
                        catch (Exception e)
                        {
                            onError(e, prop.Name);
                        }
                    }
                }
            }

            return result;
        }
        public static T ToStrongConfig<T>(this Dictionary<string, string> config, Action<Exception, string> onError)
            where T : class, new()
        {
            T result = null;

            if (config != null)
            {
                result = (T)config.ToStrongConfig(typeof(T), onError);
            }

            if (result == null)
            {
                result = new T();
            }

            return result;
        }
        public static bool Has(this Dictionary<string, string> dictionary, string key, string value, bool ignoreCase = true)
        {
            var result = false;

            if (dictionary?.ContainsKey(key) ?? false)
            {
                result = string.Compare(dictionary[key], value, ignoreCase) == 0;
            }

            return result;
        }
    }
}
