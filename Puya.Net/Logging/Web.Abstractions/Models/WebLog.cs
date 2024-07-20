using Puya.Logging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging.Web.Abstractions.Models
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
            this.Id = log.Id;
            this.AppId = log.AppId;
            this.Type = log.Type;
            this.Result = log.Result;
            this.Category = log.Category;
            this.File = log.File;
            this.Line = log.Line;
            this.MemberName = log.MemberName;
            this.Message = log.Message;
            this.StackTrace = log.StackTrace;
            this.Ip = log.Ip;
            this.User = log.User;
            this.LogDate = log.LogDate;
            this.Data = log.Data;
            this.DataObject = log.DataObject;
            this.StrongDataObject = log.StrongDataObject;
        }
    }
}
