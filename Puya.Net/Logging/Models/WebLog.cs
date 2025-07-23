namespace Puya.Logging
{
    public class WebLog : Log
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string Referrer { get; set; }
        public string Headers { get; set; }
        public string Form { get; set; }
        public string Cookies { get; set; }
        public Browser Browser { get; set; }
        public WebLog()
        { }
        public WebLog(Log log)
        {
            Id = log.Id;
            AppId = log.AppId;
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

            var _log = log as WebLog;

            if (_log != null)
            {
                Method = _log.Method;
                Url = _log.Url;
                BrowserName = _log.BrowserName;
                BrowserVersion = _log.BrowserVersion;
                Referrer = _log.Referrer;
                Headers = _log.Headers;
                Form = _log.Form;
                Cookies = _log.Cookies;
                Browser = _log.Browser;
            }
        }
        public override Log Clone()
        {
            var result = base.Clone();
            var _result = result as WebLog;

            if (_result != null)
            {
                _result.Method = Method;
                _result.Url = Url;
                _result.BrowserName = BrowserName;
                _result.BrowserVersion = BrowserVersion;
                _result.Referrer = Referrer;
                _result.Headers = Headers;
                _result.Form = Form;
                _result.Cookies = Cookies;
                _result.Browser = Browser;
            }

            return result;
        }
    }
}
