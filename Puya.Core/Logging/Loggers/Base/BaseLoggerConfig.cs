namespace Puya.Logging
{
    public class BaseLoggerConfig: IBaseLoggerConfig
    {
        public int? AppId { get; set; }
        public string User { get; set; }
        public LogLevel Level { get; set; }
        private ILogFormatter _formatter;
        public ILogFormatter Formatter
        {
            get
            {
                if (_formatter == null)
                {
                    _formatter = GetDefaultFormatter();
                }

                if (_formatter == null)
                {
                    _formatter = new StringLogFormatter();
                }

                return _formatter;
            }
            set { _formatter = value; }
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

        public BaseLoggerConfig(): this(null)
        { }
        public BaseLoggerConfig(ILogFormatter formatter): this(formatter, null) { }
        public BaseLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy)
        {
            Policy = policy;
            Level = LogLevel.All;
            _formatter = formatter;
        }
        protected virtual ILogFormatter GetDefaultFormatter()
        {
            return null;
        }
    }
}
