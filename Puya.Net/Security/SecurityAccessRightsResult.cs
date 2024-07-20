using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Security
{
    public class SecurityAccessRightsResult
    {
        public Dictionary<string, bool> Form { get; set; }
        public Dictionary<string, bool> Catalog { get; set; }
        public Dictionary<string, bool> Operations { get; set; }
        public SecurityAccessRightsResult()
        {
            Form = new Dictionary<string, bool>();
            Catalog = new Dictionary<string, bool>();
            Operations = new Dictionary<string, bool>();
        }
    }
}
