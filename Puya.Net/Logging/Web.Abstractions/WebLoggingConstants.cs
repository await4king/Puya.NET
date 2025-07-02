namespace Puya.Logging.Web.Abstractions
{
    public static class WebLoggingConstants
    {
        public static string LogLevelHeaderName { get; set; }
        static WebLoggingConstants()
        {
            LogLevelHeaderName = "x-loglevel";
        }
    }
}
