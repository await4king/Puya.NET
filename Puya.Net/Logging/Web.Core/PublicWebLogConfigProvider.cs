using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Puya.Extensions;
using Puya.Logging.Models;
using Puya.Logging.Web.Abstractions;

namespace Puya.Logging.WebCore
{
    public class PublicWebLogConfigProvider : ILogConfigProvider
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public PublicWebLogConfigProvider(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public virtual LogLevel GetLogLevel()
        {
            var context = HttpContextAccessor.HttpContext;
            string level = context.Request.Headers[WebLoggingConstants.LogLevelHeaderName];

            return level?.ToEnum<LogLevel>() ?? LogLevel.None;
        }
        public virtual string GetUser()
        {
            var context = HttpContextAccessor.HttpContext;

            return context.User.Identity.Name;
        }
    }
}
