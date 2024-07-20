using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Security.Models
{
    public class SecurityLocation
    {
        public int Id { get; set; }
        public string System { get; set; }
        public string SubSystem { get; set; }
        public string Form { get; set; }
        public string Section { get; set; }
    }
}
