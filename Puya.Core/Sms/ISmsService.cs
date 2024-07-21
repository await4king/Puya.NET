using System.Threading;
using System.Threading.Tasks;

namespace Puya.Sms
{
    public interface ISmsService
    {
        SendResponse Send(string mobile, string message);
        Task<SendResponse> SendAsync(string mobile, string message, CancellationToken cancellation);
    }
}
