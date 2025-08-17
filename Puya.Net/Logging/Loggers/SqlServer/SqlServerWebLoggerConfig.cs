namespace Puya.Logging
{
    public class SqlServerWebLoggerConfig : SqlServerLoggerConfig
    {
        public SqlServerWebLoggerConfig() : this(null)
        { }
        public SqlServerWebLoggerConfig(ILoggingPolicy policy) : base(policy)
        {
            LogTable = "dbo.WebLogs";
        }
    }
}
