using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public class SmsLoggerNull : ISmsLogger
    {
        public void Log(SmsLog log)
        { }

        public Task LogAsync(SmsLog log, CancellationToken cancellation)
        {
            return Task.CompletedTask;
        }
    }
}
