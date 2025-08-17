using System;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.Logging
{
    public class DynamicLogger : BaseLogger<DynamicLoggerConfig>
    {
        public DynamicLogger(DynamicLoggerConfig config, IDb db) : this(config, db, null)
        { }
        public DynamicLogger(DynamicLoggerConfig config, IDb db, ILogger next) : base(config, next)
        {
            Db = db;
        }
        private ILogger logger;
        public ILogger Instance
        {
            get
            {
                if (logger == null)
                {
                    logger = new NullLogger();
                    type = "null";
                }

                return logger;
            }
        }
        protected override void LogInternal(Log log)
        {
            Instance.Log(log);
        }
        protected override Task LogInternalAsync(Log log, CancellationToken cancellation)
        {
            return Instance.LogAsync(log, cancellation);
        }
        #region GetLoggers
        protected virtual ILogger GetLogger(string type)
        {
            var result = null as ILogger;

            switch (type.ToLower())
            {
                case "console":
                    result = GetConsoleLogger();
                    break;
                case "debug":
                    result = GetDebugLogger();
                    break;
                case "memory":
                    result = GetMemoryLogger();
                    break;
                case "file":
                    result = GetFileLogger();
                    break;
                case "sqlserver":
                    result = GetSqlServerLogger();
                    break;
                case "xml":
                    result = GetXmlLogger();
                    break;
                case "json":
                    result = GetJsonLogger();
                    break;
                case "csv":
                    result = GetCsvLogger();
                    break;
                case "null":
                    result = new NullLogger();
                    break;
                default:
                    if (Config.ThrowOnInvalidLoggers)
                    {
                        throw new Exception($"Logger '{type}' not supported");
                    }
                    break;
            }

            return result;
        }
        protected virtual ILogger GetConsoleLogger()
        {
            return new ConsoleLogger(new ConsoleLoggerConfig());
        }
        protected virtual ILogger GetDebugLogger()
        {
            return new DebugLogger(new DebugLoggerConfig());
        }
        protected virtual ILogger GetMemoryLogger()
        {
            return new MemoryLogger(new MemoryLoggerConfig());
        }
        protected virtual ILogger GetFileLogger()
        {
            return new FileLogger(new FileLoggerConfig());
        }
        protected virtual ILogger GetSqlServerLogger()
        {
            return new SqlServerLogger(new SqlServerLoggerConfig(), Db);
        }
        protected virtual ILogger GetXmlLogger()
        {
            return new XmlFileLogger(new XmlFileLoggerConfig());
        }
        protected virtual ILogger GetJsonLogger()
        {
            return new JsonFileLogger(new JsonFileLoggerConfig());
        }
        protected virtual ILogger GetCsvLogger()
        {
            return new FormattedFileLogger(new FormattedFileLoggerConfig());
        }
        #endregion
        private string type;
        public string Type
        {
            get
            {
                if (string.IsNullOrEmpty(type))
                {
                    type = "null";
                }

                return type;
            }
            set
            {
                var oldLogger = logger;

                logger = GetLogger(value);

                if (logger != null)
                {
                    type = value;
                }
                else
                {
                    logger = oldLogger;
                }
            }
        }

        public IDb Db { get; set; }

        protected override void ClearInternal()
        {
            logger.Clear();
        }
    }
}
