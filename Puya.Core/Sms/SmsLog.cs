using System;

namespace Puya.Sms
{
    public class SmsLog
    {
        public int Serial { get; set; }
        public string MobileNo { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public object Response { get; set; }
        public object Data { get; set; }
        public Exception Error { get; set; }
        public bool? Success { get; set; }
        public DateTime LogDate { get; set; }
        public SmsLog()
        {
            LogDate = DateTime.Now;
        }
    }
}
