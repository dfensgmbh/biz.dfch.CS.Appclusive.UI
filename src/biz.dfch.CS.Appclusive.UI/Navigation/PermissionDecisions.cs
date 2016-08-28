using biz.dfch.CS.Appclusive.UI.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using biz.dfch.CS.Appclusive.UI.Helpers;
using biz.dfch.CS.Appclusive.UI.Managers;

namespace biz.dfch.CS.Appclusive.UI.Navigation
{
    public class PermissionDecisions
    {
        #region Core Repository

        private AuthenticatedCoreApi _coreRepository;
        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        internal AuthenticatedCoreApi GetCoreRepository()
        {
            return _coreRepository ?? (_coreRepository = new AuthenticatedCoreApi());
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
                if (_tenant != null)
                {
                    GetCoreRepository().TenantID = _tenant.Id.ToString();
                    Permissions = GetPermissionsForTenant(_tenant.Id);
                    Navigation = CreateNavigation();
                }
            }
        }

        private IEnumerable<Ace> Permissions { get; set; }

        private IEnumerable<Ace> GetPermissionsForTenant(Guid tenantId)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Ace>>(_permissionManager.Aces(tenantId)); ;
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
                    biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = this.GetCoreRepository();
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

        private readonly PermissionManager _permissionManager;
        public PermissionDecisions(string username)
        {
            Tenants = new List<Tenant>();

            if (!string.IsNullOrEmpty(username))
            {
                var coreRepository = GetCoreRepository();
                _permissionManager = new PermissionManager(coreRepository);

                // load tenants
                Tenants = AutoMapper.Mapper.Map<List<Tenant>>(coreRepository
                    .Tenants
                    .Where(t => !TenantHelper.IsBuiltInTenant(t))
                    .ToList());
                this.CurrentUser = AutoMapper.Mapper.Map<User>(coreRepository.InvokeEntitySetActionWithSingleResult<Api.Core.User>("Users", "Current", null));

                if (TenantHelper.HasFixedTenantId)
                {
                    Tenant = Tenants.FirstOrDefault(t => t.Id.Equals(TenantHelper.FixedTenantId))
                             ?? Tenants.FirstOrDefault();
                }
                else if (null != CurrentUser)
                {
                    CurrentUser.ResolveNavigationProperties();
                    Tenant = Tenants.FirstOrDefault(t => t.Id == CurrentUser.Tid)
                             ?? Tenants.FirstOrDefault();
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
        /// <param name="modelName"></param>
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
            var grantedPermissions = _permissionManager.TenantPermissions(Tenant.Id);
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