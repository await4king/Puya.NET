namespace Puya.Logging
{
    public class SqlServerLoggerConfig: DbLoggerConfig
    {
        public SqlServerLoggerConfig() : this(null)
        { }
        public SqlServerLoggerConfig(ILogFormatter formatter): base(formatter)
        {
            LogTable = "dbo.Logs";
        }
    }
}
