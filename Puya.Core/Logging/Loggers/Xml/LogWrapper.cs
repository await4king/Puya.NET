using Newtonsoft.Json;
using Puya.Extensions;
using System;
using Formatting = Newtonsoft.Json.Formatting;

namespace Puya.Logging
{
    public class LogWrapper
    {
        public int Id { get; set; }
        public int? AppId { get; set; }
        public int? ThreadId { get; set; }
        public string Category { get; set; }
        public string File { get; set; }
        public int? Line { get; set; }
        public string MemberName { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Ip { get; set; }
        public string User { get; set; }
        public DateTime LogDate { get; set; }
        public string Data { get; set; }
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
        public OperationResult OperationResult { get; set; }
        public LogType LogType { get; set; }
        public LogWrapper(Log log)
        {
            Id = log.Id;
            AppId = log.AppId;
            ThreadId = log.ThreadId;
            Category = log.Category;
            File = log.File;
            Line = log.Line;
            MemberName = log.MemberName;
            Message = log.Message;
            StackTrace = log.StackTrace;
            Ip = log.Ip;
            User = log.User;
            LogDate = log.LogDate;

            var data = log.GetData == null ? log.Data : log.GetData();

            Data = data == null ? null : JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Method = log.Method;
            Url = log.Url;
            BrowserName = log.BrowserName;
            BrowserVersion = log.BrowserVersion;
            Referrer = log.Referrer;
            Headers = log.Headers;
            Form = log.Form;
            Cookies = log.Cookies;
            Body = log.Body;
            ContentType = log.ContentType;
            OperationResult = log.OperationResult;
            LogType = log.LogType;
        }
        public LogWrapper()
        { }
        public Log ToLog()
        {
            var result = new Log();

            result.Id = Id;
            result.AppId = AppId;
            result.ThreadId = ThreadId;
            result.Category = Category;
            result.File = File;
            result.Line = Line;
            result.MemberName = MemberName;
            result.Message = Message;
            result.StackTrace = StackTrace;
            result.Ip = Ip;
            result.User = User;
            result.LogDate = LogDate;
            result.Data = string.IsNullOrEmpty(this.Data) ? null : this.Data.SafeDeserialize();
            result.Method = Method;
            result.Url = Url;
            result.BrowserName = BrowserName;
            result.BrowserVersion = BrowserVersion;
            result.Referrer = Referrer;
            result.Headers = Headers;
            result.Form = Form;
            result.Cookies = Cookies;
            result.Body = Body;
            result.ContentType = ContentType;
            result.OperationResult = OperationResult;
            result.LogType = LogType;

            return result;
        }
    }
}
