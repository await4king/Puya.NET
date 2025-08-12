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
        public BaseLoggerConfig(): this(null)
        { }
        public BaseLoggerConfig(ILogFormatter formatter)
        {
            Level = LogLevel.All;
            _formatter = formatter;
        }
        protected virtual ILogFormatter GetDefaultFormatter()
        {
            return null;
        }
    }
}
