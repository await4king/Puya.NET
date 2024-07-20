using System;
using System.Runtime.CompilerServices;

namespace Puya.Service
{
    public class LogProviderBase : ILogProvider
    {
        public LogList Logs { get; set; }
        int Depth;

        public LogProviderBase(): this(new LogProviderOptions())
        { }
        public LogProviderBase(LogProviderOptions options)
        {
            Depth = 1;
            Logs = new LogList();
            this.options = options;
        }
        public void EnterScope()
        {
            Depth++;
        }
        public void ExitScope()
        {
            Depth--;
        }
        LogProviderOptions options;

        public virtual LogProviderOptions Options
        {
            get { return options ?? new LogProviderOptions(); }
            set { options = value; }
        }
        public bool CanLog(string level)
        {
            return Options.Includes(level) && !Options.Excludes(level);
        }
        public void Info(string category,
                        string message,
                        object data,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (CanLog("info"))
            {
                Logs.Add(new Log
                {
                    Source = source,
                    Type = LogType.Info,
                    Category = category,
                    Message = message,
                    Data = data,
                    CallSite = memberName,
                    FilePath = sourceFilePath,
                    Line = sourceLineNumber,
                    Depth = Depth
                });
            }
        }
        public void Debug(string category,
                        string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (CanLog("debug"))
            {
                Logs.Add(new Log
                {
                    Source = source,
                    Type = LogType.Debug,
                    Category = category,
                    Message = message,
                    Data = data,
                    CallSite = memberName,
                    FilePath = sourceFilePath,
                    Line = sourceLineNumber,
                    Depth = Depth
                });
            }
        }
        public void Warn(string category,
                        string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (CanLog("warn"))
            {
                Logs.Add(new Log
                {
                    Source = source,
                    Type = LogType.Warning,
                    Category = category,
                    Message = message,
                    Data = data,
                    CallSite = memberName,
                    FilePath = sourceFilePath,
                    Line = sourceLineNumber,
                    Depth = Depth
                });
            }
        }
        public void Message(string category,
                        string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (CanLog("message"))
            {
                Logs.Add(new Log
                {
                    Source = source,
                    Type = LogType.Message,
                    Category = category,
                    Message = message,
                    Data = data,
                    CallSite = memberName,
                    FilePath = sourceFilePath,
                    Line = sourceLineNumber,
                    Depth = Depth
                });
            }
        }
        public void Trace(string category,
                        string message,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (CanLog("trace"))
            {
                Logs.Add(new Log
                {
                    Source = source,
                    Type = LogType.Trace,
                    Category = category,
                    Message = message,
                    Data = data,
                    CallSite = memberName,
                    FilePath = sourceFilePath,
                    Line = sourceLineNumber,
                    Depth = Depth
                });
            }
        }
        public void Error(string category,
                        string message,
                        Exception e,
                        object data = null,
                        LogSource source = LogSource.App,
                        [CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (CanLog("error"))
            {
                Logs.Add(new Log
                {
                    Source = source,
                    Type = LogType.Info,
                    Category = category,
                    Message = message,
                    Data = data,
                    Exception = e,
                    CallSite = memberName,
                    FilePath = sourceFilePath,
                    Line = sourceLineNumber,
                    Depth = Depth
                });
            }
        }
    }
}
