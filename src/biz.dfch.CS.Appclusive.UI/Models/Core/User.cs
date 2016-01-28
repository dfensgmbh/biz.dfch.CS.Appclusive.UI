using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Client;
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