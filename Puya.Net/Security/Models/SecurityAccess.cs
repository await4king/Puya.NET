using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Security.Models
{
    public class SecurityAccess
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string SystemId { get; set; }
        public string PermissionClass { get; set; }
        public string DialogName { get; set; }
        public string Section { get; set; }
        public long Access { get; set; }
    }
}
