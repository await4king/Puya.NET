using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Puya.Api;
using Puya.Caching;
using Puya.Conversion;
using Puya.Core.ServiceModel;
using Puya.Data;
using Puya.Localization;
using Puya.Service;
using Puya.ServiceModel;
using Puya.Settings;
using Puya.Translation;

namespace Puya.Net.Samples.Config
{
    public class AppPath
    {
        public static string Root
        {
            get
            {
                var result = AppDomain.CurrentDomain.BaseDirectory;

                if (result.EndsWith("\\debug", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = result.Substring(0, result.Length - 6);
                }
                if (result.EndsWith("\\release", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = result.Substring(0, result.Length - 8);
                }
                if (result.EndsWith("\\bin", StringComparison.CurrentCultureIgnoreCase))
                {
                    result = result.Substring(0, result.Length - 4);
                }

                return result;
            }
        }
    }
    public class ServiceAttributeInterceptorFactory : IServiceInterceptorFactory
    {
        private readonly IServiceProvider provider;

        public ServiceAttributeInterceptorFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public IServiceInterceptor GetInterceptor(object attribute)
        {
            var type = attribute?.GetType();
            var serviceContext = provider.GetService<IServiceContext>();

            if (type == typeof(PermissionAttribute))
            {
                return new ServicePermissionCheckInterceptor(serviceContext, attribute as PermissionAttribute);
            }

            return null;
        }
    }
    public static partial class StartupConfig
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ICacheManager>(sp =>
            {
                var cache = sp.GetService<IMemoryCache>();
                var result = new MemoryCacheManager(cache);

                return result;
            });
            services.AddSingleton<ICache>(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();

                var result = new Puya.Caching.MemoryCache();

                result.Duration = SafeClrConvert.ToInt(configuration["CacheDuration"], 300);

                return result;
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IServiceContext, HttpServiceContext>();
            services.AddScoped<IServiceInterceptorFactory, ServiceAttributeInterceptorFactory>();
            services.AddScoped<IServiceInterceptor>(sp =>
            {
                var factory = sp.GetService<IServiceInterceptorFactory>();
                var db = sp.GetService<IDb>();
                var sci = new ServiceAttributeInterceptor(factory);

                return sci;
            });
            services.AddScoped<ILanguageProvider>(sp => new FixedLanguageProvider("fa"));
            services.AddScoped<ITranslator>(sp =>
            {
                var cache = sp.GetService<ICache>();
                var logger = sp.GetService<Puya.Logging.ILogger>();
                var db = sp.GetService<IDb>();
                var langProvider = sp.GetService<ILanguageProvider>();
                var result = new HybridTranslator(cache, logger, db, langProvider);

                result.File.BasePath = AppPath.Root;

                return result;
            });
            services.AddScoped<ISettingService>(sp =>
            {
                var db = sp.GetService<IDb>();
                var logger = sp.GetService<Puya.Logging.ILogger>();
                var cache = sp.GetService<ICache>();
                var httpContextAccessor = sp.GetService<IHttpContextAccessor>();
                var httpContext = httpContextAccessor.HttpContext;

                var result = new DbSettingService(db, logger, cache);

                result.TableName = "AppSettings";

                return result;
            });
            services.AddTapServices("Puya.Samples");
        }
    }
}
