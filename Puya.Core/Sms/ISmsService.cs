using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public interface ISmsService
    {
        SendResponse Send(string mobile, string message, string category = null);
        Task<SendResponse> SendAsync(string mobile, string message, string category, CancellationToken cancellation);
    }
}
