using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Api
{
    public class SchemaList<T>
    {
        public List<string> Schema { get; set; }
        public List<List<T>> Data { get; set; }
    }
    public static class ApiExtensions
    {
        public static SchemaList<object> ToSchemaList(this IEnumerable e)
        {
            if (e != null)
            {
                var cols = new List<string>();
                var data = new List<List<object>>();
                var isDictionary = false;
                PropertyInfo keysProp = null;
                PropertyInfo valuesProp = null;

                foreach (var item in e)
                {
                    if (cols.Count == 0 && item != null)
                    {
                        if (item.GetType().Implements(typeof(IDictionary<,>)))
                        {
                            isDictionary = true;

                            if (keysProp == null)
                            {
                                keysProp = item.GetType().GetProperty("Keys");
                            }

                            if (keysProp != null)
                            {
                                var keys = keysProp.GetValue(item) as IEnumerable;

                                if (keys != null)
                                {
                                    foreach (var key in keys)
                                    {
                                        cols.Add(key?.ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            ReflectionHelper.ForEachPublicInstanceReadableProperty(item.GetType(), prop =>
                            {
                                cols.Add(prop.Name);
                            });
                        }
                    }

                    if (item != null)
                    {
                        var values = new List<object>();

                        if (isDictionary)
                        {
                            if (valuesProp == null)
                            {
                                valuesProp = item.GetType().GetProperty("Values");
                            }

                            if (valuesProp != null)
                            {
                                var _values = valuesProp.GetValue(item) as IEnumerable;

                                if (_values != null)
                                {
                                    foreach (var value in _values)
                                    {
                                        values.Add(value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ReflectionHelper.ForEachPublicInstanceReadableProperty(item.GetType(), prop =>
                            {
                                values.Add(prop.GetValue(item));
                            });
                        }

                        data.Add(values);
                    }
                }

                return new SchemaList<object>{ Schema = cols, Data = data };
            }

            return null;
        }
        public static bool Accepts(this Api api, string method)
        {
            return string.IsNullOrEmpty(api.HttpVerbs) || api.HttpVerbs.Split(',').FindIndexOf(method, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
