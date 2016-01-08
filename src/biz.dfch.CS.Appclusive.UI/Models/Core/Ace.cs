using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Ace : AppcusiveEntityViewModelBase
    {
        [Required]
        [MaxLength(64)]
        [Display(Name = "Trustee", ResourceType = typeof(GeneralResources))] 
        public string Trustee { get; set; }

        private AceActionEnum _action;

        [Required]
        [MaxLength(64)]
        [Display(Name = "Action", ResourceType = typeof(GeneralResources))] 
        public string Action
        {
            get
            {
                return _action.ToString();
            }
            set
            {
                _action = (AceActionEnum)Enum.Parse(typeof(AceActionEnum), value, true);
            }
        }

        [Display(Name = "Acl", ResourceType = typeof(GeneralResources))] 
        public Acl Acl { get; set; }

        [Display(Name = "AclId", ResourceType = typeof(GeneralResources))] 
        public long AclId { get; set; }
    }
}