using System;

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
    public enum OperationResult: byte
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
        }
        public virtual Log Clone()
        {
            return new Log
            {
                AppId = AppId,
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
                User = User
            };
        }
    }
}
