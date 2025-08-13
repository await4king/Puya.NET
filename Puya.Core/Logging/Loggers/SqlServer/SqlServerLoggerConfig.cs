namespace Puya.Logging
{
    public class SqlServerLoggerConfig: DbLoggerConfig
    {
        public SqlServerLoggerConfig() : this(null)
        { }
        public SqlServerLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public SqlServerLoggerConfig(ILogFormatter formatter, ILoggingPolicy policy) : base(formatter, policy)
        {
            LogTable = "dbo.Logs";
        }
    }
}
