using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging
{
    public class NullLogger : ILogger
    {
        public void Clear()
        { }
        public Task ClearAsync(CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
        public void Log(Log log)
        { }
        public Task LogAsync(Log log, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}
