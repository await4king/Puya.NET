using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ApiLogging
{
    public enum ApiCallDirection
    {
        Incoming,   // 0
        Outgoing    // 1
    }
    public class ApiLog
    {
        public int Id { get; set; }
        public ApiCallDirection Direction { get; set; }
        public DateTime LogDate { get; set; }
        public ApiClient Client { get; set; }
        public ApiServer Server { get; set; }
        public ApiRequest Request { get; set; }
        public ApiResponse Response { get; set; }
    }
}
