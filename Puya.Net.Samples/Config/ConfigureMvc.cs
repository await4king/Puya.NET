namespace Puya.Net.Samples.Config
{
    public static partial class StartupConfig
    {
        public static void ConfigureMvc(this IServiceCollection services)
        {
            services.AddControllersWithViews();
        }
        public static void MapApiAndControllers(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "ApiEngine",
                pattern: "/api",
                defaults: new { controller = "ApiEngine", action = "Root" });

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
