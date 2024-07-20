using Puya.Logging.Web.Abstractions;
using Puya.Logging.Web.Abstractions.Models;
using Puya.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAParser;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace Puya.Logging.WebCore
{
    public interface IWebSqlServerLoggerConfigCore: IWebDbLoggerConfig, IWebLoggerConfigCore
    { }
    public class WebSqlServerLoggerConfigCore : WebDbLoggerConfig, IWebSqlServerLoggerConfigCore
    {
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public WebSqlServerLoggerConfigCore() : this(null)
        { }
        public WebSqlServerLoggerConfigCore(ILogFormatter formatter): base(formatter)
        { }
        public override void Prepare(WebLog log)
        {
            var context = HttpContextAccessor.HttpContext;

            if (string.IsNullOrEmpty(log.User))
            {
                log.User = context.User.Identity.Name;
            }

            log.Method = context.Request.Method;
            log.Referrer = context.Request.Headers["Referer"];

            if (string.Compare(log.Method, "POST", StringComparison.OrdinalIgnoreCase) == 0 && context.Request.HasFormContentType)
            {
                try
                {
                    log.Form = context.Request.Form?.Join("\n");
                }
                catch
                {
                    if (ThrowContextErrors)
                    {
                        throw;
                    }
                }
            }

            try
            {
                log.Headers = context.Request.Headers?.Join("\n");
            }
            catch
            {
                if (ThrowContextErrors)
                {
                    throw;
                }
            }

            log.Url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);

            try
            {
                log.Cookies = context.Request.Cookies?.Join(";");
            }
            catch
            {
                if (ThrowContextErrors)
                {
                    throw;
                }
            }

            try
            {
                var uaParser = Parser.GetDefault();
                var ci = uaParser.Parse(context.Request.Headers["User-Agent"]);

                log.BrowserName = ci.UA.Family;
                log.BrowserVersion = ci.UA.Major + "." + ci.UA.Minor + "." + ci.UA.Patch;
            }
            catch
            {
                if (ThrowContextErrors)
                {
                    throw;
                }
            }
        }
    }
}
