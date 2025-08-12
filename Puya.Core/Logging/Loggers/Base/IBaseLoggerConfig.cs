namespace Puya.Logging
{
    public interface IBaseLoggerConfig
    {
        int? AppId { get; set; }
        string User { get; set; }
        LogLevel Level { get; set; }
        ILogFormatter Formatter { get; set; }
    }
}
