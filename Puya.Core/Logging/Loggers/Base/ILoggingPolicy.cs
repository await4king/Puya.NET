using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public interface ILoggingPolicy
    {
        bool CanLog(ILogger logger, Log log);
        Task InitAsync(ILogger logger, Log log, CancellationToken cancellation);
    }
}
