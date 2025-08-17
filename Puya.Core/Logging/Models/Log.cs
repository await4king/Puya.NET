using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading;
using System.Xml.Serialization;

namespace Puya.Logging
{
    public enum LogType : byte
    {
        [XmlEnum("Info")]
        Info = 1,
        [XmlEnum("Warning")]
        Warning = 2,
        [XmlEnum("Alert")]
        Alert = 4,
        [XmlEnum("Debug")]
        Debug = 8,
        [XmlEnum("Error")]
        Error = 16,
        [XmlEnum("Trace")]
        Trace = 32,
        [XmlEnum("Suggestion")]
        Suggestion = 64
    }
    public enum OperationResult : byte
    {
        [XmlEnum("Normal")]
        Normal = 0,
        [XmlEnum("Success")]
        Success = 1,
        [XmlEnum("Cancel")]
        Cancel = 2,
        [XmlEnum("Fatal")]
        Fatal = 3,
        [XmlEnum("Danger")]
        Danger = 4,
        [XmlEnum("Fault")]
        Fault = 5,
        [XmlEnum("Failure")]
        Failure = 6,
        [XmlEnum("Error")]
        Error = 7,
        [XmlEnum("Abort")]
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
        [JsonIgnore]
        [XmlIgnore]
        public byte Type { get; set; }
        [JsonIgnore]
        [XmlIgnore]
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
        [JsonIgnore]
        [XmlIgnore]
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
        [JsonIgnore]
        [XmlIgnore]
        public Browser Browser { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
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
        [JsonConverter(typeof(StringEnumConverter))]
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
        public string Fill(string template)
        {
            var result = template
                .Replace("{id}", Id > 0 ? Id.ToString(): "")
                .Replace("{appid}", AppId?.ToString())
                .Replace("{threadid}", ThreadId?.ToString())
                .Replace("{type}", Type.ToString())
                .Replace("{result}", Result.ToString())
                .Replace("{category}", Category?.ToString())
                .Replace("{file}", File?.ToString())
                .Replace("{line}", Line?.ToString())
                .Replace("{membername}", MemberName?.ToString())
                .Replace("{message}", Message?.ToString())
                .Replace("{stacktrace}", StackTrace?.ToString())
                .Replace("{ip}", Ip?.ToString())
                .Replace("{user}", User?.ToString())
                .Replace("{logdate}", this.FormatDate())
                .Replace("{data}", this.SerializeData())
                .Replace("{method}", Method?.ToString())
                .Replace("{url}", Url?.ToString())
                .Replace("{browsername}", BrowserName?.ToString())
                .Replace("{browserversion}", BrowserVersion?.ToString())
                .Replace("{referrer}", Referrer?.ToString())
                .Replace("{headers}", Headers?.ToString())
                .Replace("{form}", Form?.ToString())
                .Replace("{cookies}", Cookies?.ToString())
                .Replace("{body}", Body?.ToString())
                .Replace("{contenttype}", ContentType?.ToString())
                .Replace("{operationresult}", OperationResult.ToString())
                .Replace("{logtype}", LogType.ToString());

            return result;
        }
        public string Clear(string template)
        {
            var result = template
                .Replace("{id}", "")
                .Replace("{appid}", "")
                .Replace("{threadid}", ThreadId?.ToString())
                .Replace("{type}", Type.ToString())
                .Replace("{result}", Result.ToString())
                .Replace("{category}", "")
                .Replace("{file}", "")
                .Replace("{line}", "")
                .Replace("{membername}", "")
                .Replace("{message}", "")
                .Replace("{stacktrace}", "")
                .Replace("{ip}", "")
                .Replace("{user}", "")
                .Replace("{logdate}", this.FormatDate())
                .Replace("{data}", "")
                .Replace("{method}", "")
                .Replace("{url}", "")
                .Replace("{browsername}", "")
                .Replace("{browserversion}", "")
                .Replace("{referrer}", "")
                .Replace("{headers}", "")
                .Replace("{form}", "")
                .Replace("{cookies}", "")
                .Replace("{body}", "")
                .Replace("{contenttype}", "")
                .Replace("{operationresult}", OperationResult.Normal.ToString())
                .Replace("{logtype}", LogType.Info.ToString());

            return result;
        }
    }
}
