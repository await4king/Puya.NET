using System;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.ApiLogging
{
    public interface IApiLogger
    {
        void Log(ApiLog log);
        Task LogAsync(ApiLog log, CancellationToken cancellation);
    }
}
