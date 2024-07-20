using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Caching
{
    public interface ICache
    {
        object Get(string key);
        void Set(string key, object value, int? duration = null);   // null or 0: unlimited
        object GetOrSet(string key, object value, int? duration = null);
        object GetOrSet(string key, Func<object> valueFactory, int? duration = null);
        object Remove(string key);
        bool Exists(string key);
        ICollection<string> GetKeys();
        void Clear();
    }
}
