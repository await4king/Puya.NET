namespace Puya.Logging
{
    public class SqlServerWebLoggerConfig : WebDbLoggerConfig
    {
        public SqlServerWebLoggerConfig() : this(null, null)
        { }
        public SqlServerWebLoggerConfig(IWebLoggingPolicy webLoggingPolicy, ILogFormatter formatter) : base(formatter, webLoggingPolicy)
        {
            LogTable = "dbo.WebLogs";
        }
    }
}
