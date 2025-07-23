namespace Puya.Logging
{
    public class MemoryLoggerConfig : BaseLoggerConfig
    {
        public MemoryLoggerConfig() : this(null)
        { }
        public MemoryLoggerConfig(ILogFormatter formatter) : base(formatter)
        { }
        public int MaxLogCount { get; set; }
    }
}
