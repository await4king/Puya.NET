using Puya.Collections;
using Puya.Data;
using System.Data;

namespace Puya.Logging
{
    public class SqlServerLogger : DbLogger<SqlServerLoggerConfig>
    {
        #region ctor
        public SqlServerLogger(): this(null, null, null)
        { }
        public SqlServerLogger(IDb db) : this(null, db, null)
        { }
        public SqlServerLogger(SqlServerLoggerConfig config) : this(config, null, null)
        { }
        public SqlServerLogger(SqlServerLoggerConfig config, IDb db) : this(config, db, null)
        { }
        public SqlServerLogger(SqlServerLoggerConfig config, IDb db, ILogger next) : base(config, db, next)
        {
            Db = db;
        }
        #endregion
        protected override string GetInsertLogQuery(out CommandType commandType)
        {
            commandType = CommandType.StoredProcedure;

            if (string.IsNullOrEmpty(Config.LogTable))
            {
                return string.Empty;
            }

            return $@"usp0_Log_insert";
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
