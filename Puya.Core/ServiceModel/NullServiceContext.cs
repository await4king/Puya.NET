using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Puya.Core.ServiceModel
{
    public class NullServiceContext : IServiceContext
    {
        public ClaimsPrincipal User => null;
    }
}