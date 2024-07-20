using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Puya.IdentityModel
{
    public static class Extensions
    {
        public static string ToSecurityAlgorithm(this string securityAlgorithm)
        {
            var result = string.Empty;

            if (Enum.TryParse(securityAlgorithm, out SecurityAlgorithm type))
            {
                var fld = typeof(SecurityAlgorithms).GetFields().FirstOrDefault(f => string.Compare(f.Name, securityAlgorithm, true) == 0);

                if (fld != null)
                {
                    result = (string)fld.GetValue(null);
                }
            }

            return result;
        }
    }
}
