using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Puya.Debugging.AspNetCore
{
    public class HttpDebuggerOptions
    {
        public bool DebuggingEnabled { get; set; }
        public bool IsGlobalDebugging { get; set; }
        public string DebuggerUsers { get; set; }
        public string DebuggerRoleName { get; set; }
        public HttpDebuggerOptions()
        {
            DebuggingEnabled = true;
            IsGlobalDebugging = false;
            DebuggerRoleName = "Debugger";
        }
    }
    public class HttpDebugger : IDebugger
    {
        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpDebuggerOptions Options { get; set; }

        public HttpDebugger(IHttpContextAccessor httpContextAccessor, HttpDebuggerOptions options)
        {
            HttpContextAccessor = httpContextAccessor;
            Options = options;
        }
        bool? isDebugger;
        public bool IsDebugging
        {
            get
            {
                if (isDebugger == null)
                {
                    var httpContext = HttpContextAccessor.HttpContext;

                    if (httpContext != null)
                    {
                        var isDebugging = HttpContextAccessor.HttpContext.Request.Headers["x-debug"].ToString();
                        var debuggers = Options.DebuggerUsers?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[] { };

                        isDebugger = (isDebugging == "1" || isDebugging == "true")
                                        &&
                                        Options.DebuggingEnabled
                                        &&
                                        (
                                            Options.IsGlobalDebugging
                                                ||
                                            HttpContextAccessor.HttpContext.User.IsInRole(Options.DebuggerRoleName)
                                                ||
                                            debuggers.Contains(HttpContextAccessor.HttpContext.User.Identity.Name, StringComparer.OrdinalIgnoreCase)
                                        );
                    }
                }

                return isDebugger.Value;
            }
        }

    }
}
