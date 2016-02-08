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
    public class Ace : AppcusiveEntityViewModelBase
    {
        [Display(Name = "Trustee", ResourceType = typeof(GeneralResources))]
        public IAppcusiveEntityBase Trustee { get; set; }

        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "TrusteeId", ResourceType = typeof(GeneralResources))]
        public long TrusteeId { get; set; }

        [Range(0, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "PermissionId", ResourceType = typeof(GeneralResources))]
        public long PermissionId { get; set; }

        [Display(Name = "TrusteeType", ResourceType = typeof(GeneralResources))]
        public long TrusteeType { get; set; }

        [Display(Name = "TrusteeType", ResourceType = typeof(GeneralResources))]
        public string TrusteeTypeStr
        {
            get
            {
                return Enum.GetName(typeof(TrusteeTypeEnum), TrusteeType);
            }
        }

        [Display(Name = "Type", ResourceType = typeof(GeneralResources))]
        public long Type { get; set; }

        [Display(Name = "Type", ResourceType = typeof(GeneralResources))]
        public string TypeStr
        {
            get
            {
                return Enum.GetName(typeof(AceTypeEnum), Type);
            }
        }

        [Display(Name = "Acl", ResourceType = typeof(GeneralResources))]
        public Acl Acl { get; set; }

        [Display(Name = "AclId", ResourceType = typeof(GeneralResources))]
        public long AclId { get; set; }

        [Display(Name = "Permission", ResourceType = typeof(GeneralResources))]
        public Permission Permission { get; set; }

        public string CssClass
        {
            get
            {
                if (AceTypeEnum.ALARM.GetHashCode() == this.Type)
                {
                    return "warning";
                }
                if (AceTypeEnum.ALLOW.GetHashCode() == this.Type)
                {
                    return "success";
                }
                if (AceTypeEnum.AUDIT.GetHashCode() == this.Type)
                {
                    return "info";
                }
                if (AceTypeEnum.DENY.GetHashCode() == this.Type)
                {
                    return "danger";
                }
                return string.Empty;
            }
        }

        internal void ResolveNavigationProperties(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository, Models.Core.Acl acl = null)
        {
            Contract.Requires(null != coreRepository);

            // ACL
            if (this.AclId > 0 && this.Acl == null)
            {
                if (null == acl)
                {
                    try
                    {
                        acl = Models.Core.Acl.GetAclsFromCache()
                             .FirstOrDefault(j => j.Id == this.AclId);
                    }
                    catch
                    {
                        Contract.Assert(null != acl, "no acl available");
                    }
                }
                this.Acl = acl;
            }

            // Permission
            if (this.Permission == null)
            {
                if (this.PermissionId > 0)
                {
                    try
                    {
                        this.Permission = Models.Core.Permission.GetPermissionsFromCache()
                             .FirstOrDefault(j => j.Id == this.PermissionId);
                    }
                    catch
                    {
                        //   Contract.Assert(null != this.Permission, "no permission available");
                    }
                }
                else
                {
                    // all permissions
                    this.Permission = new Permission() { Name = GeneralResources.PermissionsAll };
                }
            }

            // Trustee
            if (this.TrusteeId > 0 && this.Trustee == null)
            {
                try
                {
                    if (TrusteeType == TrusteeTypeEnum.Role.GetHashCode())
                    {
                        Models.Core.Role role = Models.Core.Role.GetRolesFromCache()
                             .FirstOrDefault(j => j.Id == this.TrusteeId);
                        //Contract.Assert(null != role, "no role available");
                        this.Trustee = role;
                    }
                    else
                    {
                        Models.Core.User user = Models.Core.User.GetUsersFromCache()
                             .FirstOrDefault(j => j.Id == this.TrusteeId);
                        //Contract.Assert(null != user, "no user available");
                        this.Trustee = user;
                    }
                }
                catch
                {
                    // Contract.Assert(null != this.Trustee, "no trustee available");
                }
            }
        }

        internal static List<Ace> GetAcesFromCache(long aclId)
        {
            string cacheKey = "aces_" + aclId.ToString();
            List<Ace> aces = (List<Ace>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
            if (null == aces)
            {
                lock (locker)
                {
                    aces = (List<Ace>)System.Web.HttpContext.Current.Cache.Get(cacheKey);
                    if (null == aces)
                    {
                        aces = new List<Models.Core.Ace>();

                        biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
                        var query = coreRepository.Aces.AddQueryOption("$top", 10000);
                        if (aclId > 0)
                        {
                            query = query.AddQueryOption("$filter", "AclId eq " + aclId);
                        }
                        QueryOperationResponse<Api.Core.Ace> queryResponse = query.Execute() as QueryOperationResponse<Api.Core.Ace>;
                        while (null != queryResponse)
                        {
                            aces.AddRange(AutoMapper.Mapper.Map<List<Models.Core.Ace>>(queryResponse.ToList()));
                            DataServiceQueryContinuation<Api.Core.Ace> cont = queryResponse.GetContinuation();
                            if (null != cont)
                            {
                                queryResponse = coreRepository.Execute<Api.Core.Ace>(cont) as QueryOperationResponse<Api.Core.Ace>;
                            }
                            else
                            {
                                queryResponse = null;
                            }
                        }

                        System.Web.HttpContext.Current.Cache.Add(cacheKey, aces, null, DateTime.Now.AddSeconds(5), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
            }
            return aces;
        }

        static object locker = new object();

        internal static List<Ace> SortAndFilter(List<Ace> allAces, out PagingInfo pagingInfo, int pageNr = 1, string itemSearchTerm = null, string orderBy = null, bool distinct = false)
        {
            IEnumerable<Models.Core.Ace> aces;
            int itemCount = 0;

            biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
            if (string.IsNullOrEmpty(itemSearchTerm) && string.IsNullOrEmpty(orderBy))
            {
                // paging only
                aces = allAces.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize);
                foreach (var ace in aces)
                {
                    ace.ResolveNavigationProperties(coreRepository);
                }
                itemCount = allAces.Count;
            }
            else
            {
                // search or order by and paging 
                foreach (var ace in allAces)
                {
                    ace.ResolveNavigationProperties(coreRepository);
                }
                if (!string.IsNullOrEmpty(itemSearchTerm))
                {
                    aces = allAces.Where(a => a.TypeStr.ToLower().Contains(itemSearchTerm.ToLower())
                        || (a.Trustee != null && a.Trustee.Name.ToLower().Contains(itemSearchTerm.ToLower()))
                        || (a.Permission != null && a.Permission.Name.ToLower().Contains(itemSearchTerm.ToLower()))
                    );
                }
                switch (orderBy)
                {
                    case "Type": aces = allAces.OrderBy(a => a.TypeStr); break;
                    case "Type desc": aces = allAces.OrderByDescending(a => a.TypeStr); break;
                    case "Trustee": aces = allAces.OrderBy(a => a.Trustee != null ? a.Trustee.Name : ""); break;
                    case "Trustee desc": aces = allAces.OrderByDescending(a => a.Trustee != null ? a.Trustee.Name : ""); break;
                    case "Permission": aces = allAces.OrderBy(a => a.Permission != null ? a.Permission.Name : ""); break;
                    case "Permission desc": aces = allAces.OrderByDescending(a => a.Permission != null ? a.Permission.Name : ""); break;
                    default: aces = allAces.OrderBy(a => a.Type); break;
                }
                if (distinct)
                {
                    aces = aces.Distinct(new Models.Core.AceSearchViewComparer());
                }
                itemCount = aces.Count();
                aces = aces.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize);
            }
            pagingInfo = new PagingInfo(pageNr, itemCount);
            return aces.ToList();
        }
    }
    
}