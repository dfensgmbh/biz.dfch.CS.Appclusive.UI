using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Acl : AppcusiveEntityViewModelBase, IEntityReference
    {
        public Acl() : base()
        {
            this.Aces = new List<Ace>();
        }

        [Display(Name = "Aces", ResourceType = typeof(GeneralResources))] 
        public List<Ace> Aces { get; set; }

        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityId", ResourceType = typeof(GeneralResources))]
        public long? EntityId { get; set; }

        //[Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Range(1, 1, ErrorMessageResourceName = "onlyNodeAllowed", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityKindId", ResourceType = typeof(GeneralResources))] 
        public long EntityKindId { get; set; }

        [Display(Name = "EntityKind", ResourceType = typeof(GeneralResources))] 
        public EntityKind EntityKind { get; set; }

        [Display(Name = "NoInheritanceFromParent", ResourceType = typeof(GeneralResources))]
        public bool NoInheritanceFromParent { get; set; }

        /// <param name="coreRepository"></param>
        internal void ResolveNavigationProperties(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            // EntityKind
            if (this.EntityKindId > 0)
            {
                Api.Core.EntityKind entityKind = coreRepository.EntityKinds
                     .Where(j => j.Id == this.EntityKindId)
                     .FirstOrDefault();
                Contract.Assert(null != entityKind, "no entityKind available");
                this.EntityKind = AutoMapper.Mapper.Map<EntityKind>(entityKind);
            }

        }

        public string EntityName { get; set; }


        internal static List<Acl> GetAclsFromCache()
        {
            string cacheKey = "acls";
            List<Acl> acls = (List<Acl>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
            if (null == acls)
            {
                lock (locker)
                {
                    acls = (List<Acl>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
                    if (null == acls)
                    {
                        acls = new List<Models.Core.Acl>();

                        biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
                        QueryOperationResponse<Api.Core.Acl> queryResponse = coreRepository.Acls.AddQueryOption("$top", 10000).Execute() as QueryOperationResponse<Api.Core.Acl>;
                        while (null != queryResponse)
                        {
                            acls.AddRange(AutoMapper.Mapper.Map<List<Models.Core.Acl>>(queryResponse.ToList()));
                            DataServiceQueryContinuation<Api.Core.Acl> cont = queryResponse.GetContinuation();
                            if (null != cont)
                            {
                                queryResponse = coreRepository.Execute<Api.Core.Acl>(cont) as QueryOperationResponse<Api.Core.Acl>;
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

        internal static List<Models.Core.Ace> LoadAces(long aclId, int skip, out PagingFilterInfo pagingFilterInfo, Uri uri, bool distinct = false, string itemSearchTerm = null, string orderBy = null)
        {
            List<Ace> allAces = Ace.GetAcesFromCache(aclId);

            IEnumerable<Models.Core.Ace> aces;
            if (allAces == null)
            {
                aces = new List<Ace>();
                pagingFilterInfo = new PagingFilterInfo();
            }
            else
            {
                aces = Ace.SortAndFilter(allAces, out pagingFilterInfo, uri, skip, itemSearchTerm, orderBy, distinct);
            }
            return aces.ToList();
        }
    }
}