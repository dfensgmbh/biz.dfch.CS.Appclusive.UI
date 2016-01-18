using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.SpecialOperations
{
    public class CheckPermission
    {
        [Display(Name = "Trustee", ResourceType = typeof(GeneralResources))]
        public IAppcusiveEntityBase Trustee { get; set; }

        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "TrusteeId", ResourceType = typeof(GeneralResources))]
        public long TrusteeId { get; set; }

        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
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

        [Display(Name = "Permission", ResourceType = typeof(GeneralResources))]
        public Permission Permission { get; set; }
        
        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "NodeId", ResourceType = typeof(GeneralResources))]
        public long NodeId { get; set; }

        public bool? Granted { get; set; }
        
        public string GrantedMessage { 
            get{
                return !this.Granted.HasValue?
                    "" :
                    this.Granted.Value ?
                    GeneralResources.Granted :
                    GeneralResources.Denied;
            }
        }
        

        internal void ResolveNavigationProperties(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            // Permission
            if (this.PermissionId > 0)
            {
                Api.Core.Permission permission = coreRepository.Permissions
                     .Where(j => j.Id == this.PermissionId)
                     .FirstOrDefault();
                Contract.Assert(null != permission, "no permission available");
                this.Permission = AutoMapper.Mapper.Map<Permission>(permission);
            }

            // Trustee
            if (this.TrusteeId > 0)
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
        }

    }
}