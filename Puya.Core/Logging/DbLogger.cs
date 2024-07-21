using Puya.Base;
using Puya.Data;
using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging
{
    public abstract class DbLogger : BaseLogger
    {
        public IDbLoggerConfig DbConfig
        {
            get { return Config as IDbLoggerConfig; }
        }
        public virtual IDb Db { get; set; }
        #region ctor
        public DbLogger() : this(null, null)
        { }
        public DbLogger(IDb db) : this(db, null)
        { }
        public DbLogger(IDb db, ILogger next) : base(next)
        {
            Db = db;
        }
        #endregion
        protected virtual bool Init(Log log, out string query)
        {
            if (log == null || Db == null)
            {
                query = "";

                return false;
            }

            if (log.AppId == null)
            {
                log.AppId = Config.AppId;
            }

            if (string.IsNullOrEmpty(log.User))
            {
                log.User = Config.User;
            }

            query = $@"
                    insert into dbo.Logs
                    (
                        AppId
                        LogType,
                        OperationResult,
                        File,
                        Line,
                        MemberName,
                        Message,
                        StackTrace,
                        Ip,
                        User,
                        Category,
                        LogDate,
                        Data
                    )
                    values
                    (
                        @AppId,
                        @LogType,
                        @OperationResult,
                        @File,
                        @Line,
                        @MemberName,
                        @Message,
                        @StackTrace,
                        @Ip,
                        @User,
                        @Category,
                        @LogDate,
                        @Data
                    )
                ";

            var data = string.Empty;

            if (Config.Formatter.DataConverter != null)
                data = Config.Formatter.DataConverter.Serialize(log.DataObject);
            if (string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(log.Data))
                data = log.Data;

            log.Data = data;

            return true;
        }
        protected override void LogInternal(Log log)
        {
            string query;

            if (!Init(log, out query))
            {
                return;
            }

            Db.ExecuteNonQuerySql(query, new
            {
                log.AppId,
                log.LogType,
                log.OperationResult,
                log.File,
                log.Line,
                log.MemberName,
                log.Message,
                log.StackTrace,
                log.Ip,
                log.User,
                log.Category,
                log.LogDate,
                log.Data
            });
        }
        protected override Task LogInternalAsync(Log log, CancellationToken cancellation)
        {
            string query;

            if (!Init(log, out query))
            {
                return Task.CompletedTask;
            }

            return Db.ExecuteNonQuerySqlAsync(query, new
            {
                log.AppId,
                log.LogType,
                log.OperationResult,
                log.File,
                log.Line,
                log.MemberName,
                log.Message,
                log.StackTrace,
                log.Ip,
                log.User,
                log.Category,
                log.LogDate,
                log.Data
            }, cancellation);
        }
        protected virtual string GetClearQuery()
        {
            return "delete from dbo.Logs";
        }
        public override void Clear()
        {
            Db.ExecuteNonQuerySql(GetClearQuery());
        }
        public override Task ClearAsync(CancellationToken cancellation)
        {
            return Db.ExecuteNonQuerySqlAsync(GetClearQuery(), null, cancellation);
        }
    }
}
