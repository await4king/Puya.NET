using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.MessageNotification
{
    public class MessageNotifier : IMessageNotification
    {
        private readonly IDb db;
        public MessageNotifier(IDb db)
        {
            this.db = db;
        }
        public Task Notify(NotificationType notificationType, string target, string subject, string content, CancellationToken cancellation)
        {
            return db.ExecuteNonQuerySqlAsync($@"
insert into dbo.Notifications(NotificationType, Target, Subject, [Content])
values (@NotificationType, @Target, @Subject, @Content)", new { NotificationType = notificationType.ToString(), target, subject, content });
        }
    }
}
