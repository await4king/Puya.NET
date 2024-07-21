using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using Puya.Base;
using Puya.Collections;
using Puya.Extensions;
using Puya.Reflection;

namespace Puya.Serialization
{
    public static class Extensions
    {
        static void SerializeQueryParam(string prefix, object obj, IDictionary<string, string> query, QuerySerializationOptions options)
        {
            var result = string.Empty;

            if (obj != null)
            {
                var type = obj.GetType();
                
                if (type.IsNullableOrBasicType())
                {
                    if (type == TypeHelper.TypeOfString)
                    {
                        result = WebUtility.UrlEncode(obj.ToString());
                    }
                    else if (type == TypeHelper.TypeOfDateTime || type == TypeHelper.TypeOfDateTimeOffset)
                    {
                        result = WebUtility.UrlEncode(((DateTime)obj).ToUniversalTime().ToString(options.DateTimeFormat));
                    }
                    else if (type == TypeHelper.TypeOfTimeSpan)
                    {
                        result = WebUtility.UrlEncode(((TimeSpan)obj).ToString());
                    }
                    else if (type == TypeHelper.TypeOfBool || type == TypeHelper.TypeOfNullableBool)
                    {
                        result = ((bool)obj).ToString().ToLower();
                    }
                    else if (type.IsEnum && !options.EnumAsString)
                    {
                        result = ((long)obj).ToString();
                    }
                    else
                    {
                        result = obj.ToString();
                    }

                    query[prefix] = result;
                }
                else
                {
                    if (type.IsDictionary())
                    {
                        obj.IterateDictionary(item =>
                        {
                            var key = string.IsNullOrEmpty(prefix) ? item.Key?.ToString() : prefix + "." + item.Key;

                            SerializeQueryParam(key, item.Value, query, options);
                        });
                    }
                    else if (type.IsEnumerable())
                    {
                        var en = (obj as IEnumerable).GetEnumerator();
                        var i = 0;
                        var sb = new StringBuilder();

                        while (en.MoveNext())
                        {
                            var value = WebUtility.UrlEncode(en.Current?.ToString());

                            if (options.ExtendArrays)
                            {
                                if (!options.IgnoreNullOrEmpty || !string.IsNullOrEmpty(value))
                                {
                                    query[prefix + $"[{i}]"] = value;
                                }
                            }
                            else
                            {
                                sb.Append((sb.Length == 0 ? "" : options.ArraySeparator) + value);
                            }

                            i++;
                        }

                        if (!options.ExtendArrays && !string.IsNullOrEmpty(prefix))
                        {
                            query[prefix] = sb.ToString();
                        }
                    }
                    else
                    {
                        ReflectionHelper.ForEachPublicInstanceReadableNotIgnorableProperty(obj.GetType(), prop =>
                        {
                            var indexers = prop.GetIndexParameters();

                            if (indexers == null || indexers.Length == 0)
                            {
                                var key = string.IsNullOrEmpty(prefix) ? prop.Name : prefix + "." + prop.Name;

                                SerializeQueryParam(key, prop.GetValue(obj), query, options);
                            }
                        });
                    }
                }
            }
        }
        public static string ToQuerystring(this object obj, QuerySerializationOptions options = null)
        {
            options = options ?? new QuerySerializationOptions();

            IDictionary<string, string> query;
            
            if (options.CaseSensitivePropNames)
            {
                query = new CaseSensitiveDictionary<string>(true);
            }
            else
            {
                query = new CaseInsensitiveDictionary<string>(true);
            }

            SerializeQueryParam("", obj, query, options);

            var querystring = query.Where(x => !options.IgnoreNullOrEmpty || !string.IsNullOrEmpty(x.Value))
                            .Select(x => $"{(options.EncodePropNames ? WebUtility.UrlEncode(x.Key) : x.Key)}{(string.IsNullOrEmpty(x.Key) ? "": "=")}{x.Value}").Join("&");

            if (options.UseQuestionMark && !string.IsNullOrEmpty(querystring))
            {
                querystring = "?" + querystring;
            }

            return querystring;
        }
    }
}
