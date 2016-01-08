using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.Models.Cmp;
using biz.dfch.CS.Appclusive.UI.Models.SpecialOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class PermissionDecisions
    {
        #region Core Repository
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

        #endregion

        #region private variables

        private List<Api.Core.Permission> permissions = null;

        #endregion

        #region Current instance

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

        #endregion

        #region Constructors

        public PermissionDecisions(string username, string domain)
        {
            //if (!string.IsNullOrEmpty(username))
            //{
            //    string name = (!string.IsNullOrEmpty(domain) ? (domain + "\\") : "") + username;
            //    List<Api.Core.Role> userRoles = CoreRepository.Roles.Expand("Permissions")
            //        .Where(r => null != r.Users.Where(u => u.Name == name).FirstOrDefault())
            //        .ToList();

            //    List<Api.Core.Permission> permissions = new List<Api.Core.Permission>();
            //    userRoles.ForEach(r => permissions.AddRange(r.Permissions));
            //}
            //else
            //{
                permissions = CoreRepository.Permissions.AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", 10000).ToList();
            //}
        }

        #endregion

        #region Private Helpers

        private bool GetPermission(Type modelType, string operation)
        {
            string permissionName = string.Format("Apc:{0}s{1}", modelType.Name, operation); // Apc:CataloguesCanRead 
            Api.Core.Permission permission = permissions.Where(p => p.Name == permissionName).FirstOrDefault();
            return true; // TODO cwi:  (permission != null);
        }

        #endregion

        #region internal methods

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

        #endregion
    }
}