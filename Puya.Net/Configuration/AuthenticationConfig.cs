using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Configuration
{
    public class CookieConfig
    {
        public string Domain { get; set; }
        public TimeSpan? Expiration { get; set; }
        public bool? HttpOnly { get; set; }
        public bool? IsEssential { get; set; }
        public TimeSpan? MaxAge { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string SameSite { get; set; }
        public SameSiteMode? StrongSameSite
        {
            get
            {
                if (!string.IsNullOrEmpty(SameSite))
                {
                    int intSameSite;

                    if (Int32.TryParse(SameSite, out intSameSite))
                    {
                        return (SameSiteMode)intSameSite;
                    }
                    else
                    {
                        SameSiteMode sameSiteMode;

                        if (Enum.TryParse(SameSite, out sameSiteMode))
                        {
                            return sameSiteMode;
                        }
                    }
                }

                return null;
            }
        }
        public string SecurePolicy { get; set; }
        public CookieSecurePolicy? StrongSecurePolicy
        {
            get
            {
                if (!string.IsNullOrEmpty(SecurePolicy))
                {
                    int intSecurePolicy;

                    if (Int32.TryParse(SecurePolicy, out intSecurePolicy))
                    {
                        return (CookieSecurePolicy)intSecurePolicy;
                    }
                    else
                    {
                        CookieSecurePolicy securePolicy;

                        if (Enum.TryParse(SecurePolicy, out securePolicy))
                        {
                            return securePolicy;
                        }
                    }
                }

                return null;
            }
        }
    }
    public class AuthenticationConfigItem
    {
        public string Name { get; set; }
        public string ApplicationName { get; set; }
        public string AuthenticationScheme { get; set; }
        public string DataProtectionDir { get; set; }
        public bool IsActive { get; set; }
        public string AccessDeniedPath { get; set; }
        public CookieConfig Cookie { get; set; }
        public TimeSpan? ExpireTimeSpan { get; set; }
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public string ReturnUrlParameter { get; set; }
        public bool? SlidingExpiration { get; set; }
    }
    public class AuthenticationConfig : List<AuthenticationConfigItem>
    {
    }
}
