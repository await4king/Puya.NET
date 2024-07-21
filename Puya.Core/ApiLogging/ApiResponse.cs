using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ApiLogging
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string StatusDesc { get; set; }
        public string Body { get; set; }
        public byte[] RawBody { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary<string, string> Cookies { get; set; }
    }
}
