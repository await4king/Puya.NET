using Puya.Collections;
using Puya.Data;
using System.Data;

namespace Puya.Logging
{
    public class SqlServerWebLogger: WebDbLogger<SqlServerWebLoggerConfig>
    {
        #region ctor
        public SqlServerWebLogger() : this(null, null, null)
        { }
        public SqlServerWebLogger(IDb db) : this(null, db, null)
        { }
        public SqlServerWebLogger(SqlServerWebLoggerConfig config) : this(config, null, null)
        { }
        public SqlServerWebLogger(SqlServerWebLoggerConfig config, IDb db) : this(config, db, null)
        { }
        public SqlServerWebLogger(SqlServerWebLoggerConfig config, IDb db, ILogger next) : base(config, db, next)
        { }
        #endregion
        protected override string GetInsertLogQuery(out CommandType commandType)
        {
            commandType = CommandType.StoredProcedure;

            if (string.IsNullOrEmpty(Config.LogTable))
            {
                return string.Empty;
            }

            return "usp0_WebLogs_insert";
        }
        protected override DynamicModel GetInsertLogArgs(Log log)
        {
            var wlog = log as WebLog;
            
            var args = new DynamicModel
            {
                ["MaxLog"] = Config.MaxLog,
                ["MaxDailyLog"] = Config.MaxDailyLog,
                ["AppId"] = log.AppId,
                ["LogDate"] = log.LogDate,
                ["LogType"] = log.LogType,
                ["OperationResult"] = log.OperationResult,
                ["Category"] = log.Category,
                ["File"] = log.File,
                ["Line"] = log.Line,
                ["MemberName"] = log.MemberName,
                ["User"] = log.User,
                ["Ip"] = log.Ip,
                ["Message"] = log.Message,
                ["StackTrace"] = log.StackTrace,
                ["Data"] = Config.Formatter?.SerializeData(log),

                ["BrowserName"] = wlog?.BrowserName,
                ["BrowserVersion"] = wlog?.BrowserVersion,
                ["Method"] = wlog?.Method,
                ["Url"] = wlog?.Url,
                ["Referrer"] = wlog?.Referrer,
                ["Headers"] = wlog?.Headers,
                ["Form"] = wlog?.Form,
                ["Cookies"] = wlog?.Cookies
            };

            return args;
        }

        protected override string GetClearQuery(out CommandType commandType)
        {
            commandType = CommandType.Text;

            if (string.IsNullOrEmpty(Config.LogTable))
            {
                return string.Empty;
            }

            return $@"
if object_id(@tbl) is not null
    truncate table {Config.LogTable}";
        }
        protected override DynamicModel GetClearLogArgs()
        {
            var args = new DynamicModel
            {
                ["tbl"] = Config.LogTable,
            };

            return args;
        }
    }
}
