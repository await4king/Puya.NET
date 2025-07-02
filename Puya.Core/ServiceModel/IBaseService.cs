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
        IDb Db { get; }
        ILogger Logger { get; }
        ICacheManager Cache { get; }
        ISettingService Settings { get; }
        ILogProvider LogProvider { get; }
        IDebugger Debugger { get; }
        string Name { get; }
    }
}
