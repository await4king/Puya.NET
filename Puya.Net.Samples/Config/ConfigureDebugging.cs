using Puya.Conversion;
using Puya.Core.Debugging;
using Puya.Debugging;
using Puya.Debugging.AspNetCore;

namespace Puya.Net.Samples.Config
{
    public static partial class StartupConfig
    {
        public static void ConfigureDebugging(this IServiceCollection services)
        {
            services.AddScoped<IDebugger>(sp =>
            {
                var httpcontextAccessor = sp.GetService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();

                return new HttpDebugger(httpcontextAccessor, new DebuggerOptions
                {
                    DebuggingEnabled = SafeClrConvert.ToBoolean(Configuration["Debugging"]),
                    GlobalDebugging = SafeClrConvert.ToBoolean(Configuration["GlobalDebugging"])
                });
            });
        }
    }
}
