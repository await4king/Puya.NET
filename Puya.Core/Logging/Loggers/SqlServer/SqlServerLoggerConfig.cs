namespace Puya.Logging
{
    public class SqlServerLoggerConfig: DbLoggerConfig
    {
        public SqlServerLoggerConfig() : this(null)
        { }
        public SqlServerLoggerConfig(ILoggingPolicy policy) : base(policy)
        {
            LogTable = "dbo.Logs";
        }
    }
}
