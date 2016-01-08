using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.Models.Cmp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class PermissionDecisions
    {
        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        protected biz.dfch.CS.Appclusive.Api.Core.Core CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.Api.Core.Core(new Uri(Properties.Settings.Default.AppculsiveApiBaseUrl + "Core"));
                    coreRepository.IgnoreMissingProperties = true;
                    coreRepository.Format.UseJson();
                    coreRepository.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
                    coreRepository.MergeOption = MergeOption.PreserveChanges;

                    System.Net.NetworkCredential apiCreds = HttpContext.Current.Session["LoginData"] as System.Net.NetworkCredential;
                    if (null != apiCreds)
                    {
                        coreRepository.Credentials = apiCreds;
                    }
                    else
                    {
                        coreRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    }
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Core.Core coreRepository;

        public static PermissionDecisions Current
        {
            get
            {
                PermissionDecisions current = HttpContext.Current.Session["PermissionDecisions"] as PermissionDecisions;
                if (current == null)
                {
                    current = new PermissionDecisions(null, null);
                }
                return current;
            }
        }
        public static bool generalDefaultValue = false; // TODO cwi: FALSE: Everything that has no explizit permission is NOT accessible. 
        private Dictionary<Type, List<string>> permissions = new Dictionary<Type, List<string>>(); 

        public PermissionDecisions(string username, string domain)
        {
            if (!string.IsNullOrEmpty(username))
            {
                // TODO: only users permission load
                List<Api.Core.Permission> permissions = CoreRepository.Permissions.ToList();

                // Permissions must be loaded here. 
                // TODO cwi: IEnumerable
                //permissions.Add(typeof(IEnumerable<Customer>), new List<string>() { "CanCreate", "CanRead", "CanUpdate", "CanDelete" });
                //permissions.Add(typeof(IEnumerable<Tenant>), new List<string>() { "CanCreate", "CanRead", "CanUpdate" });
                //permissions.Add(typeof(IEnumerable<User>), new List<string>() { "CanCreate", "CanRead" });
                //permissions.Add(typeof(IEnumerable<Approval>), new List<string>() { "CanCreate", "CanRead", "CanUpdate", "CanDelete" });

                //// TODO cwi: Menus ohne IEnumerable
                //permissions.Add(typeof(Customer), new List<string>() { "CanRead" });
                //permissions.Add(typeof(Tenant), new List<string>() { "CanRead" });
                //permissions.Add(typeof(User), new List<string>() { "CanRead" });
                //permissions.Add(typeof(Catalogue), new List<string>() { "CanRead" });
                //permissions.Add(typeof(CimiTarget), new List<string>() { "CanDelete" });
                //permissions.Add(typeof(Approval), new List<string>() { "CanRead" });
            }
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