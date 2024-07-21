using Hangfire;
using System;

namespace Puya.Notification
{
    public class HangFireSmsNotifier : ISmsNotifier
    {
        private readonly SmsSender sms;
        private readonly IBackgroundJobClient backgroundJobClient;

        public HangFireSmsNotifier(SmsSender sms, IBackgroundJobClient backgroundJobClient)
        {
            this.sms = sms;
            this.backgroundJobClient = backgroundJobClient;
        }
        public void Notify(string target, string message)
        {
            var mobiles = target?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var mobile in mobiles)
            {
                backgroundJobClient.Enqueue(() => sms.Send(mobile, message));
            }
        }
    }
}
