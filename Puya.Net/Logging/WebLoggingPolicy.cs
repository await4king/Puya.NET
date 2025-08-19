using Microsoft.AspNetCore.Http;
using Puya.Extensions;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UAParser;

namespace Puya.Logging
{
    public class WebLoggingPolicy : OverridableLogLevelPolicy
    {
        public WebLoggingPolicy(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }
        protected override string GetOverridedLogLevel()
        {
            string result = null;

            if (HttpContextAccessor.TryGetHttpContext(out HttpContext context))
            {
                result = context.Request.Headers[WebLoggingConstants.LogLevelHeaderName];
            }

            return result;
        }
        public async override Task InitAsync(ILogger logger, Log log, CancellationToken cancellation)
        {
            var context = HttpContextAccessor.HttpContext;

            if (context != null && log != null)
            {
                if (string.IsNullOrEmpty(log.User))
                {
                    log.User = context.User?.Identity?.Name;
                }

                if (context.Request != null)
                {
                    log.Method = context.Request.Method;
                    log.ContentType = context.Request.ContentType;

                    if (context.Request.Headers != null)
                    {
                        log.Referrer = context.Request.Headers["Referer"];
                    }

                    if (string.Compare(log.Method, "POST", StringComparison.OrdinalIgnoreCase) == 0 &&
                        context.Request.HasFormContentType &&
                        Options.Form)
                    {
                        log.Form = context.Request.Form.Join(Options.FormIncludeKeys, Options.FormExcludeKeys);
                    }

                    if (Options.Headers)
                    {
                        log.Headers = context.Request.Headers.Join(Options.HeadersIncludeKeys, Options.HeadersExcludeKeys);
                    }

                    try
                    {
                        log.Url = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(context.Request);
                    }
                    catch
                    { }

                    if (Options.Cookies)
                    {
                        log.Cookies = context.Request.Cookies.Join(Options.CookiesIncludeKeys, Options.CookiesExcludeKeys);
                    }

                    if (context.Request.Headers != null)
                    {
                        try
                        {
                            var uaParser = Parser.GetDefault();
                            var ci = uaParser.Parse(context.Request.Headers["User-Agent"]);

                            if (ci.UA != null)
                            {
                                log.BrowserName = ci.UA.Family;
                                log.BrowserVersion = ci.UA.Major + "." + ci.UA.Minor + "." + ci.UA.Patch;
                            }
                        }
                        catch
                        { }
                    }

                    if (Options.Body)
                    {
                        try
                        {
                            using (var reader = new StreamReader(context.Request.Body,
                                                            encoding: Encoding.UTF8,
                                                            detectEncodingFromByteOrderMarks: false,
                                                            bufferSize: 10240,
                                                            leaveOpen: true))
                            {
                                log.Body = await reader.ReadToEndAsync();
                            }
                        }
                        catch
                        { }
                    }
                }
            }
        }
    }
}
