using Puya.Collections;
using Puya.Data;
using System.Data;

namespace Puya.Logging
{
    public class SqlServerWebLogger: SqlServerLogger
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
            var args = new DynamicModel
            {
                ["LogTable"] = Config.LogTable,
                ["MaxLog"] = Config.MaxLog,
                ["MaxDailyLog"] = Config.MaxDailyLog,
                ["ThreadId"] = log.ThreadId,
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

                ["BrowserName"] = log.BrowserName,
                ["BrowserVersion"] = log.BrowserVersion,
                ["Method"] = log.Method,
                ["ContentType"] = log.ContentType,
                ["Url"] = log.Url,
                ["Referrer"] = log.Referrer,
                ["Headers"] = log.Headers,
                ["Form"] = log.Form,
                ["Cookies"] = log.Cookies,
                ["Body"] = log.Body
            };

            return args;
        }
    }
}
