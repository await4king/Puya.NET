using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Settings
{
    public static class Extensions
    {
        public static Task<IDictionary<string, string>> GetAllAsync(this ISettingService service)
        {
            return service.GetAllAsync(CancellationToken.None);
        }
        public static Task<string> GetAsync(this ISettingService service, string key)
        {
            return service.GetAsync(key, CancellationToken.None);
        }
        public static Task<bool> SetAsync(this ISettingService service, string key, string value)
        {
            return service.SetAsync(key, value, CancellationToken.None);
        }
        public static void SetMany(this ISettingService service, IDictionary<string, string> items)
        {
            foreach (var item in items)
            {
                service.Set(item.Key, item.Value);
            }
        }
        public static Task<bool[]> SetManyAsync(this ISettingService service, IDictionary<string, string> items)
        {
            return service.SetManyAsync(items, CancellationToken.None);
        }
        public static async Task<bool[]> SetManyAsync(this ISettingService service, IDictionary<string, string> items, CancellationToken token)
        {
            var result = new List<bool>();

            foreach (var item in items)
            {
                result.Add(await service.SetAsync(item.Key, item.Value, token));
            }

            return result.ToArray();
        }
    }
}
