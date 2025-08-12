using System;
using System.Threading;

namespace Puya.Logging
{
    public enum LogType : byte
    {
        Info = 1,
        Warning = 2,
        Alert = 4,
        Debug = 8,
        Error = 16,
        Trace = 32,
        Suggestion = 64
    }
    public enum OperationResult : byte
    {
        Normal = 0,
        Success = 1,
        Cancel = 2,
        Fatal = 3,
        Danger = 4,
        Fault = 5,
        Failure = 6,
        Error = 7,
        Abort = 8
    }
    public enum LogLevel : byte
    {
        None = 0,
        Info = 71,          // Info (1) + Warning (2) + Alert (4) + Suggestion (64)
        Debug = 40,         // Debug (8) + Trace (32)
        Error = 16,         // Error (16)
        InfoError = 87,     // InfoLevel (71) + ErrorLevel (16)
        InfoDebug = 111,    // InfoLevel (71) + DebugLevel (40)
        DebugError = 56,    // DebugLevel (40) + ErrorLevel (16)
        All = 127
    }
    public class Log
    {
        public int Id { get; set; }
        /// <summary>
        /// AppId is used to separate logs of different applications that are using the same database and the same logging table.
        /// For example suppose we have a single database that is used by our web app, mobile app, api app and desktop apps.
        /// For each application we specify a unique AppId. This way we can filter logs based on AppId to see the logs of a
        /// specific application.
        /// </summary>
        public int? AppId { get; set; }
        public int? ThreadId { get; set; }
        public byte Type { get; set; }
        public byte Result { get; set; }
        public string Category { get; set; }
        public string File { get; set; }
        public int? Line { get; set; }
        public string MemberName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Ip { get; set; }
        public string User { get; set; }
        public DateTime LogDate { get; set; }
        public Func<object> GetData { get; set; }
        public object Data { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string Referrer { get; set; }
        public string Headers { get; set; }
        public string Form { get; set; }
        public string Cookies { get; set; }
        public string Body { get; set; }
        public string ContentType { get; set; }
        public Browser Browser { get; set; }
        public OperationResult OperationResult
        {
            get
            {
                return (OperationResult)this.Result;
            }
            set
            {
                this.Result = (byte)value;
            }
        }
        public LogType LogType
        {
            get
            {
                return (LogType)this.Type;
            }
            set
            {
                this.Type = (byte)value;
            }
        }
        public Log()
        {
            LogDate = DateTime.Now;
            OperationResult = OperationResult.Normal;
            LogType = LogType.Info;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
        }
        public virtual Log Clone()
        {
            return new Log
            {
                AppId = AppId,
                ThreadId = ThreadId,
                LogDate = LogDate,
                Category = Category,
                Data = Data,
                File = File,
                GetData = GetData,
                Id = Id,
                Ip = Ip,
                Line = Line,
                MemberName = MemberName,
                Message = Message,
                Result = Result,
                StackTrace = StackTrace,
                Type = Type,
                User = User,
                Method = Method,
                Url = Url,
                BrowserName = BrowserName,
                BrowserVersion = BrowserVersion,
                Referrer = Referrer,
                Headers = Headers,
                Form = Form,
                Body = Body,
                ContentType = ContentType,
                Cookies = Cookies,
                Browser = Browser,
            };
        }
        public Log(Log log)
        {
            if (log != null)
            {
                Id = log.Id;
                AppId = log.AppId;
                ThreadId = log.ThreadId;
                Type = log.Type;
                Result = log.Result;
                Category = log.Category;
                File = log.File;
                Line = log.Line;
                MemberName = log.MemberName;
                Message = log.Message;
                StackTrace = log.StackTrace;
                Ip = log.Ip;
                User = log.User;
                LogDate = log.LogDate;
                Data = log.Data;
                Method = log.Method;
                Url = log.Url;
                BrowserName = log.BrowserName;
                BrowserVersion = log.BrowserVersion;
                Referrer = log.Referrer;
                Headers = log.Headers;
                Form = log.Form;
                Cookies = log.Cookies;
                Browser = log.Browser;
                Body = log.Body;
                ContentType = log.ContentType;
            }
        }
    }
}
