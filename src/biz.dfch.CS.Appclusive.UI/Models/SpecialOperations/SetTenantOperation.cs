using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.SpecialOperations
{
    public class SetTenantOperation : IOperationParams
    {
        public const string ACTION_NAME = "SetTenantOperation";

        public SetTenantOperation()
        {
            TenantId = string.Empty;
        }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntitySet", ResourceType = typeof(GeneralResources))]
        public string EntitySet { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityId", ResourceType = typeof(GeneralResources))]
        public string EntityId { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "TenantId", ResourceType = typeof(GeneralResources))]
        public string TenantId { get; set; }


        public object GetRequestPramsObject()
        {
            return this;
        }
    }
}