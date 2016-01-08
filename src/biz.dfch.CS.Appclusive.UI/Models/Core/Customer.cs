using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Customer : AppcusiveEntityViewModelBase
    {
        [Display(Name = "ContractMappings", ResourceType = typeof(GeneralResources))]
        public List<ContractMapping> ContractMappings { get; set; }

        [Display(Name = "Tenants", ResourceType = typeof(GeneralResources))]
        public List<Tenant> Tenants { get; set; }

    }
}