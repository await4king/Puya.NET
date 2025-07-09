using System;
using System.Linq;
using System.Security.Claims;
using Puya.Security.Microsoft;

namespace Puya.Security
{
    public static class Extensions
    {
        public static string ToClaimType(this string claimType)
        {
            var result = string.Empty;

            if (Enum.TryParse(claimType, out ClaimType type))
            {
                var fld = typeof(ClaimTypes).GetFields().FirstOrDefault(f => string.Compare(f.Name, claimType, true) == 0);

                if (fld != null)
                {
                    result = (string)fld.GetValue(null);
                }
            }

            return result;
        }
    }
}
