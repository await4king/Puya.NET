using System.Threading;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public interface ILogger
    {
        void Log(Log log);
        Task LogAsync(Log log, CancellationToken cancellation);
        void Clear();
        Task ClearAsync(CancellationToken cancellation);
    }
}
