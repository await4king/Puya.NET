using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Puya.Net;
using System.Linq;
using System.Net;

namespace Puya.Web
{
    public class IPAddressService: IIPAddressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IPAddressService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetClientIPAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return string.Empty;

            // Try to get IP from X-Forwarded-For header (for proxies)
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues forwardedFor))
            {
                return forwardedFor.FirstOrDefault()?.Split(',')[0].Trim();
            }

            // Try to get IP from X-Real-IP header
            if (context.Request.Headers.TryGetValue("X-Real-IP", out StringValues realIp))
            {
                return realIp.FirstOrDefault();
            }

            // Fall back to RemoteIpAddress
            return context.Connection.RemoteIpAddress?.ToString();
        }

        public string GetClientIPAddressWithIPv4()
        {
            var ipAddress = GetClientIPAddress();

            if (IPAddress.TryParse(ipAddress, out var address))
            {
                // Handle IPv4-mapped IPv6 addresses
                if (address.IsIPv4MappedToIPv6)
                {
                    return address.MapToIPv4().ToString();
                }

                // Return IPv4 address
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }

                // Convert IPv6 to IPv4 if possible (for localhost, etc.)
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    return address.MapToIPv4().ToString();
                }
            }

            return ipAddress;
        }
    }
}
