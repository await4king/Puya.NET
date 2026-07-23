using Puya.Caching;
using Puya.Data;
using Puya.Debugging;
using Puya.Logging;
using Puya.Service;
using Puya.Settings;

namespace Puya.ServiceModel
{
    public interface IBaseService
    {
        IDb Db { get; set; }
        ILogger Logger { get; set; }
        ICacheManager Cache { get; set; }
        ISettingService Settings { get; set; }
        ILogProvider LogProvider { get; set; }
        IDebugger Debugger { get; set; }
        string Name { get; set; }
    }
}
