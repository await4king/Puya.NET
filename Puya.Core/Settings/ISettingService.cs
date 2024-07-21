using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Settings
{
    public interface ISettingService
    {
        string Get(string key);
        Task<string> GetAsync(string key, CancellationToken cancellation);
        bool Set(string key, string value);
        Task<bool> SetAsync(string key, string value, CancellationToken cancellation);
        IDictionary<string, string> GetAll();
        Task<IDictionary<string, string>> GetAllAsync(CancellationToken cancellation);
        IDictionary<string, string> GetRange(string prefix);
        Task<IDictionary<string, string>> GetRangeAsync(string prefix, CancellationToken cancellation);
        string this[string key] { get; set; }
    }
}
