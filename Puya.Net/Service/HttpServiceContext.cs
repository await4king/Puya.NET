using Microsoft.AspNetCore.Http;
using Puya.Core.ServiceModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Puya.Service
{
    public class HttpServiceContext : IServiceContext
    {
        public HttpServiceContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User => HttpContextAccessor.HttpContext?.User;

        public IHttpContextAccessor HttpContextAccessor { get; set; }
    }
}