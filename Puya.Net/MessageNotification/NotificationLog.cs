using System;

namespace Puya.MessageNotification
{
    public class NotificationLog
    {
        public int Id { get; set; }
        public int Parent { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
