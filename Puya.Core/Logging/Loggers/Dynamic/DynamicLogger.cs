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
            return new SqlServerLogger(new SqlServerLoggerConfig(Config?.Formatter), Db, Next);
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
            return new CsvFileLogger(new CsvFileLoggerConfig(Config?.Formatter), Next);
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
                value = value?.ToLower();
                var oldLogger = logger;

                logger = null;

                switch (value)
                {
                    case "console":
                        logger = GetConsoleLogger();
                        break;
                    case "debug":
                        logger = GetDebugLogger();
                        break;
                    case "memory":
                        logger = GetMemoryLogger();
                        break;
                    case "file":
                        logger = GetFileLogger();
                        break;
                    case "sqlserver":
                        logger = GetSqlServerLogger();
                        break;
                    case "sql":
                        logger = GetSqlServerLogger();
                        break;
                    case "xml":
                        logger = GetXmlLogger();
                        break;
                    case "json":
                        logger = GetJsonLogger();
                        break;
                    case "csv":
                        logger = GetCsvLogger();
                        break;
                    case "null":
                        logger = new NullLogger();
                        break;
                    default:
                        if (Config.ThrowOnInvalidLoggers)
                        {
                            throw new Exception($"Logger '{type}' not supported");
                        }
                        break;
                }

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
