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
    public class WebLoggingPolicy : ILoggingPolicy
    {
        public WebLoggingPolicy(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }
        protected virtual LogLevel? GetOverridedLogLevel()
        {
            LogLevel? result = null;

            var context = HttpContextAccessor?.HttpContext;

            if (!(context == null || context.Request == null || context.Request.Headers == null))
            {
                string level = context.Request.Headers[WebLoggingConstants.LogLevelHeaderName];

                if (!string.IsNullOrEmpty(level))
                {
                    var _level = level.ToEnum<LogLevel>(LogLevel.None);

                    if (_level != LogLevel.None)
                    {
                        result = _level;
                    }
                }
            }

            return result;
        }
        LoggingPolicyOptions _options;
        public LoggingPolicyOptions Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new LoggingPolicyOptions();
                }

                return _options;
            }
            set
            {
                _options = value;
            }
        }
        public virtual bool CanLog(ILogger logger, Log log)
        {
            var _logger = logger as IBaseLogger;

            if (_logger != null)
            {
                var logLevel = GetOverridedLogLevel();

                if (logLevel.HasValue && logLevel.Value != LogLevel.None)
                {
                    return _logger.Config.Level == logLevel.Value;
                }
            }

            return true;
        }

        public async Task InitAsync(ILogger logger, Log log, CancellationToken cancellation)
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

                            log.BrowserName = ci.UA.Family;
                            log.BrowserVersion = ci.UA.Major + "." + ci.UA.Minor + "." + ci.UA.Patch;
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
