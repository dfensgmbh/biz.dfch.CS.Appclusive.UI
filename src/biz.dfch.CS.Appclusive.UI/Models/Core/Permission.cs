using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Permission : AppcusiveEntityViewModelBase
    {
        internal static List<Permission> GetPermissionsFromCache()
        {
            string cacheKey = "permission";
            List<Permission> permissions = (List<Permission>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
            if (null == permissions)
            {
                lock (locker)
                {
                    permissions = (List<Permission>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
                    if (null == permissions)
                    {
                        permissions = new List<Models.Core.Permission>();

                        biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
                        QueryOperationResponse<Api.Core.Permission> queryResponse = coreRepository.Permissions.AddQueryOption("$top", 10000).Execute() as QueryOperationResponse<Api.Core.Permission>;
                        while (null != queryResponse)
                        {
                            permissions.AddRange(AutoMapper.Mapper.Map<List<Models.Core.Permission>>(queryResponse.ToList()));
                            DataServiceQueryContinuation<Api.Core.Permission> cont = queryResponse.GetContinuation();
                            if (null != cont)
                            {
                                queryResponse = coreRepository.Execute<Api.Core.Permission>(cont) as QueryOperationResponse<Api.Core.Permission>;
                            }
                            else
                            {
                                queryResponse = null;
                            }
                        }

                        System.Web.HttpContext.Current.Cache.Add(cacheKey, permissions, null, DateTime.Now.AddSeconds(300), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
            }
            return permissions;
        }

        static object locker = new object();

    }
}