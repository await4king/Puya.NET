using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Puya.Security.Microsoft;
using Puya.Security.Models;

namespace Puya.Security
{
    public static class Extensions
    {
        public static Dictionary<string, bool> GetUserFormAccessRights(this ISecurityAccessClass securityAccessClass, string username, string system, string subsystem, string form, string section, string createdBy = "")
        {
            return securityAccessClass.GetUserFormAccessRights(username,
                new SecurityLocation
                {
                    System = system,
                    SubSystem = subsystem,
                    Form = form,
                    Section = section
                }, createdBy);
        }
        public static Dictionary<string, bool> GetUserCatalogAccessRights(this ISecurityAccessClass securityAccessClass, string username, string system, string subsystem, string catalog, string section)
        {
            return securityAccessClass.GetUserCatalogAccessRights(username,
                new SecurityLocation
                {
                    System = system,
                    SubSystem = subsystem,
                    Form = catalog,
                    Section = section
                });
        }
        public static Dictionary<string, bool> GetUserOperationAccessRights(this ISecurityAccessClass securityAccessClass, string username, string system, string subsystem, string section)
        {
            return securityAccessClass.GetUserOperationAccessRights(username,
                new SecurityLocation
                {
                    System = system,
                    SubSystem = subsystem,
                    Section = section
                });
        }
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
