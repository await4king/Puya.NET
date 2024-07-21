using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public interface ISmsLogger
    {
        void Log(SmsLog log);
        Task LogAsync(SmsLog log, CancellationToken cancellation);
    }
}
