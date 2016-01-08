using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.Models.Cmp;
using biz.dfch.CS.Appclusive.UI.Models.SpecialOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class PermissionDecisions
    {
        public static PermissionDecisions Current { get { return new PermissionDecisions(); } }
        public static bool generalDefaultValue = false; // TODO cwi: FALSE: Everything that has no explizit permission is NOT accessible. 
        private Dictionary<Type, List<string>> permissions = new Dictionary<Type, List<string>>(); 

        public PermissionDecisions()
        {
            permissions.Add(typeof(Customer), new List<string>() { "CanCreate", "CanRead", "CanUpdate", "CanDelete" });
            permissions.Add(typeof(ManagementCredential), new List<string>() { "CanCreate", "CanRead", "CanUpdate", "CanDelete" });
            permissions.Add(typeof(Tenant), new List<string>() { "CanRead" });
            permissions.Add(typeof(User), new List<string>() { "CanRead" });
            permissions.Add(typeof(Catalogue), new List<string>() { "CanCreate", "CanRead" });
            permissions.Add(typeof(CimiTarget), new List<string>() { "CanDelete" });
            permissions.Add(typeof(Approval), new List<string>() { "CanRead" });
            permissions.Add(typeof(CatalogueItem), new List<string>() { "CanCreate", "CanRead" });
            permissions.Add(typeof(Ace), new List<string>() { "CanCreate", "CanRead" });
        }
        
        private bool GetPermission(Type modelType, string operation)
        {
            if (permissions.ContainsKey(modelType))
            {
                return permissions[modelType].Contains(operation);
            }

            return generalDefaultValue;
        }

        internal bool CanCreate(Type modelType)
        {
            return GetPermission(modelType, "CanCreate");
        }

        internal bool CanRead(Type modelType)
        {
            return GetPermission(modelType, "CanRead");
        }

        internal bool CanUpdate(Type modelType)
        {
            return GetPermission(modelType, "CanUpdate");
        }

        internal bool CanDelete(Type modelType)
        {
            return GetPermission(modelType, "CanDelete");
        }


        internal bool CanDecrypt(Type modelType)
        {
            return GetPermission(modelType, "CanDecrypt");
        }
    }
}