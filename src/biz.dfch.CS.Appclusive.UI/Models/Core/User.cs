using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    }
}