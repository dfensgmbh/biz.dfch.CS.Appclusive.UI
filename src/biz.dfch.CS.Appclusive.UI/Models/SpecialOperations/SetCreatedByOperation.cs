using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.SpecialOperations
{
    public class SetCreatedByOperation : IOperationParams
    {
        public const string ACTION_NAME = "SetCreatedBy";

        [Required]
        [Display(Name = "EntitySet", ResourceType = typeof(GeneralResources))]
        public string EntitySet { get; set; }

        [Required]
        [Display(Name = "EntityId", ResourceType = typeof(GeneralResources))]
        public string EntityId { get; set; }

        [Required]
        [Display(Name = "CreatedBy", ResourceType = typeof(GeneralResources))] 
        public long CreatedById { get; set; }
        
        public object GetRequestPramsObject()
        {
            return this;
        }

    }
}