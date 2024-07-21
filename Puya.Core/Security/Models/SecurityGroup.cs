using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Security.Models
{
    public class SecurityGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAdmin { get; set; }
        public bool PuyaReport { get; set; }
    }
}
