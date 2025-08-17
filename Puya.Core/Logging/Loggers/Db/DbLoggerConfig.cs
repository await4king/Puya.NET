namespace Puya.Logging
{
    public class DbLoggerConfig : BaseLoggerConfig
    {
        public string LogTable { get; set; }
        public int MaxLog { get; set; }
        public int MaxDailyLog { get; set; }
        public DbLoggerConfig() : this(null)
        { }
        public DbLoggerConfig(ILoggingPolicy policy) : base(policy)
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
