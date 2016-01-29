using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class User : AppcusiveEntityViewModelBase
    {
        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "ExternalId", ResourceType = typeof(GeneralResources))]
        public string ExternalId { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "ExternalType", ResourceType = typeof(GeneralResources))]
        public string ExternalType { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Mail", ResourceType = typeof(GeneralResources))]
        public string Mail { get; set; }

        [Display(Name = "Roles", ResourceType = typeof(GeneralResources))]
        public List<Role> Roles { get; set; }

        [Display(Name = "Permissions", ResourceType = typeof(GeneralResources))]
        public List<Permission> Permissions { get; set; }

        internal void ResolveNavigationProperties()
        {
            biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();

            // Roles & Permissions
            if (null == this.Roles || null == this.Permissions)
            {
                this.Roles = new List<Role>();
                this.Permissions = new List<Permission>();

                try
                {
                    // load all roles
                    List<Role> allRoles = Models.Core.Role.GetRolesFromCache();
                    // Navigatin properties deliver only 45 entires
                    // TODO load all permissions and roles

                    foreach (Role role in allRoles.Where(r => r.Users!=null && r.Users.Exists(u => u.Id == this.Id)))
                    {
                        this.Roles.Add(role);
                        if (role.Permissions != null)
                        {
                            foreach (Permission rp in role.Permissions)
                            {
                                if (!this.Permissions.Exists(up => up.Id == rp.Id))
                                {
                                    this.Permissions.Add(rp);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }


        internal static List<User> GetUsersFromCache()
        {
            string cacheKey = "users";
            List<User> acls = (List<User>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
            if (null == acls)
            {
                lock (locker)
                {
                    acls = (List<User>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
                    if (null == acls)
                    {
                        acls = new List<Models.Core.User>();

                        biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
                        QueryOperationResponse<Api.Core.User> queryResponse = coreRepository.Users.AddQueryOption("$top", 10000).Execute() as QueryOperationResponse<Api.Core.User>;
                        while (null != queryResponse)
                        {
                            acls.AddRange(AutoMapper.Mapper.Map<List<Models.Core.User>>(queryResponse.ToList()));
                            DataServiceQueryContinuation<Api.Core.User> cont = queryResponse.GetContinuation();
                            if (null != cont)
                            {
                                queryResponse = coreRepository.Execute<Api.Core.User>(cont) as QueryOperationResponse<Api.Core.User>;
                            }
                            else
                            {
                                queryResponse = null;
                            }
                        }

                        System.Web.HttpContext.Current.Cache.Add(cacheKey, acls, null, DateTime.Now.AddSeconds(5), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
            }
            return acls;
        }

        static object locker = new object();
    }
}