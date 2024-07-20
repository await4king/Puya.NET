using System;

namespace Puya.MessageNotification
{
    public class Notification
    {
        public int Id { get; set; }
        public string NotificationType { get; set; }
        public NotificationType NotificationStrongType
        {
            get
            {
                Enum.TryParse(NotificationType, out NotificationType result);

                return result;
            }
        }
        public string Target { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int RetryCount { get; set; }
        public bool? Succeeded { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
