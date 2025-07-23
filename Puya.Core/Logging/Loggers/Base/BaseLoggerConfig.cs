using Puya.Base;

namespace Puya.Logging
{
    public class BaseLoggerConfig
    {
        public int? AppId { get; set; }
        public string User { get; set; }
        public LogLevel Level { get; set; }
        private ILogFormatter _formatter;
        public ILogFormatter Formatter
        {
            get
            {
                return TypeHelper.EnsureInitialized<ILogFormatter, JsonLogFormatter>(ref _formatter);
            }
            set { _formatter = value; }
        }
        public BaseLoggerConfig(): this(null)
        { }
        public BaseLoggerConfig(ILogFormatter formatter)
        {
            Level = LogLevel.All;
            _formatter = formatter;

            if (_formatter == null)
            {
                _formatter = GetDefaultFormatter();
            }
        }
        protected virtual ILogFormatter GetDefaultFormatter()
        {
            return null;
        }
    }
}
