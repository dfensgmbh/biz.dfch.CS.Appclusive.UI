using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.Api.Core
{
    public class Customer : AppcusiveEntityViewModelBase
    {
        public Customer():base()
        {
            this.ContractMappings = new List<ContractMapping>();
            this.Tenants = new List<Tenant>();
        }

        public List<ContractMapping> ContractMappings { get; set; }

        public List<Tenant> Tenants { get; set; }

    }
}