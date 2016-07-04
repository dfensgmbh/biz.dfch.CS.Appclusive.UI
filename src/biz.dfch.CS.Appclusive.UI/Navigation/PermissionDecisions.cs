using biz.dfch.CS.Appclusive.UI.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using biz.dfch.CS.Appclusive.UI.Managers;

namespace biz.dfch.CS.Appclusive.UI.Navigation
{
    public class PermissionDecisions
    {
        #region Core Repository

        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        internal AuthenticatedCoreApi CoreRepositoryGet()
        {
            return new AuthenticatedCoreApi();
        }

        #endregion

        #region properties

        public Dictionary<string, NavEntry> Navigation = new Dictionary<string, NavEntry>();
        public List<Tenant> Tenants { get; private set; }

        /// <summary>
        /// current tenant
        /// </summary>
        private Tenant _tenant = null;
        public Tenant Tenant {
            get { return _tenant ?? (_tenant = new Tenant {Id = Guid.Empty, Name = ""}); }
            set
            {
                _tenant = value;
                Permissions = GetPermissionsForTenant(_tenant.Id);
                Navigation = CreateNavigation();
            }
        }

        private IEnumerable<Ace> Permissions { get; set; }

        private IEnumerable<Ace> GetPermissionsForTenant(Guid tenantId)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Ace>>(new PermissionManager().Aces(tenantId)); ;
        }

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
                try
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
                catch
                {
                    return 0;
                }
            }
        }

        #endregion

        #region Current instance

        public static PermissionDecisions Current
        {
            get
            {
                return HttpContext.Current.Session["PermissionDecisions"] as PermissionDecisions
                            ?? new PermissionDecisions(null);
            }
        }

        #endregion

        #region Constructors

        private PermissionManager _permissionManager;
        public PermissionDecisions(string username)
        {
            Tenants = new List<Tenant>();

            if (!string.IsNullOrEmpty(username))
            {
                var coreRepository = CoreRepositoryGet();
                _permissionManager = new PermissionManager(coreRepository);

                // load tenants
                Tenants = AutoMapper.Mapper.Map<List<Tenant>>(coreRepository
                    .Tenants
                    .Where(t => !t.Name.Equals("HOME_TENANT") && !t.Name.Equals("GROUP_TENANT"))
                    .ToList());
                // Tenants.Add(new Tenant { Id = Guid.Empty, Name = GeneralResources.TenantSwitchAll });

                this.CurrentUser = AutoMapper.Mapper.Map<User>(coreRepository.InvokeEntitySetActionWithSingleResult<Api.Core.User>("Users", "Current", null));
                
                // default tenant
                if (null != CurrentUser)
                {
                    CurrentUser.ResolveNavigationProperties();
                    Tenant = Tenants.FirstOrDefault(t => t.Id == CurrentUser.Tid);
                }
            }
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
            var grantedPermissions = _permissionManager.ReadPermissions(Tenant.Id);
            return grantedPermissions.Any(p => p.Equals(permissionName));
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