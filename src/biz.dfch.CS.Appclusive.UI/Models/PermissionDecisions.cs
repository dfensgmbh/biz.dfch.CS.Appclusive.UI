using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.Models.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class PermissionDecisions
    {
        public static PermissionDecisions Current { get { return new PermissionDecisions(); } }
        public static bool generalDefaultValue = true; // TODO cwi: FALSE: Everything that has no explizit permission is NOT accessible. 
        private Dictionary<Type, List<string>> permissions = new Dictionary<Type, List<string>>(); 

        public PermissionDecisions()
        {

            // Permissions must be loaded here. 
            // TODO cwi: IEnumerable
            permissions.Add(typeof(IEnumerable<Customer>), new List<string>() { "CanCreate", "CanRead", "CanUpdate", "CanDelete" });
            permissions.Add(typeof(IEnumerable<Tenant>), new List<string>() { "CanCreate", "CanRead", "CanUpdate" });
            permissions.Add(typeof(IEnumerable<User>), new List<string>() { "CanCreate", "CanRead" });
            permissions.Add(typeof(IEnumerable<Approval>), new List<string>() { "CanCreate", "CanRead", "CanUpdate", "CanDelete" });

            // TODO cwi: Menus ohne IEnumerable
            permissions.Add(typeof(Customer), new List<string>() { "CanRead" });
            permissions.Add(typeof(Tenant), new List<string>() { "CanRead" });
            permissions.Add(typeof(User), new List<string>() { "CanRead" });
            permissions.Add(typeof(Catalogue), new List<string>() { "CanRead" });
            permissions.Add(typeof(CimiTarget), new List<string>() { "CanDelete" });
            permissions.Add(typeof(Approval), new List<string>() { "CanRead" });
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
     
    }
}