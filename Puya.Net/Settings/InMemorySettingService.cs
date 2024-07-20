using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Collections;

namespace Puya.Settings
{
    public class InMemorySettingService : ISettingService
    {
        protected IDictionary<string, string> _items;
        public InMemorySettingService()
        {
            _items = new CaseInsensitiveDictionary<string>(true);
        }
        public virtual string Get(string key)
        {
            return _items[key];
        }
        public virtual int Count { get { return _items.Count; } }

        public virtual string this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value);
            }
        }

        public virtual IDictionary<string, string> GetAll()
        {
            return _items;
        }

        public virtual Task<IDictionary<string, string>> GetAllAsync(CancellationToken cancellation)
        {
            return Task.FromResult(_items);
        }

        public virtual Task<string> GetAsync(string key, CancellationToken cancellation)
        {
            return Task.FromResult(_items[key]);
        }

        public virtual bool Set(string key, string value)
        {
            _items[key] = value;

            return true;
        }

        public virtual Task<bool> SetAsync(string key, string value, CancellationToken cancellation)
        {
            return Task.FromResult(Set(key, value));
        }

        public virtual IDictionary<string, string> GetRange(string prefix)
        {
            var result = new CaseInsensitiveDictionary<string>(true);
            var all = GetAll();

            foreach (var item in all)
            {
                if (item.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }

        public virtual Task<IDictionary<string, string>> GetRangeAsync(string prefix, CancellationToken cancellation)
        {
            return Task.FromResult(GetRange(prefix));
        }
    }
}
