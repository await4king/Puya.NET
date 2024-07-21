using System;
using System.Collections.Generic;
using System.Text;
using Puya.Logging.Models;

namespace Puya.Logging
{
    public interface ILogConfigProvider
    {
        string GetUser();
        LogLevel GetLogLevel();
    }
    public class NullLogConfigProvider : ILogConfigProvider
    {
        public string GetUser()
        {
            return null;
        }
        public LogLevel GetLogLevel()
        {
            return LogLevel.None;
        }
    }
}
