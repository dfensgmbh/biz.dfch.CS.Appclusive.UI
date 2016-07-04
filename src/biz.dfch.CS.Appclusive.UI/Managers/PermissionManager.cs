using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using biz.dfch.CS.Appclusive.Api.Core;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Managers
{
    public class PermissionManager
    {
        private const string PermissionsCacheKey = "_PermissionManager_Permissions";
        private const string TenantPermissionsCacheKey = "_PermissionManager_TenantPermissions";
        
        private const long AllPermissions = 0;

        private readonly AuthenticatedCoreApi _authenticatedCoreApi;

        public PermissionManager() : this(new AuthenticatedCoreApi())
        {
        }

        public PermissionManager(AuthenticatedCoreApi authenticatedCoreApi)
        {
            _authenticatedCoreApi = authenticatedCoreApi;
        }

        public IEnumerable<Permission> Permissions
        {
            get
            {
                var cachedPermissions = HttpContext.Current.Session[PermissionsCacheKey] as IEnumerable<Permission>;

                if (cachedPermissions == null)
                {
                    cachedPermissions = LoadPermissions();
                    HttpContext.Current.Session[PermissionsCacheKey] = cachedPermissions;
                }

                return cachedPermissions;
            }
        }

        public IEnumerable<Ace> Aces(Guid tenantId)
        {
            Contract.Requires(tenantId != null);
            Contract.Requires(tenantId != Guid.Empty);

            return GetEffectivePermissionForNode(GetAdminUiConfigurationNode(tenantId));
        }

        private Node GetAdminUiConfigurationNode(Guid tenantId)
        {
            Contract.Requires(tenantId != null);
            Contract.Requires(tenantId != Guid.Empty);

            return GetNodeByEntityKindId(tenantId, GetAdminUiConfigurationEntityKindId());
        }

        private IEnumerable<Ace> GetEffectivePermissionForNode(Node node)
        {
            Contract.Requires(node != null);

            var permissions = _authenticatedCoreApi
                .InvokeEntityActionWithListResult<Ace>("Nodes", node.Id, "GetEffectivePermissions", null);
            return permissions;
        }

        private IEnumerable<Permission> LoadPermissions()
        {
            var permissions = new List<Permission>();
            long totalCount;

            do
            {
                var response = _authenticatedCoreApi
                    .Permissions
                    .AddQueryOption("$skip", permissions.Count)
                    .IncludeTotalCount()
                    .Execute() as QueryOperationResponse<Permission>;
                Contract.Assert(response != null);

                totalCount = response.TotalCount;
                permissions.AddRange(response);
            } while (permissions.Count < totalCount);


            return permissions;
        }

        public IEnumerable<string> TenantPermissions(Guid tenantId)
        {
            var cacheKey = string.Format("{0}_{1}", TenantPermissionsCacheKey, tenantId);
            var cachedPermissions = HttpContext.Current.Session[cacheKey] as IEnumerable<string>;

            if (cachedPermissions == null)
            {
                cachedPermissions = LoadTenantPermissions(tenantId).ToList().AsEnumerable();
                HttpContext.Current.Session[cacheKey] = cachedPermissions;
            }

            return cachedPermissions;
        }

        private Permission GetPermissionById(long id)
        {
            var permission = Permissions.FirstOrDefault(p => p.Id == id);
            Contract.Assert(permission != null);

            return permission;
        }

        private IEnumerable<string> LoadTenantPermissions(Guid tenantId)
        {
            var aces = Aces(tenantId);

            var grantedPermissions = new Dictionary<string, bool>();

            foreach (var ace in aces)
            {
                if (ace.Type == (long) AceType.Alarm || ace.Type == (long) AceType.Audit)
                {
                    continue;
                }

                if (ace.PermissionId == AllPermissions)
                {
                    foreach (var permission in Permissions)
                    {
                        Set(grantedPermissions, permission.Name, ace.Type == (long) AceType.Allow);
                    }
                }
                else
                {
                    var permission = GetPermissionById(ace.PermissionId);
                    Set(grantedPermissions, permission.Name, ace.Type == (long) AceType.Allow);
                }
            }

            return grantedPermissions
                .Where(pair => pair.Value)
                .Select(pair => pair.Key);
        }

        private void Set(Dictionary<string, bool> grantDictionary, string key, bool grant)
        {
            bool currentGrant;

            if (grantDictionary.TryGetValue(key, out currentGrant))
            {
                if (currentGrant != grant && grant == false)
                {
                    grantDictionary[key] = false;
                }
            }
            else
            {
                grantDictionary.Add(key, grant);
            }
        }

        private Node GetNodeByEntityKindId(Guid tenantId, long adminUiConfigurationEntityKindId)
        {
            Contract.Requires(adminUiConfigurationEntityKindId > 0);

            var node = _authenticatedCoreApi.Nodes
                .Where(entity => entity.EntityKindId == adminUiConfigurationEntityKindId && entity.Tid == tenantId)
                .FirstOrDefault();
            Contract.Assert(node != null, "|404| Admin UI Configuration Node not found");

            return node;
        }

        private long GetAdminUiConfigurationEntityKindId()
        {
            var entityKind = _authenticatedCoreApi.EntityKinds
                .Where(entity => entity.Name.Equals(PermissionConfig.AdminUIConfigurationEntityKindName))
                .FirstOrDefault();
            Contract.Assert(entityKind != null, "|404| Admin UI Configuration EntityKind is not installed.");

            return entityKind.Id;
        }

        private enum AceType : long
        {
            Deny = 1,
            Allow = 2,
            Audit = 3,
            Alarm = 4
        }
    }
}