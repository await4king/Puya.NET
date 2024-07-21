using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Puya.Date;

namespace Puya.Caching
{
    public abstract class BaseCache: ICache
    {
        public class CacheItem
        {
            public string Key { get; set; }
            public object Value { get; set; }
            private int _duration;
            public int Duration
            {
                get { return _duration; }
                set
                {
                    _duration = (value >= 0) ? value : 0;
                }
            }
            public int Hits { get; set; }
            public DateTime LastAccess { get; set; }
            public bool IsValid(INow now)
            {
                return (now.Value - LastAccess).TotalSeconds < Duration;
            }
            public CacheItem Copy(CacheItem item)
            {
                this.Key = item.Key;
                this.Duration = item.Duration;
                this.Value = item.Value;
                this.Hits = item.Hits;
                this.LastAccess = item.LastAccess;

                return this;
            }
        }
        public abstract string CacheName { get; }
        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = (value >= 0) ? value : 0;
            }
        }
        protected abstract ConcurrentDictionary<string, CacheItem> GetItems();
        protected abstract void SetItems(ConcurrentDictionary<string, CacheItem> items);
        private INow _now;
        protected INow Now
        {
            get
            {
                if (_now == null)
                {
                    _now = NowProvider.Instance;
                }

                return _now;
            }
            set { _now = value; }
        }
        public BaseCache(): this(null)
        { }
        public BaseCache(INow now)
        {
            Now = now;
        }
        public bool Exists(string key)
        {
            var items = GetItems();

            CacheItem ci;

            if (items.TryGetValue(key, out ci))
            {
                return ci.IsValid(Now);
            }

            return false;
        }

        public object Get(string key)
        {
            var result = null as object;
            var items = GetItems();

            CacheItem ci;

            if (items.TryGetValue(key, out ci))
            {
                var isValid = ci.IsValid(Now);

                if (isValid)
                {
                    ci.LastAccess = Now.Value;
                    ci.Hits++;
                }
                else
                {
                    ci.Hits = 0;
                }

                items.AddOrUpdate(key, ci, (k, old) => old.Copy(ci));

                result = isValid ? ci.Value : null;

                SetItems(items);
            }

            return result;
        }

        public void Set(string key, object value, int? duration = null)
        {
            var items = GetItems();

            items.AddOrUpdate(key, new CacheItem { Key = key, Value = value, Duration = duration ?? Duration, LastAccess = Now.Value }, (k, old) =>
            {
                old.Value = value;
                old.Duration = duration ?? old.Duration;
                old.LastAccess = Now.Value;
                old.Hits++;

                return old;
            });

            SetItems(items);
        }
        
        public object GetOrSet(string key, object value, int? duration = null)
        {
            var items = GetItems();
            var ci = items.GetOrAdd(key, k => new CacheItem { Key = key, Value = value, Duration = duration ?? Duration, LastAccess = Now.Value });
            var isValid = ci.IsValid(Now);

            if (isValid)
            {
                ci.Hits++;
            }
            else
            {
                ci.Hits = 0;
                ci.Value = value;
            }

            ci.LastAccess = Now.Value;

            items.AddOrUpdate(key, ci, (k, old) => old.Copy(ci));

            var result = ci.Value;

            SetItems(items);

            return result;
        }

        public object GetOrSet(string key, Func<object> valueFactory, int? duration = null)
        {
            var items = GetItems();
            var ci = items.GetOrAdd(key, k => new CacheItem { Key = key, Value = valueFactory(), Duration = duration ?? Duration, LastAccess = Now.Value });
            var isValid = ci.IsValid(Now);

            if (isValid)
            {
                ci.Hits++;
            }
            else
            {
                ci.Hits = 0;
                ci.Value = valueFactory();
            }

            ci.LastAccess = Now.Value;

            items.AddOrUpdate(key, ci, (k, old) => old.Copy(ci));

            var result = ci.Value;

            SetItems(items);

            return result;
        }

        public object Remove(string key)
        {
            var items = GetItems();
            var result = null as object;

            CacheItem ci;

            if (items.TryRemove(key, out ci))
            {
                result = ci.Value;
            }

            SetItems(items);

            return result;
        }

        public ICollection<string> GetKeys()
        {
            return GetItems().Keys;
        }

        public void Clear()
        {
            SetItems(new ConcurrentDictionary<string, CacheItem>());
        }
        public ConcurrentDictionary<string, CacheItem> GetAll()
        {
            return GetItems();
        }
    }
}