using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Collections;

namespace Puya.Settings
{
    public class NullSettingService : ISettingService
    {
        protected CaseInsensitiveDictionary<string> items;
        public NullSettingService()
        {
            items = new CaseInsensitiveDictionary<string>();
        }
        public string this[string key]
        {
            get { return items[key]; }
            set { items[key] = value; }
        }

        public string Get(string key)
        {
            return items[key];
        }

        public IDictionary<string, string> GetAll()
        {
            return items;
        }

        public Task<IDictionary<string, string>> GetAllAsync(CancellationToken cancellation)
        {
            return Task.FromResult(items as IDictionary<string, string>);
        }

        public Task<string> GetAsync(string key, CancellationToken cancellation)
        {
            return Task.FromResult(items[key]);
        }

        public IDictionary<string, string> GetRange(string prefix)
        {
            var filtered = items.Where(x => x.Key.StartsWith(prefix));
            var result = new CaseInsensitiveDictionary<string>();

            foreach (var item in filtered)
            {
                result.Add(item.Key, item.Value);
            }

            return result;
        }

        public Task<IDictionary<string, string>> GetRangeAsync(string prefix, CancellationToken cancellation)
        {
            var result = GetRange(prefix);

            return Task.FromResult(result);
        }

        public bool Set(string key, string value)
        {
            items[key] = value;

            return true;
        }

        public Task<bool> SetAsync(string key, string value, CancellationToken cancellation)
        {
            items[key] = value;

            return Task.FromResult(true);
        }
    }
}
