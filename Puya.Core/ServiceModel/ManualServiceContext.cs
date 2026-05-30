using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Puya.Core.ServiceModel
{
    public class ManualServiceContext : IServiceContext
    {
        public ManualServiceContext(ClaimsPrincipal principal)
        {
            User = principal;
        }
        public ClaimsPrincipal User { get; set; }
    }
}