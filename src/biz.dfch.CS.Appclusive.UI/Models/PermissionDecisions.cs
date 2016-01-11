using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.Models.Cmp;
using biz.dfch.CS.Appclusive.UI.Models.SpecialOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services.Client;
using biz.dfch.CS.Appclusive.UI.Navigation;

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
        private Dictionary<string, NavGroup> Navigation;

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
            // Load permissions:
            if (!string.IsNullOrEmpty(username))
            {
                permissions = CoreRepository.Permissions.AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", 10000).ToList();
                //string name = (!string.IsNullOrEmpty(domain) ? (domain + "\\") : "") + username;
                //List<Api.Core.Role> userRoles = CoreRepository.Roles.Expand("Permissions")
                //    .Where(r => null != r.Users.Where(u => u.Name == name).FirstOrDefault())
                //    .ToList();

                //List<Api.Core.Permission> permissions = new List<Api.Core.Permission>();
                //userRoles.ForEach(r => permissions.AddRange(r.Permissions));
            }
            else
            {
                permissions = new List<Api.Core.Permission>();
            }

            Navigation = CreateNavigation();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="action">without entity</param>
        /// <returns></returns>
        private bool IsAllowed(Type modelType, string action)
        {
            string permissionName = string.Format("Apc:{0}s{1}", modelType.Name, action); // Apc:CataloguesCanRead 
            return IsAllowed(permissionName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionName">Apc:CataloguesCanRead</param>
        /// <returns></returns>
        private bool IsAllowed(string permissionName)
        {
            Api.Core.Permission permission = permissions.Where(p => p.Name == permissionName).FirstOrDefault();
            return (permission != null);
        }

        private Dictionary<string, NavGroup> CreateNavigation()
        {
            NavigationConfigurationSection configSection = (NavigationConfigurationSection)System.Configuration.ConfigurationManager.GetSection(NavigationConfigurationSection.SectionName);
            Dictionary<string, NavGroup> navigation = new Dictionary<string, NavGroup>();

            foreach (NavGroupElement groupConfig in configSection.NavGroups)
            {
                NavGroup group = AutoMapper.Mapper.Map<NavGroup>(groupConfig);
                foreach (NavEntryElement entryConfig in groupConfig.NavEntries)
                {
                    if (String.IsNullOrWhiteSpace(entryConfig.Permission) || IsAllowed(entryConfig.Permission))
                    {
                        group.NavEntries.Add(AutoMapper.Mapper.Map<NavEntry>(entryConfig));
                    }
                }
                if (group.NavEntries.Count>0)
                {
                    navigation.Add(group.Name, group);
                }
            }
            return navigation;
        }

        #endregion

        #region internal methods

        internal bool CanCreate(Type modelType)
        {
            return IsAllowed(modelType, "CanCreate");
        }

        internal bool CanRead(Type modelType)
        {
            return IsAllowed(modelType, "CanRead");
        }

        internal bool CanUpdate(Type modelType)
        {
            return IsAllowed(modelType, "CanUpdate");
        }

        internal bool CanDelete(Type modelType)
        {
            return IsAllowed(modelType, "CanDelete");
        }
        
        internal bool CanDecrypt(Type modelType)
        {
            return IsAllowed(modelType, "CanDecrypt");
        }

        #endregion
    }
}