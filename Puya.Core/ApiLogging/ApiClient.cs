using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ApiLogging
{
    public class ApiClient
    {
        public string Type { get; set; }
        public string App { get; set; }
        public object Credentials { get; set; }
    }
}
