using System;
using System.Threading;
using System.Threading.Tasks;
using Puya.Data;

namespace Puya.Logging
{
    public class DynamicLogger : BaseLogger<DynamicLoggerConfig>
    {
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
            return new ConsoleLogger(new ConsoleLoggerConfig(Config?.Formatter), Next);
        }
        protected virtual ILogger GetDebugLogger()
        {
            return new DebugLogger(new DebugLoggerConfig(Config?.Formatter), Next);
        }
        protected virtual ILogger GetMemoryLogger()
        {
            return new MemoryLogger(new MemoryLoggerConfig(Config?.Formatter), Next);
        }
        protected virtual ILogger GetFileLogger()
        {
            return new FileLogger(new FileLoggerConfig(Config?.Formatter), Next);
        }
        protected virtual ILogger GetSqlServerLogger()
        {
            return new SqlServerLogger(new SqlServerLoggerConfig(), Db, Next);
        }
        protected virtual ILogger GetXmlLogger()
        {
            return new XmlFileLogger(new XmlFileLoggerConfig(Config?.Formatter), Next);
        }
        protected virtual ILogger GetJsonLogger()
        {
            return new JsonFileLogger(new JsonFileLoggerConfig(Config?.Formatter), Next);
        }
        protected virtual ILogger GetCsvLogger()
        {
            return new FormattedFileLogger(new FormattedFileLoggerConfig(Config?.Formatter), Next);
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

        public override void Clear()
        {
            logger.Clear();
        }
        public override Task ClearAsync(CancellationToken cancellation)
        {
            return logger.ClearAsync(cancellation);
        }
    }
}
