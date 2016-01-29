using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.Models.Cmp;
using biz.dfch.CS.Appclusive.UI.Models.SpecialOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services.Client;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Navigation
{
    public class PermissionDecisions
    {
        #region Core Repository

        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        internal biz.dfch.CS.Appclusive.Api.Core.Core CoreRepositoryGet()
        {
            biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = new biz.dfch.CS.Appclusive.Api.Core.Core(new Uri(Properties.Settings.Default.AppculsiveApiBaseUrl + "Core"));
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

            return coreRepository;
        }

        #endregion

        #region properties

        private List<Models.Core.Permission> permissions = null;
        public Dictionary<string, NavEntry> Navigation;
        public List<Tenant> Tenants { get; private set; }

        /// <summary>
        /// current tenant
        /// </summary>
        public Tenant Tenant {
            get {
                if (tenant == null)
                {
                    tenant = new Tenant() { Id = Guid.Empty, Name = "" };
                }
                return tenant;
            }
            set { tenant = value; }
        }
        Tenant tenant = null;

        public User CurrentUser { get; private set; }

        /// <summary>
        /// returns cart id if available, 
        /// 0 if there is no cart
        /// lt 0 if there are several
        /// </summary>
        public long ShoppingCartId
        {
            get
            {
                if (null == CurrentUser)
                {
                    return 0;
                }
                biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = this.CoreRepositoryGet();
                Api.Core.Cart[] carts = coreRepository.Carts.ToArray();
                switch (carts.Length)
                {
                    case 0: return 0;
                    case 1: return carts[0].Id;
                    default: return -1;
                }
            }
        }

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
            if (!string.IsNullOrEmpty(username))
            {
                biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = this.CoreRepositoryGet();

                // load tenants
                Tenants = AutoMapper.Mapper.Map<List<Models.Core.Tenant>>(coreRepository.Tenants.ToList());
                Tenants.Add(new Tenant() { Id = Guid.Empty, Name = GeneralResources.TenantSwitchAll });

                // load user
                string fullUserName = string.Format("{0}\\{1}",domain,username);
                CurrentUser = AutoMapper.Mapper.Map<Models.Core.User>(coreRepository.Users.Where(u => u.Name == fullUserName).FirstOrDefault());

                // default tenant
                if (null != CurrentUser)
                {
                    this.Tenant = this.Tenants.FirstOrDefault(t => t.Id == CurrentUser.Tid);
                }

                // Load permissions:            
                permissions = Models.Core.Permission.GetPermissionsFromCache();


                //string name = (!string.IsNullOrEmpty(domain) ? (domain + "\\") : "") + username;
                //List<Api.Core.Role> userRoles = CoreRepository.Roles.Expand("Permissions")
                //    .Where(r => null != r.Users.Where(u => u.Name == name).FirstOrDefault())
                //    .ToList();

                //List<Api.Core.Permission> permissions = new List<Api.Core.Permission>();
                //userRoles.ForEach(r => permissions.AddRange(r.Permissions));
            }
            else
            {
                Tenants = new List<Tenant>();
                permissions = new List<Models.Core.Permission>();
            }

            Navigation = CreateNavigation();
        }


        #endregion

        #region Private Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="action">without entity: CanRead</param>
        /// <returns></returns>
        public bool HasPermission(Type modelType, string action)
        {
            return HasPermission(modelType.Name, action);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelType"></param>
        /// <param name="action">without entity: CanRead</param>
        /// <returns></returns>
        public bool HasPermission(string modelName, string action)
        {
            string permissionName = string.Format("Apc:{0}s{1}", modelName, action); // Apc:CataloguesCanRead 
            return HasPermission(permissionName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionName">Apc:CataloguesCanRead</param>
        /// <returns></returns>
        private bool HasPermission(string permissionName)
        {
            Models.Core.Permission permission = permissions.Where(p => p.Name == permissionName).FirstOrDefault();
            return (permission != null);
        }

        private Dictionary<string, NavEntry> CreateNavigation()
        {
            NavigationConfigurationSection configSection = (NavigationConfigurationSection)System.Configuration.ConfigurationManager.GetSection(NavigationConfigurationSection.SectionName);
            Dictionary<string, NavEntry> navigation = new Dictionary<string, NavEntry>();

            foreach (NavEntryElement groupConfig in configSection.NavGroups)
            {
                NavEntry group = AutoMapper.Mapper.Map<NavEntry>(groupConfig);
                foreach (NavEntryElement entryConfig in groupConfig.NavEntryElements)
                {
                    if (!String.IsNullOrWhiteSpace(groupConfig.Action) && !String.IsNullOrWhiteSpace(groupConfig.Controller)
                        || !String.IsNullOrWhiteSpace(entryConfig.Action) && !String.IsNullOrWhiteSpace(entryConfig.Controller))
                    {
                        string permissionName = entryConfig.Permission;
                        if (String.IsNullOrWhiteSpace(permissionName))
                        {
                            permissionName = string.Format("Apc:{0}CanRead", entryConfig.Controller); //Apc:AcesCanRead
                        }
                        if (permissionName == "*" || HasPermission(permissionName))
                        {
                            group.NavEntries.Add(AutoMapper.Mapper.Map<NavEntry>(entryConfig));
                        }
                    }
                }
                if (group.NavEntries.Count > 0)
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
            return HasPermission(modelType, "CanCreate");
        }

        internal bool CanRead(Type modelType)
        {
            return HasPermission(modelType, "CanRead");
        }

        internal bool CanUpdate(Type modelType)
        {
            return HasPermission(modelType, "CanUpdate");
        }

        internal bool CanDelete(Type modelType)
        {
            return HasPermission(modelType, "CanDelete");
        }
        
        internal bool CanDecrypt(Type modelType)
        {
            return HasPermission(modelType, "CanDecrypt");
        }

        #endregion
    }
}