using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public class DynamicLoggerConfig : BaseLoggerConfig
    {
        #region ctor
        public DynamicLoggerConfig() : this(null, null)
        { }
        public DynamicLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public DynamicLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider) : base(formatter, logConfigProvider)
        {
            if (formatter == null)
            {
                Formatter = new StringLogFormatter();
            }
        }
        public bool ThrowOnInvalidLoggers { get; set; }
        #endregion
    }
    public class DynamicLogger : BaseLogger<DynamicLoggerConfig>
    {
        public DynamicLogger() : this(null, null)
        { }
        public DynamicLogger(ILogger next) : this(null, next)
        { }
        public DynamicLogger(DynamicLoggerConfig config) : this(config, null)
        { }
        public DynamicLogger(DynamicLoggerConfig config, ILogger next) : base(config, next)
        { }
        public IBaseLoggerConfig LoggerConfig
        {
            get
            {
                var baseLogger = logger as BaseLogger;

                if (baseLogger != null)
                    return baseLogger.Config;

                return null;
            }
            set
            {
                var baseLogger = logger as BaseLogger;

                if (baseLogger != null)
                    baseLogger.Config = value;
            }
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
            return new ConsoleLogger(new ConsoleLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetDebugLogger()
        {
            return new DebugLogger(new DebugLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetMemoryLogger()
        {
            return new MemoryLogger(new MemoryLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetFileLogger()
        {
            return new FileLogger(new FileLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetSqlServerLogger()
        {
            return new SqlServerLogger(new SqlServerLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetXmlLogger()
        {
            return new XmlFileLogger(new XmlFileLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetJsonLogger()
        {
            return new JsonFileLogger(new JsonFileLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
        }
        protected virtual ILogger GetCsvLogger()
        {
            return new CsvFileLogger(new CsvFileLoggerConfig(Config.Formatter, Config.ConfigProvider), Next);
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
                        if (StrongConfig.ThrowOnInvalidLoggers)
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
