using System;
using System.Collections.Generic;
using System.Text;
using Puya.Sms;

namespace Puya.Notification
{
    public class SmsSender
    {
        private readonly ISmsService sms;

        public SmsSender(ISmsService sms)
        {
            this.sms = sms;
        }
        public void Send(string mobile, string message)
        {
            var sr = sms.Send(mobile, message);
        }
    }
}
