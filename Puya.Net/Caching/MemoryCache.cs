using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Puya.Date;

namespace Puya.Caching
{
    public class MemoryCache: BaseCache
    {
        public MemoryCache() : this(null)
        { }
        public MemoryCache(INow now) : base(now)
        {
            items = new ConcurrentDictionary<string, CacheItem>();
        }
        ConcurrentDictionary<string, CacheItem> items;
        public override string CacheName => "MemoryCache";
        protected override ConcurrentDictionary<string, CacheItem> GetItems()
        {
            return items;
        }
        protected override void SetItems(ConcurrentDictionary<string, CacheItem> items)
        {
            this.items = items;
        }
    }
}
