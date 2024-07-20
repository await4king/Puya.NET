using Puya.Logging.Models;
using Puya.Logging.Web.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging.Web.Abstractions
{
    public interface IWebLogger : ILogger
    {
        void Log(WebLog log);
        Task LogAsync(WebLog log, CancellationToken cancellation);
    }
}
