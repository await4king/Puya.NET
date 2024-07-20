using System;

namespace Puya.Captcha
{
    public class MathCaptchaItem
    {
        public string Id { get; set; }
        public int A { get; set; }
        public int B { get; set; }
        public byte AttemptCount { get; set; }
        public DateTime LastAttempt { get; set; }
        public MathCaptchaItem()
        {
            LastAttempt = DateTime.Now.AddSeconds(-1);
        }
        public MathCaptchaItem Challenged()
        {
            AttemptCount++;
            LastAttempt = DateTime.Now;

            return this;
        }
        public MathCaptchaItem Reset()
        {
            AttemptCount = 0;
            LastAttempt = DateTime.Now;

            return this;
        }
    }
}
