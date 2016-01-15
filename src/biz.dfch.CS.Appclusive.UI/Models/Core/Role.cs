using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}