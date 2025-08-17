namespace Puya.Logging
{
    public class BaseLoggerConfig: IBaseLoggerConfig
    {
        public int? AppId { get; set; }
        public string User { get; set; }
        public LogLevel Level { get; set; }
        private ILogFormatter formatter;
        public ILogFormatter Formatter
        {
            get
            {
                if (formatter == null)
                {
                    formatter = GetDefaultFormatter();
                }

                if (formatter == null)
                {
                    formatter = new StringLogFormatter();
                }

                return formatter;
            }
            set { formatter = value; }
        }
        public IDetailedLogFormatter DetailedFormatter
        {
            get { return Formatter as IDetailedLogFormatter; }
        }
        ILoggingPolicy policy;
        public ILoggingPolicy Policy
        {
            get
            {
                if (policy == null)
                {
                    policy = new NoLoggingPolicy();
                }

                return policy;
            }
            set { policy = value; }
        }

        public BaseLoggerConfig(): this(null, null)
        { }
        public BaseLoggerConfig(ILoggingPolicy policy) : this(null, policy)
        { }
        public BaseLoggerConfig(ILogFormatter formatter): this(formatter, null) { }
        public BaseLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy)
        {
            Policy = policy;
            Level = LogLevel.All;
            this.formatter = formatter;
        }
        protected virtual ILogFormatter GetDefaultFormatter()
        {
            return null;
        }
    }
}
