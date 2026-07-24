using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Puya.Collections
{
    public class DynamicModelPropertyIndexOutOfRangeException : Exception
    {
        public DynamicModelPropertyIndexOutOfRangeException(int index) : base($"Model object does not have a property at {index} index")
        {
        }
    }
    public class DynamicModelPropertyNotFoundException : Exception
    {
        public DynamicModelPropertyNotFoundException(string name) : base($"Model object does not have a {name} property")
        {
        }
    }
    public class DynamicModel : DynamicModel<object>
    {
        public DynamicModel(): this(null, true)
        { }
        public DynamicModel(IEqualityComparer<string> comparer) : this(comparer, true)
        { }
        public DynamicModel(IEqualityComparer<string> comparer, bool ignoreNotExistingKeys): base(comparer, ignoreNotExistingKeys)
        { }
    }
    public class DynamicModel<T> : DynamicObject, IDictionary<string, T>
    {
        Dictionary<string, T> props;
        public bool IgnoreNotExistingKeys { get; set; }
        public DynamicModel(): this(null, true)
        { }
        public DynamicModel(IEqualityComparer<string> comparer) : this(comparer, true)
        { }
        public DynamicModel(IEqualityComparer<string> comparer = null, bool ignoreNotExistingKeys = true)
        {
            if (comparer == null)
            {
                comparer = StringComparer.OrdinalIgnoreCase;
            }

            props = new Dictionary<string, T>(comparer);
            IgnoreNotExistingKeys = ignoreNotExistingKeys;
        }
        public int Count
        {
            get
            {
                return props.Count;
            }
        }
        public ICollection<string> Keys
        {
            get { return props.Keys; }
        }
        public ICollection<T> Values
        {
            get { return props.Values; }
        }
        public bool IsReadOnly => false;
        public virtual void SetProperty(string name, T value)
        {
            if (props.ContainsKey(name))
            {
                props[name] = value;
            }
            else
            {
                if (IgnoreNotExistingKeys)
                {
                    props.Add(name, value);
                }
                else
                {
                    throw new DynamicModelPropertyNotFoundException(name);
                }
            }
        }
        public virtual T GetProperty(string name)
        {
            if (props.ContainsKey(name))
            {
                return props[name];
            }
            else
            {
                return default(T);
            }
        }
        public virtual bool RemoveProperty(string name)
        {
            return props.Remove(name);
        }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string name = binder.Name;

            if (props.TryGetValue(name, out T res))
            {
                result = res;
                return true;
            }
            else
            {
                result = null;
            }

            return false;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetProperty(binder.Name, (T)value);

            return true;
        }
        public bool ContainsKey(string key)
        {
            return props.ContainsKey(key);
        }
        public void Add(string key, T value)
        {
            SetProperty(key, value);
        }
        public bool Remove(string key)
        {
            return props.Remove(key);
        }
        public bool TryGetValue(string key, out T value)
        {
            return props.TryGetValue(key, out value);
        }
        public void Add(KeyValuePair<string, T> item)
        {
            SetProperty(item.Key, item.Value);
        }
        public void Clear()
        {
            props.Clear();
        }
        public bool Contains(KeyValuePair<string, T> item)
        {
            var _item = props.FirstOrDefault(x =>
            {
                var result = string.Compare(x.Key, item.Key, StringComparison.OrdinalIgnoreCase) == 0;

                if (result)
                {
                    IComparable x1 = x.Value as IComparable;

                    if (x1 != null)
                    {
                        result &= x1.CompareTo(item.Value) == 0;
                    }
                    else
                    {
                        IComparable<object> x2 = x.Value as IComparable<object>;

                        if (x2 != null)
                        {
                            result &= x2.CompareTo(item.Value) == 0;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }

                return result;
            });

            return !string.IsNullOrEmpty(_item.Key);
        }
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            if (array != null && array.Length > 0)
            {
                if (arrayIndex < 0 || arrayIndex >= array.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                else
                {
                    var i = arrayIndex;

                    foreach (var prop in props)
                    {
                        if (i++ < array.Length)
                        {
                            array[i] = prop;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        public bool Remove(KeyValuePair<string, T> item)
        {
            if (!props.ContainsKey(item.Key))
                return false;

            var x = props[item.Key];

            if (x == null && item.Value == null)
                return props.Remove(item.Key);

            if (x.Equals(item.Value))
                return props.Remove(item.Key);
            else
                return false;
        }
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return props.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return props.GetEnumerator();
        }
        public T this[string name]
        {
            get
            {
                return GetProperty(name);
            }
            set
            {
                SetProperty(name, value);
            }
        }
        public T this[int index]
        {
            get
            {
                var keys = Keys;
                var result = default(T);

                if (index >= 0 && index < keys.Count)
                {
                    var i = 0;

                    foreach (var key in keys)
                    {
                        if (i++ == index)
                        {
                            result = props[key];
                            break;
                        }
                    }
                }
                else
                {
                    if (!IgnoreNotExistingKeys)
                    {
                        throw new DynamicModelPropertyIndexOutOfRangeException(index);
                    }
                }

                return result;
            }
            set
            {
                var keys = Keys;

                if (index >= 0 && index < keys.Count)
                {
                    var i = 0;

                    foreach (var key in keys)
                    {
                        if (i++ == index)
                        {
                            SetProperty(key, value);

                            break;
                        }
                    }
                }
                else
                {
                    if (!IgnoreNotExistingKeys)
                    {
                        throw new DynamicModelPropertyIndexOutOfRangeException(index);
                    }
                }
            }
        }
    }
}
