using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Client;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Role : AppcusiveEntityViewModelBase
    {
        public Role()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.Permissions = new List<Permission>();
            this.Users = new List<User>();
        }

        [Display(Name = "Permissions", ResourceType = typeof(GeneralResources))]
        public List<Permission> Permissions { get; set; }

        [Display(Name = "Users", ResourceType = typeof(GeneralResources))]
        public List<User> Users { get; set; }

        [Display(Name = "MailAddress", ResourceType = typeof(GeneralResources))]
        public string MailAddress { get; set; }
        
        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "RoleType", ResourceType = typeof(GeneralResources))]
        public string RoleType
        {
            get
            {
                return _roleType.ToString();
            }
            set
            {
                _roleType = (RoleTypeEnum)Enum.Parse(typeof(RoleTypeEnum), value, true);
            }
        }
        private RoleTypeEnum _roleType;



        internal static List<Role> GetRolesFromCache()
        {
            string cacheKey = "roles";
            List<Role> acls = (List<Role>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
            if (null == acls)
            {
                lock (locker)
                {
                    acls = (List<Role>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
                    if (null == acls)
                    {
                        acls = new List<Models.Core.Role>();

                        biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
                        var query = coreRepository.Roles.AddQueryOption("$top", 10000);
                        QueryOperationResponse<Api.Core.Role> queryResponse = query.Execute() as QueryOperationResponse<Api.Core.Role>;
                        while (null != queryResponse)
                        {
                            acls.AddRange(AutoMapper.Mapper.Map<List<Models.Core.Role>>(queryResponse.ToList()));
                            DataServiceQueryContinuation<Api.Core.Role> cont = queryResponse.GetContinuation();
                            if (null != cont)
                            {
                                queryResponse = coreRepository.Execute<Api.Core.Role>(cont) as QueryOperationResponse<Api.Core.Role>;
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