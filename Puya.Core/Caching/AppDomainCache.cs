using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Puya.Date;

namespace Puya.Caching
{
    public class AppDomainCache : BaseCache
    {
        public AppDomainCache()
        { }
        public AppDomainCache(INow now) : base(now)
        { }
        public override string CacheName => "AppDomainCache";
        protected override ConcurrentDictionary<string, CacheItem> GetItems()
        {
            var result = (AppDomain.CurrentDomain.GetData(CacheName) as ConcurrentDictionary<string, CacheItem>) ?? new ConcurrentDictionary<string, CacheItem>();

            return result;
        }

        protected override void SetItems(ConcurrentDictionary<string, CacheItem> items)
        {
            AppDomain.CurrentDomain.SetData(CacheName, items);
        }
    }
}
