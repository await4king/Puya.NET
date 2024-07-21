using System;
using System.Runtime.CompilerServices;

namespace Puya.Service
{
    public interface ILogProvider
    {
        void EnterScope();
        void ExitScope();
        LogList Logs { get; set; }
        void Debug(string category, string message, object data = null, LogSource source = LogSource.App, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Error(string category, string message, Exception e, object data = null, LogSource source = LogSource.App, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Info(string category, string message, object data, LogSource source = LogSource.App, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Message(string category, string message, object data = null, LogSource source = LogSource.App, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Trace(string category, string message, object data = null, LogSource source = LogSource.App, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
        void Warn(string category, string message, object data = null, LogSource source = LogSource.App, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0);
    }
}