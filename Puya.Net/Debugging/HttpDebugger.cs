using Microsoft.AspNetCore.Http;
using Puya.Core.Debugging;

namespace Puya.Debugging.AspNetCore
{
    public class HttpDebugger : BaseDebugger
    {
        public static string IsDebuggingHeaderName { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public HttpDebugger(IHttpContextAccessor httpContextAccessor) : base(new DebuggerOptions())
        {
            HttpContextAccessor = httpContextAccessor;
        }
        public HttpDebugger(IHttpContextAccessor httpContextAccessor, DebuggerOptions options) : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
        }
        static HttpDebugger()
        {
            IsDebuggingHeaderName = "x-debug";
        }
        protected override bool? GetIsDebugging()
        {
            var isDebugging = string.IsNullOrEmpty(IsDebuggingHeaderName) ? string.Empty : HttpContextAccessor?.HttpContext?.Request?.Headers[IsDebuggingHeaderName].ToString();

            return isDebugging == "1" || isDebugging == "true";
        }
        protected override string GetUserName()
        {
            return HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }
        protected override bool IsInRole(string roleName)
        {
            return HttpContextAccessor?.HttpContext?.User?.IsInRole(Options.DebuggerRoleName) ?? false;
        }
    }
}
