namespace Puya.Logging
{
    public interface IWebLoggingPolicy
    {
        bool CanLog(Log log);
        void Prepare(WebLog log);
        LogLevel? GetOverridedLogLevel();
    }
}
