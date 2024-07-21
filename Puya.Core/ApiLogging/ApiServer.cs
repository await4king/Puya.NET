using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.ApiLogging
{
    public class ApiServer
    {
        public string EndPoint { get; set; }
        public string Service { get; set; }
        public object Credentials { get; set; }
    }
}
