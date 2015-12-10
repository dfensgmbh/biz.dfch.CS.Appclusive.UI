using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.Api.Core
{
    public class ContractMapping : AppcusiveEntityBase
    {
        public ContractMapping()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }
        [Required]
        public string ExternalType { get; set; }

        [Required]
        public string ExternalId { get; set; }

        public bool IsPrimary { get; set; }

        [Required]
        public DateTimeOffset ValidFrom { get; set; }

        [Required]
        public DateTimeOffset ValidUntil { get; set; }

        [Required]
        public long CustomerId { get; set; }

        public Customer Customer { get; set; }

        public string Parameters { get; set; }

    }
}