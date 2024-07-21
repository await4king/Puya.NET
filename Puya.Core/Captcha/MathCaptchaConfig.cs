namespace Puya.Captcha
{
    public class MathCaptchaConfig
    {
        private int maxAttempts;
        public int MaxAttempts
        {
            get { return maxAttempts; }
            set
            {
                maxAttempts = value;

                if (maxAttempts <= 0)
                {
                    maxAttempts = 10;
                }
            }
        }
        private int lockDuration;
        public int LockDuration
        {
            get { return lockDuration; }
            set
            {
                lockDuration = value;

                if (lockDuration <= 0)
                {
                    lockDuration = 1200;
                }
            }
        }
        public int MinA { get; set; }
        public int MaxA { get; set; }
        public int MinB { get; set; }
        public int MaxB { get; set; }
        public MathCaptchaConfig()
        {
            maxAttempts = 10;
            lockDuration = 1200;   // 1200 seconds = 20 min
            MinA = 1;
            MinB = 1;
            MaxA = 100;
            MaxB = 100;
        }
    }
}
