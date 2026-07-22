using Puya.Configuration;
using Puya.Data;
using Puya.Extensions;

namespace Puya.Net.Samples.Config
{
    public static partial class StartupConfig
    {
        public static void ConfigureDb(this IServiceCollection services)
        {
            services.Configure<DataStoreConfig>(Configuration.GetSection("DataStore"));
            services.AddSingleton<IConnectionStringProvider>(sp =>
            {
                var dss = Configuration.GetSection<DataStoreConfig>("DataStore");
                var cstr = dss.GetConnectionString("DefaultConnection");
                var result = new DefaultConnectionStringProvider();

                result.SetConnectionString(cstr);

                return result;
            });
            services.AddTransient<IDbContextInfoProvider, DefaultDbContextInfoProvider>();
            services.AddTransient<IDb>(sp =>
            {
                var constrProvider = sp.GetService<IConnectionStringProvider>();
                var dbCtxProvider = sp.GetService<IDbContextInfoProvider>();
                var result = new SqlServerDb(constrProvider, dbCtxProvider);

                return result;
            });
        }
    }
}
