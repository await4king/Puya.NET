using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puya.Logging.Models
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
        Info = 71,  // LogTypes: 1: Info, 2: Warning, 4: Alert, 64: Suggestion
        Debug = 40, // LogType: 8: Debug, 32: Trace
        Error = 16, // LogType: 32: Error
        InfoError = 87, // Info (71) + Error (16)
        InfoDebug = 111,    // Info (71) + Debug (40)
        DebugError = 56,    // Debug (16) + Error (16)
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
        public string Data { get; set; }
        public Func<ILogger, object> GetData { get; set; }
        public object DataObject { get; set; }
        public dynamic StrongDataObject { get; set; }
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
    }
}
