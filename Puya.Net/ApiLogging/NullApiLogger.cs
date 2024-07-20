using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.ApiLogging
{
    public class NullApiLogger : IApiLogger
    {
        public void Log(ApiLog log)
        {
        }

        public Task LogAsync(ApiLog log, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}
