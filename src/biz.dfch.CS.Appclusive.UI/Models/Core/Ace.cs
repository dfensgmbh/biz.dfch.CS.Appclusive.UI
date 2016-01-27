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

        internal void ResolveNavigationProperties(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            // ACL
            if (this.AclId > 0)
            {
                Api.Core.Acl acl=null;
                try
                {
                    acl = coreRepository.Acls
                         .Where(j => j.Id == this.AclId)
                         .FirstOrDefault();
                }
                catch 
                {
                    Contract.Assert(null != acl, "no acl available");
                }
                this.Acl = AutoMapper.Mapper.Map<Acl>(acl);
            }

            // Permission
            if (this.PermissionId > 0)
            {
                Api.Core.Permission permission = null;
                try
                {
                    permission = coreRepository.Permissions
                         .Where(j => j.Id == this.PermissionId)
                         .FirstOrDefault();
                }
                catch
                {
                    Contract.Assert(null != permission, "no permission available");
                }
                this.Permission = AutoMapper.Mapper.Map<Permission>(permission);
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
                        Api.Core.Role role = coreRepository.Roles
                             .Where(j => j.Id == this.TrusteeId)
                             .FirstOrDefault();
                        Contract.Assert(null != role, "no role available");
                        this.Trustee = AutoMapper.Mapper.Map<Role>(role);
                    }
                    else
                    {
                        Api.Core.User user = coreRepository.Users
                             .Where(j => j.Id == this.TrusteeId)
                             .FirstOrDefault();
                        Contract.Assert(null != user, "no user available");
                        this.Trustee = AutoMapper.Mapper.Map<User>(user);
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