using Puya.Conversion;
using Puya.Data;
using Puya.Logging;

namespace Puya.Core.Tests.Logging
{
    public class PuyaLoggingSqlServer
    {
        IDb GetDb()
        {
            var constrProvider = new DefaultConnectionStringProvider();

            constrProvider.SetConnectionString("Server=.\\I2k17;Database=MyDb;Trusted_Connection=True;");

            var db = new SqlServerDb(constrProvider);

            return db;
        }
        SqlServerLoggerConfig GetDbLoggerConfig()
        {
            return new SqlServerLoggerConfig { LogTable = "dbo.Logs" };
        }
        [Fact]
        public void Test_DbLogger_LogsTable_Exists()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig();
            var tbl = config.LogTable;
            var schema = "dbo";
            var index = tbl.IndexOf(".");

            if (index > 0)
            {
                schema = tbl.Substring(0, index);
                tbl = tbl.Substring(index + 1);
            }

            var exists = db.ExecuteScalarSql(@"
select case when exists
(
    select 1
    from            sys.tables  t
        inner join  sys.schemas s on t.schema_id = s.schema_id
    where t.name = @tbl and s.name = @schema
) then 1 else 0 end", new { tbl, schema });

            Assert.True(SafeClrConvert.ToBoolean(exists));
        }
        [Fact]
        public void Test_DbLogger_Clear()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig();
            var logger = new SqlServerLogger(config, db);

            logger.Clear();

            var logs = logger.FetchLogs();

            Assert.True(SafeClrConvert.ToInt(logs.Count)  == 0);
        }
        [Fact]
        public void Test_DbLogger_Insert()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig();
            var logger = new SqlServerLogger(config, db);

            logger.Clear();
            
            var category = "Category";
            var message = "this is a message";
            var data = new { a = 10 };

            logger.Info(category, message, data);

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
            Assert.Equal(category, logs[0].Category);
            Assert.Equal(message, logs[0].Message);
            Assert.Equal(LogType.Info, logs[0].LogType);
            Assert.NotNull(logs[0].Data);
        }
        [Fact]
        public void Test_DbLogger_LogLevel()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig();

            config.Level = LogLevel.Debug;

            var logger = new SqlServerLogger(config, db);

            logger.Clear();

            logger.Info("test");
            
            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 0);

            config.Level = LogLevel.Info;

            logger.Clear();

            logger.Info("test");

            logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            config.Level = LogLevel.All;

            logger.Clear();

            logger.Info("test");

            logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            config.Level = LogLevel.InfoDebug;

            logger.Clear();

            logger.Debug("test");

            logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);

            config.Level = LogLevel.DebugError;

            logger.Clear();

            logger.Warn("warning");

            logs = logger.FetchLogs();

            Assert.True(logs.Count == 0);
        }
        [Fact]
        public void Test_DbLogger_MaxDailyLog()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig();

            config.MaxDailyLog = 3;

            var logger = new SqlServerLogger(config, db);

            logger.Clear();

            logger.Info("test");
            logger.Info("test");
            logger.Info("test");
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
        [Fact]
        public void Test_DbLogger_MaxLog()
        {
            var db = GetDb();
            var config = GetDbLoggerConfig();

            config.MaxLog = 3;

            var logger = new SqlServerLogger(config, db);

            logger.Clear();

            logger.Info("test");
            logger.Info("test");
            logger.Info("test");
            logger.Info("test");

            var logs = logger.FetchLogs();

            Assert.True(logs.Count == 1);
        }
    }
}
