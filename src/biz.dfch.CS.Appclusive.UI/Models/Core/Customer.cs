using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Customer : AppcusiveEntityViewModelBase
    {
        public List<ContractMapping> ContractMappings { get; set; }

        public List<Tenant> Tenants { get; set; }

    }
}