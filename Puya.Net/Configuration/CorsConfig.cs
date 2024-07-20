using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Configuration
{
    public class CorsConfigItem
    {
        public string Name { get; set; }
        public string Origins { get; set; } // any, * to allow any origins; http://foo.com, http://bar.com
        public string Headers { get; set; }
        public string Methods { get; set; }
        public string ExposedHeaders { get; set; }
        public bool AllowCredentials { get; set; }
        public bool DisallowCredentials { get; set; }
        public int PreflightMaxAge { get; set; }
        public bool Disabled { get; set; }
    }
    public class CorsConfig
    {
        public bool Enabled { get; set; }
        public List<CorsConfigItem> Policies { get; set; }
    }
}
