using System.Threading.Tasks;
using System.Threading;

namespace Puya.MessageNotification
{
    public interface IMessageNotification
    {
        Task Notify(NotificationType notificationType, string target, string subject, string content, CancellationToken cancellation);
    }
}
