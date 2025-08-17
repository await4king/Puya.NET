namespace Puya.Logging
{
    public class MemoryLoggerConfig : BaseLoggerConfig
    {
        public MemoryLoggerConfig() : this(null)
        { }
        public MemoryLoggerConfig(ILoggingPolicy policy) : base(policy)
        { }
        public int MaxLogCount { get; set; }
        protected override ILogFormatter GetDefaultFormatter()
        {
            return new StringLogFormatter();
        }
    }
}
