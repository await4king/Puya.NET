using System.Collections.Generic;
using System.Security.Claims;

namespace Puya.Extensions.WebCore
{
    public static class WbCoreExtensions
    {
        public static IEnumerable<string> Roles(this ClaimsPrincipal principal)
        {
            var result = new List<string>();

            foreach (var identity in principal.Identities)
            {
                if (identity != null)
                {
                    foreach (var claim in identity.Claims)
                    {
                        if (claim.Type == identity.RoleClaimType)
                        {
                            result.Add(claim.Value);
                        }
                    }
                }
            }

            return result;
        }
    }
}
