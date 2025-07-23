
namespace Puya.Logging
{
    public interface IBaseWebLoggerConfig
    {
        IWebLoggingPolicy WebPolicy { get; set; }
    }
}
