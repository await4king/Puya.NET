using System;
using System.Collections.Generic;
using System.Text;
using Puya.Security.Models;

namespace Puya.Security
{
    public interface ISecurityAccessClass
    {
        bool IsAdmin(string username);
        bool IsSubordinate(string Subordinate, string Head);
        Dictionary<string, bool> GetUserFormAccessRights(string username, SecurityLocation location, string createdBy = "");
        Dictionary<string, bool> GetUserCatalogAccessRights(string username, SecurityLocation location);
        Dictionary<string, bool> GetUserOperationAccessRights(string username, SecurityLocation location);
    }
}
