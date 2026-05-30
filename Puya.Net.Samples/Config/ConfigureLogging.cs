using Puya.ApiLogging;
using Puya.Data;
using Puya.Debugging;
using Puya.Debugging.AspNetCore;
using Puya.Extensions;
using Puya.Logging;
using Puya.Service;

namespace Puya.Net.Samples.Config
{
    public static partial class StartupConfig
    {
        public static void ConfigureLogging(this IServiceCollection services)
        {
            services.AddScoped<Puya.Logging.ILogger, Puya.Logging.DebugLogger>();
            services.AddScoped<ILogProvider>(sp =>
            {
                var httpAccessor = sp.GetService<IHttpContextAccessor>();
                var options = httpAccessor.HttpContext == null ? new LogProviderOptions() : httpAccessor.HttpContext.Request?.Headers["x-log-level"].ToString().SafeDeserialize(new LogProviderOptions());

                return new LogProviderBase(options);
            });
            
        }
    }
}
