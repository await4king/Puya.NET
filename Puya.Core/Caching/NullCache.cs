using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Caching
{
    public class NullCache : ICache
    {
        public void Clear()
        {
        }

        public bool Exists(string key)
        {
            return false;
        }

        public object Get(string key)
        {
            return null;
        }

        public ICollection<string> GetKeys()
        {
            return new string[] { };
        }

        public object GetOrSet(string key, object value, int? duration = null)
        {
            return value;
        }

        public object GetOrSet(string key, Func<object> valueFactory, int? duration = null)
        {
            return valueFactory();
        }

        public object Remove(string key)
        {
            return null;
        }

        public void Set(string key, object value, int? duration = null)
        { }
    }
}
