namespace Puya.Logging
{
    public class SqlServerLoggerConfig: DbLoggerConfig
    {
        public SqlServerLoggerConfig()
        {
            LogTable = "dbo.Logs";
        }
    }
}
