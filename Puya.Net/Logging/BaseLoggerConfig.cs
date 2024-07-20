using Puya.Base;
using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging
{
    public interface IBaseLoggerConfig
    {
        int? AppId { get; set; }
        string User { get; set; }
        LogLevel Level { get; set; }
        ILogFormatter Formatter { get; set; }
        ILogConfigProvider ConfigProvider { get; set; }
    }
    public class BaseLoggerConfig: IBaseLoggerConfig
    {
        public int? AppId { get; set; }
        public string user;
        public string User
        {
            get
            {
                if (string.IsNullOrEmpty(user))
                {
                    user = ConfigProvider.GetUser();
                }

                return user;
            }
            set { user = value; }
        }
        private LogLevel? level;
        public LogLevel Level
        {
            get
            {
                if (level == null)
                {
                    level = ConfigProvider.GetLogLevel();
                }

                return level.Value;
            }
            set { level = value; }
        }
        private ILogConfigProvider _logConfigProvider;
        public ILogConfigProvider ConfigProvider
        {
            get
            {
                return TypeHelper.EnsureInitialized<ILogConfigProvider, NullLogConfigProvider>(ref _logConfigProvider);
            }
            set { _logConfigProvider = value; }
        }
        private ILogFormatter _formatter;
        public ILogFormatter Formatter
        {
            get
            {
                return TypeHelper.EnsureInitialized<ILogFormatter, JsonLogFormatter>(ref _formatter);
            }
            set { _formatter = value; }
        }
        public BaseLoggerConfig(): this(null, null)
        { }
        public BaseLoggerConfig(ILogFormatter formatter) : this(formatter, null)
        { }
        public BaseLoggerConfig(ILogFormatter formatter, ILogConfigProvider logConfigProvider)
        {
            Level = LogLevel.All;
            _logConfigProvider = logConfigProvider;
            _formatter = formatter;

            if (_formatter == null)
            {
                _formatter = GetDefaultFormatter();
            }
        }
        protected virtual ILogFormatter GetDefaultFormatter()
        {
            return null;
        }
    }
}
