using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class LoginData
    {
        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Username", ResourceType = typeof(GeneralResources))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Password", ResourceType = typeof(GeneralResources))]
        public string Password { get; set; }

        //[Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Domain", ResourceType = typeof(GeneralResources))]
        public string Domain { get; set; }

        [Display(Name = "ReturnUrl", ResourceType = typeof(GeneralResources))]
        public string ReturnUrl { get; set; }
    }
}