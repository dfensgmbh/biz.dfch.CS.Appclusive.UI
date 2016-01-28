using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Permission : AppcusiveEntityViewModelBase
    {

        static internal Permission GetFromCache(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository, long id)
        {
            string cacheKey = "permission_" + id;
            Permission permission = (Permission)System.Web.HttpContext.Current.Cache.Get(cacheKey);
            if (null == permission)
            {
                Permission
                Api.Core.Role role = GetCachedRole(roleId);
                foreach (var perm in CoreRepository.Permissions)
                {
                    if (role == null || role.Permissions == null || role.Permissions.Count == 0
                        || role.Permissions.Where(p => p.Id == perm.Id).Count() == 0)
                    {
                        options.Add(new AjaxOption(perm.Id, perm.Name));
                    }
                }
                System.Web.HttpContext.Current.Cache.Add(cacheKeyPermissions, options, null, DateTime.Now.AddSeconds(30), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return permission;

        }
    }
}