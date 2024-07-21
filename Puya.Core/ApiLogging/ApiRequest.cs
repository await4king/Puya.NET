using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ApiLogging
{
    public class ApiRequest
    {
        public string Method { get; set; }
        public object Data { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public IDictionary <string, string> Cookies { get; set; }
    }
}
