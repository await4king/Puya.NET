namespace Puya.Logging
{
    public class MemoryLoggerConfig : BaseLoggerConfig
    {
        public MemoryLoggerConfig() : this(null)
        { }
        public MemoryLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public MemoryLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        { }
        public int MaxLogCount { get; set; }
    }
}
