namespace Puya.Logging
{
    public class DbLoggerConfig : BaseLoggerConfig
    {
        public string LogTable { get; set; }
        public int MaxLog { get; set; }
        public int MaxDailyLog { get; set; }
        public DbLoggerConfig() : this(null)
        { }
        public DbLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public DbLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        {
            MaxDailyLog = -1;
            MaxLog = -1;
        }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new JsonLogFormatter();
        }
    }
}
