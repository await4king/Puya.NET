using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Puya.Configuration
{
    public static class Extensions
    {
        public static string GetDataStoreConnectionString(this IConfiguration config, string name)
        {
            var dsConfig = config.GetSection("DataStore").Get<DataStoreConfig>();
            var result = dsConfig.GetConnectionString(name);

            return result;
        }
        public static AuthenticationBuilder Configure(this AuthenticationBuilder authenticationBuilder, AuthenticationConfig config)
        {
            if (config != null)
            {
                var first = config.FirstOrDefault(x => x.IsActive);

                if (first != null)
                {
                    Action<CookieAuthenticationOptions> optionBuilder = options =>
                    {
                        if (!string.IsNullOrEmpty(first.AccessDeniedPath))
                        {
                            options.AccessDeniedPath = first.AccessDeniedPath;
                        }
                        if (first.ExpireTimeSpan != null)
                        {
                            options.ExpireTimeSpan = first.ExpireTimeSpan.Value;
                        }
                        if (!string.IsNullOrEmpty(first.LoginPath))
                        {
                            options.LoginPath = first.LoginPath;
                        }
                        if (!string.IsNullOrEmpty(first.LogoutPath))
                        {
                            options.LogoutPath = first.LogoutPath;
                        }
                        if (!string.IsNullOrEmpty(first.ReturnUrlParameter))
                        {
                            options.ReturnUrlParameter = first.ReturnUrlParameter;
                        }
                        if (first.SlidingExpiration != null)
                        {
                            options.SlidingExpiration = first.SlidingExpiration.Value;
                        }

                        if (first.Cookie != null)
                        {
                            if (!string.IsNullOrEmpty(first.Cookie.Domain))
                            {
                                options.Cookie.Domain = first.Cookie.Domain;
                            }
                            if (first.Cookie.Expiration != null)
                            {
                                options.Cookie.Expiration = first.Cookie.Expiration;
                            }
                            if (first.Cookie.HttpOnly != null)
                            {
                                options.Cookie.HttpOnly = first.Cookie.HttpOnly.Value;
                            }
                            if (first.Cookie.IsEssential != null)
                            {
                                options.Cookie.IsEssential = first.Cookie.IsEssential.Value;
                            }
                            if (first.Cookie.MaxAge != null)
                            {
                                options.Cookie.MaxAge = first.Cookie.MaxAge.Value;
                            }
                            if (!string.IsNullOrEmpty(first.Cookie.Name))
                            {
                                options.Cookie.Name = first.Cookie.Name;
                            }
                            if (!string.IsNullOrEmpty(first.Cookie.Path))
                            {
                                options.Cookie.Path = first.Cookie.Path;
                            }

                            if (first.Cookie.StrongSameSite != null)
                            {
                                options.Cookie.SameSite = first.Cookie.StrongSameSite.Value;
                            }

                            if (first.Cookie.StrongSecurePolicy != null)
                            {
                                options.Cookie.SecurePolicy = first.Cookie.StrongSecurePolicy.Value;
                            }
                        }
                    };

                    if (!string.IsNullOrEmpty(first.AuthenticationScheme))
                    {
                        authenticationBuilder.AddCookie(first.AuthenticationScheme, optionBuilder);
                    }
                    else
                    {
                        authenticationBuilder.AddCookie(optionBuilder);
                    }
                }
            }
            else
            {
                authenticationBuilder.AddCookie();
            }

            return authenticationBuilder;
        }
        public static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var authConfig = configuration.GetSection("Authentication");
            services.Configure<AuthenticationConfig>(authConfig);
            var authCfg = configuration.GetSection("Authentication").Get<AuthenticationConfig>();

            var first = authCfg?.FirstOrDefault(x => x.IsActive);

            var dataProtectionDirValue = first?.DataProtectionDir;

            if (!string.IsNullOrEmpty(dataProtectionDirValue))
            {
                var dataProtectionDir = new DirectoryInfo(dataProtectionDirValue);
                var dpBuilder = services.AddDataProtection().PersistKeysToFileSystem(dataProtectionDir);

                if (first != null && !string.IsNullOrEmpty(first.ApplicationName))
                {
                    dpBuilder.SetApplicationName(first.ApplicationName);
                }
            }

            var authScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            if (first != null && !string.IsNullOrEmpty(first.AuthenticationScheme))
            {
                authScheme = first.AuthenticationScheme;
            }

            var result = services.AddAuthentication(authScheme).Configure(authCfg);

            return result;
        }
    }
}
