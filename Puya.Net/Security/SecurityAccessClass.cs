using System;
using System.Collections.Generic;
using System.Text;
using Puya.Conversion;
using Puya.Data;
using Puya.Security.Models;

namespace Puya.Security
{
    public class SecurityAccessClass : ISecurityAccessClass
    {
        private readonly IDb _db;
        public SecurityAccessClass(IDb db)
        {
            _db = db;
        }
        public bool IsAdmin(string username)
        {
            var isAdmin = _db.ExecuteScalerSql($"SELECT dbo.UDF_GetIsAdmin(@username)", new { username });

            return SafeClrConvert.ToBoolean(isAdmin);
        }
        public bool IsSubordinate(string Subordinate, string Head)
        {
            var result = _db.ExecuteScalerSql($@" SELECT 1 FROM VW_SecuritySubordinates 
                                                  WHERE Head = N'{Head}' AND Subordinate = N'{Subordinate}''");
            return result != null;
        }
        IList<SecurityAccess> GetUserAccessRights(string username, string permissionClass, SecurityLocation location)
        {
            var result = _db.ExecuteReaderCommand<SecurityAccess>("USP_GetUserAccessRights",
                new
                {
                    UserName = username,
                    PermissionClass = permissionClass,
                    location.System,
                    location.SubSystem,
                    location.Form,
                    location.Section
                });

            return result;
        }
        private Dictionary<string, bool> GetUserAccessRights(string username, string permissionClass, SecurityLocation location, string createdBy)
        {
            var isSubordinate = string.IsNullOrEmpty(createdBy) || IsSubordinate(createdBy, username);
            var all = GetUserAccessRights(username, permissionClass, location);
            long access = 0;

            foreach (var a in all)
            {
                access |= a.Access;
            }

            var result = new Dictionary<string, bool>();

            Array accesslist;

            switch (permissionClass)
            {
                case "1":
                    accesslist = Enum.GetValues(typeof(TBrowseAccess));
                    break;
                case "2":
                    accesslist = Enum.GetValues(typeof(TCatalogAccess));
                    break;
                default:
                    accesslist = new object[] { };
                    break;
            }
            
            foreach (var tba in accesslist)
            {
                var ba = (long)tba;
                var cp2 = (long)Math.Pow(2, (long)Math.Floor(Math.Log(ba, 2))); // closest power of 2 (bitwise).
                                                                                // eg. #1: if ba = 12 (001100) then cp2 will be 8  (001000)
                                                                                // eg. #2: if ba = 48 (110000) then cp2 will be 32 (100000)
                var baAndAccess = ba & access;

                if
                (
                  (baAndAccess == cp2)  // 2: public
                  ||
                  (baAndAccess == (cp2 >> 1) && (string.IsNullOrEmpty(createdBy) || string.Compare(username, createdBy, true) == 0))  // 1: private
                  ||
                  (baAndAccess == ba && isSubordinate)  // 3: sub
                )
                    result.Add(tba.ToString(), true);
                else
                    result.Add(tba.ToString(), false);
            }

            return result;
        }
        public Dictionary<string, bool> GetUserCatalogAccessRights(string username, SecurityLocation location)
        {
            return GetUserAccessRights(username, "2", location, "");
        }
        public Dictionary<string, bool> GetUserFormAccessRights(string username, SecurityLocation location, string createdBy = "")
        {
            return GetUserAccessRights(username, "1", location, createdBy);
        }
        public Dictionary<string, bool> GetUserOperationAccessRights(string username, SecurityLocation location)
        {
            var all = GetUserAccessRights(username, "3", location);
            var result = new Dictionary<string, bool>();

            foreach (var a in all)
            {
                if (!result.ContainsKey(a.DialogName))
                {
                    if (a.Access == 0)
                        result.Add(a.DialogName, false);
                    else
                        result.Add(a.DialogName, true);
                }
                else
                {
                    if (a.Access != 0)
                        result[a.DialogName] = true;
                }
            }

            return result;
        }
    }
}
