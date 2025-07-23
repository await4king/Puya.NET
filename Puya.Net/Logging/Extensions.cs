namespace Puya.Logging
{
    public static class Extensions
    {
        public static void Init(this BaseLoggerConfig config)
        {
            var webLoggerConfig = config as IBaseWebLoggerConfig;

            if (webLoggerConfig != null)
            {
                var logLevel = webLoggerConfig.WebPolicy?.GetOverridedLogLevel();

                if (logLevel.HasValue && logLevel.Value != LogLevel.None)
                {
                    config.Level = logLevel.Value;
                }
            }
        }
    }
}
