using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Puya.Base;

namespace Puya.Reflection
{
    public class PropertyIterationResult<TResult>
    {
        public TResult Result { get; set; }
        public bool Break { get; set; }
        public bool Skip { get; set; }
    }
    public static class ReflectionHelper
    {
        public static ConcurrentDictionary<Type, ConcurrentDictionary<BindingFlags, PropertyInfo[]>> PropertyCache { get; private set; }
        public static ConcurrentDictionary<Type, ConcurrentDictionary<BindingFlags, MethodInfo[]>> MethodCache { get; private set; }
        static ReflectionHelper()
        {
            PropertyCache = new ConcurrentDictionary<Type, ConcurrentDictionary<BindingFlags, PropertyInfo[]>>();
            MethodCache = new ConcurrentDictionary<Type, ConcurrentDictionary<BindingFlags, MethodInfo[]>>();
        }
        public static PropertyInfo[] GetProperties(Type type, BindingFlags flags)
        {
            var entry = PropertyCache.GetOrAdd(type, new ConcurrentDictionary<BindingFlags, PropertyInfo[]>());
            var result = entry.GetOrAdd(flags, type.GetProperties(flags));

            return result;
        }
        public static PropertyInfo[] GetPublicInstanceProperties(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var entry = PropertyCache.GetOrAdd(type, new ConcurrentDictionary<BindingFlags, PropertyInfo[]>());
            var result = entry.GetOrAdd(flags, type.GetProperties(flags));

            return result;
        }
        public static PropertyInfo[] GetPublicInstanceReadableProperties(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var entry = PropertyCache.GetOrAdd(type, new ConcurrentDictionary<BindingFlags, PropertyInfo[]>());
            var result = entry.GetOrAdd(flags, type.GetProperties(flags).Where(prop => prop.CanRead).ToArray());

            return result;
        }
        public static PropertyInfo[] GetPublicInstanceWritableProperties(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var entry = PropertyCache.GetOrAdd(type, new ConcurrentDictionary<BindingFlags, PropertyInfo[]>());
            var result = entry.GetOrAdd(flags, type.GetProperties(flags).Where(prop => prop.CanWrite).ToArray());

            return result;
        }
        public static PropertyInfo[] GetProperties<TModel>(BindingFlags flags)
        {
            return GetProperties(typeof(TModel), flags);
        }
        public static PropertyInfo[] GetPublicInstanceProperties<TModel>()
        {
            return GetPublicInstanceProperties(typeof(TModel));
        }
        public static PropertyInfo[] GetPublicInstanceReadableProperties<TModel>()
        {
            return GetPublicInstanceReadableProperties(typeof(TModel));
        }
        public static PropertyInfo[] GetPublicInstanceWritableProperties<TModel>()
        {
            return GetPublicInstanceWritableProperties(typeof(TModel));
        }
        public static void ForEachProperty(Type type, Action<PropertyInfo> callback, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            var props = GetProperties(type, flags);

            foreach (var prop in props)
            {
                callback(prop);
            }
        }
        public static void ForEachProperty<TModel>(Action<PropertyInfo> callback, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            ForEachProperty(typeof(TModel), callback, flags);
        }
        public static void ForEachProperty(Type type, Func<PropertyInfo, bool> callback, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            var props = GetProperties(type, flags);

            foreach (var prop in props)
            {
                if (callback(prop))
                {
                    break;
                }
            }
        }
        public static void ForEachProperty<TModel>(Func<PropertyInfo, bool> callback, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            ForEachProperty(typeof(TModel), callback, flags);
        }
        public static List<TResult> ForEachProperty<TResult>(Type type, Func<PropertyInfo, PropertyIterationResult<TResult>> callback, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            var result = new List<TResult>();
            var props = GetProperties(type, flags);

            foreach (var prop in props)
            {
                var r = callback(prop);

                if (r.Break)
                {
                    break;
                }

                if (!r.Skip)
                {
                    result.Add(r.Result);
                }
            }

            return result;
        }
        public static List<TResult> ForEachProperty<TModel, TResult>(Func<PropertyInfo, PropertyIterationResult<TResult>> callback, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            return ForEachProperty(typeof(TModel), callback, flags);
        }
        public static void ForEachPublicInstanceReadableProperty(Type type, Action<PropertyInfo> callback)
        {
            ForEachProperty(type, prop =>
            {
                if (prop.CanRead)
                {
                    callback(prop);
                }
            });
        }
        public static void ForEachPublicInstanceReadableNotIgnorableProperty(Type type, Action<PropertyInfo> callback)
        {
            ForEachPublicInstanceReadableProperty(type, prop =>
            {
                if (prop.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0)
                {
                    callback(prop);
                }
            });
        }
        public static void ForEachPublicInstanceReadableProperty<TModel>(Action<PropertyInfo> callback)
        {
            ForEachProperty(typeof(TModel), callback);
        }
        public static void ForEachPublicInstanceReadableNotIgnorableProperty<TModel>(Action<PropertyInfo> callback)
        {
            ForEachPublicInstanceReadableNotIgnorableProperty(typeof(TModel), callback);
        }
        public static void ForEachPublicInstanceReadableProperty(Type type, Func<PropertyInfo, bool> callback)
        {
            ForEachProperty(type, prop =>
            {
                if (prop.CanRead)
                {
                    return callback(prop);
                }

                return false;
            });
        }
        public static void ForEachPublicInstanceReadableNotIgnorableProperty(Type type, Func<PropertyInfo, bool> callback)
        {
            ForEachPublicInstanceReadableProperty(type, prop =>
            {
                if (prop.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0)
                {
                    return callback(prop);
                }

                return false;
            });
        }
        public static void ForEachPublicInstanceReadableProperty<TModel>(Func<PropertyInfo, bool> callback)
        {
            ForEachProperty(typeof(TModel), callback);
        }
        public static void ForEachPublicInstanceReadableNotIgnorableProperty<TModel>(Func<PropertyInfo, bool> callback)
        {
            ForEachPublicInstanceReadableNotIgnorableProperty(typeof(TModel), callback);
        }
        public static List<TResult> ForEachPublicInstanceReadableProperty<TResult>(Type type, Func<PropertyInfo, PropertyIterationResult<TResult>> callback)
        {
            return ForEachProperty<TResult>(type, prop =>
            {
                if (prop.CanRead)
                {
                    return callback(prop);
                }

                return new PropertyIterationResult<TResult> { Skip = true };
            });
        }
        public static List<TResult> ForEachPublicInstanceReadableNotIgnorableProperty<TResult>(Type type, Func<PropertyInfo, PropertyIterationResult<TResult>> callback)
        {
            return ForEachPublicInstanceReadableProperty<TResult>(type, prop =>
            {
                if (prop.CustomAttributes.Count(a => a.AttributeType == typeof(IgnoreAttribute)) == 0)
                {
                    return callback(prop);
                }

                return new PropertyIterationResult<TResult> { Skip = true };
            });
        }
        public static List<TResult> ForEachPublicInstanceReadableProperty<TModel, TResult>(Func<PropertyInfo, PropertyIterationResult<TResult>> callback)
        {
            return ForEachProperty(typeof(TModel), callback);
        }
        public static List<TResult> ForEachPublicInstanceReadableNotIgnorableProperty<TModel, TResult>(Func<PropertyInfo, PropertyIterationResult<TResult>> callback)
        {
            return ForEachPublicInstanceReadableNotIgnorableProperty(typeof(TModel), callback);
        }
        public static void ForEachPublicInstanceWritableProperty(Type type, Action<PropertyInfo> callback)
        {
            ForEachProperty(type, prop =>
            {
                if (prop.CanWrite)
                {
                    callback(prop);
                }
            });
        }
        public static void ForEachPublicInstanceWritableProperty<TModel>(Action<PropertyInfo> callback)
        {
            ForEachProperty(typeof(TModel), callback);
        }
        public static void ForEachPublicInstanceWritableProperty(Type type, Func<PropertyInfo, bool> callback)
        {
            ForEachProperty(type, prop =>
            {
                if (prop.CanWrite)
                {
                    return callback(prop);
                }

                return false;
            });
        }
        public static void ForEachPublicInstanceWritableProperty<TModel>(Func<PropertyInfo, bool> callback)
        {
            ForEachProperty(typeof(TModel), callback);
        }
        public static List<TResult> ForEachPublicInstanceWritableProperty<TResult>(Type type, Func<PropertyInfo, PropertyIterationResult<TResult>> callback)
        {
            return ForEachProperty(type, prop =>
            {
                if (prop.CanWrite)
                {
                    return callback(prop);
                }

                return new PropertyIterationResult<TResult> { Skip = true };
            });
        }
        public static List<TResult> ForEachPublicInstanceWritableProperty<TModel, TResult>(Func<PropertyInfo, PropertyIterationResult<TResult>> callback)
        {
            return ForEachProperty(typeof(TModel), callback);
        }
    }
}
