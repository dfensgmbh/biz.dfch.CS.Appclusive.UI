using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (this.AclId > 0)
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
            if (this.PermissionId > 0)
            {
                try
                {
                    this.Permission = Models.Core.Permission.GetPermissionsFromCache()
                         .FirstOrDefault(j => j.Id == this.PermissionId);
                }
                catch
                {
                    Contract.Assert(null != this.Permission, "no permission available");
                }
            }
            else
            {
                // all permissions
                this.Permission = new Permission() { Name = GeneralResources.PermissionsAll };
            }

            // Trustee
            if (this.TrusteeId > 0)
            {
                try
                {
                    if (TrusteeType == TrusteeTypeEnum.Role.GetHashCode())
                    {
                        Models.Core.Role role = Models.Core.Role.GetRolesFromCache()
                             .FirstOrDefault(j => j.Id == this.TrusteeId);
                        Contract.Assert(null != role, "no role available");
                        this.Trustee = role;
                    }
                    else
                    {
                        Models.Core.User user = Models.Core.User.GetUsersFromCache()
                             .FirstOrDefault(j => j.Id == this.TrusteeId);
                        Contract.Assert(null != user, "no user available");
                        this.Trustee = user;
                    }
                }
                catch
                {
                    Contract.Assert(null != this.Trustee, "no trustee available");
                }
            }
        }

    }
    
}