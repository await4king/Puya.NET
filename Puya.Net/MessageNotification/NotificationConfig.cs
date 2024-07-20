using System;

namespace Puya.MessageNotification
{
    public class NotificationConfig
    {
        public byte MaxRetry { get; set; }
        public byte PollSeconds { get; set; }
        public NotificationConfig()
        {
            MaxRetry = 3;
            PollSeconds = 5;
        }
    }
}
