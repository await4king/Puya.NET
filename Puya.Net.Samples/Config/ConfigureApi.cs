using Puya.Api;
using Puya.Base;
using Puya.Caching;
using Puya.Conversion;
using Puya.Cryptography.v2;
using Puya.Data;
using Puya.Debugging;
using Puya.Service;
using Puya.Settings;
using Puya.Text;

namespace Puya.Net.Samples.Config
{
    public static partial class StartupConfig
    {
        public static void ConfigureApi(this IServiceCollection services)
        {
            services.AddSingleton<IBase64Encryption, DotNetBase64Encryption>();
            services.AddSingleton<IAesEncryption, AesEncryption>();
            services.AddSingleton<IEncodingUtility, EncodingUtility>();
            services.AddSingleton<IApiCryptor, ApiAesCryptor>();
            services.AddSingleton<IApiResponseSerializer, NewtonsoftJsonResponseSerializer>();
            services.AddScoped<IApiManager>(sp =>
            {
                var db = sp.GetService<IDb>();
                var configuration = sp.GetService<IConfiguration>();
                var logger = sp.GetService<Logging.ILogger>();
                var cache = sp.GetService<ICache>();
                var settings = sp.GetService<ISettingService>();
                var logprovider = sp.GetService<ILogProvider>();
                var debugger = sp.GetService<IDebugger>();
                var result = new SqlServerApiManager(db, logger, cache, settings, logprovider, debugger);

                result.CacheDuration = SafeClrConvert.ToInt(configuration["CacheDuration"], 300) / 60;

                return result;
            });
            services.AddSingleton<IMiddlewaresStore, MiddlewaresStore>();
            services.AddScoped<IApiEngine>(sp =>
            {
                var apiManager = sp.GetService<IApiManager>();
                var apiCryptor = sp.GetService<IApiCryptor>();
                var apiSerializer = sp.GetService<IApiResponseSerializer>();
                var logger = sp.GetService<Logging.ILogger>();
                var debugger = sp.GetService<IDebugger>();
                var defaultApp = Configuration["DefaultApp"];
                var middlewareStore = sp.GetService<IMiddlewaresStore>();

                var result = new ApiEngineDefault(sp, apiManager, apiCryptor, apiSerializer, debugger, logger, middlewareStore);

                result.DefaultApp = string.IsNullOrEmpty(defaultApp) ? "/" : defaultApp;

                return result;
            });

            AssemblyLoader.Load("Puya.Net.Samples");
        }
    }
}
