using Puya.Base;
using Puya.Data;
using Puya.Logging.Models;
using Puya.Logging.Web.Abstractions;
using Puya.Logging.Web.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Puya.Logging.Web.Abstractions
{
    public abstract class WebDbLogger : BaseWebLogger, IWebLogger
    {
        public virtual IDb Db { get; set; }
        public WebDbLoggerConfig WebDbConfig
        {
            get { return Config as WebDbLoggerConfig; }
        }
        #region ctor
        public WebDbLogger() : this(null, null)
        { }
        public WebDbLogger(IDb db) : this(db, null)
        { }
        public WebDbLogger(IDb db, ILogger next) : base(next)
        {
            Db = db;
        }
        #endregion
        private bool Init(WebLog log, out string query)
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

            WebDbConfig.Prepare(log);

            var data = string.Empty;

            if (Config.Formatter.DataConverter != null)
                data = Config.Formatter.DataConverter.Serialize(log.DataObject);
            if (string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(log.Data))
                data = log.Data;

            log.Data = data;

            query = "usp0_WebLogs_add";

            return true;
        }
        protected override void LogInternal(WebLog log)
        {
            string query;

            if (!Init(log, out query))
            {
                return;
            }

            Db.ExecuteNonQueryCommand(query, new
            {
                log.AppId,
                log.LogDate,
                log.LogType,
                log.OperationResult,
                log.Category,
                log.File,
                log.Line,
                log.MemberName,
                log.User,
                log.Ip,
                log.Message,
                log.StackTrace,
                log.Data,
                log.BrowserName,
                log.BrowserVersion,
                log.Method,
                log.Url,
                log.Referrer,
                log.Headers,
                log.Form,
                log.Cookies
            });
        }
        protected override Task LogInternalAsync(WebLog log, CancellationToken cancellation)
        {
            string query;

            if (!Init(log, out query))
            {
                return Task.CompletedTask;
            }

            return Db.ExecuteNonQueryCommandAsync(query, new
            {
                log.AppId,
                log.LogDate,
                log.LogType,
                log.OperationResult,
                log.Category,
                log.File,
                log.Line,
                log.MemberName,
                log.User,
                log.Ip,
                log.Message,
                log.StackTrace,
                log.Data,
                log.BrowserName,
                log.BrowserVersion,
                log.Method,
                log.Url,
                log.Referrer,
                log.Headers,
                log.Form,
                log.Cookies
            }, cancellation);
        }
        protected override void LogInternal(Log log)
        {
            LogInternal(new WebLog(log));
        }
        protected override Task LogInternalAsync(Log log, CancellationToken cancellation)
        {
            return LogInternalAsync(new WebLog(log), cancellation);
        }
    }
}
