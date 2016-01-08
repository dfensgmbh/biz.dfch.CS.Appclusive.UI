using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class ContractMapping : AppcusiveEntityViewModelBase
    {
        [Required]
        [Display(Name = "ExternalType", ResourceType = typeof(GeneralResources))]
        public string ExternalType { get; set; }

        [Required]
        [Display(Name = "ExternalId", ResourceType = typeof(GeneralResources))]
        public string ExternalId { get; set; }

        [Display(Name = "IsPrimary", ResourceType = typeof(GeneralResources))]
        public bool IsPrimary { get; set; }

        [Required]
        [Display(Name = "ValidFrom", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidFrom { get; set; }

        [Required]
        [Display(Name = "ValidUntil", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidUntil { get; set; }

        [Required]
        [Display(Name = "CustomerId", ResourceType = typeof(GeneralResources))]
        public long CustomerId { get; set; }
        
        [Display(Name = "Customer", ResourceType = typeof(GeneralResources))]
        public Customer Customer { get; set; }
        
        [Display(Name = "Parameters", ResourceType = typeof(GeneralResources))]
        public string Parameters { get; set; }


        /// <summary>
        /// needed when the date is allowed to be MinValue and a Date-Picker is used
        /// </summary>
        [Display(Name = "ValidFrom", ResourceType = typeof(GeneralResources))]
        public DateTime ValidFromDateTime
        {
            get
            {
                return ValidFrom.ToDateTime();
            }
            set
            {
                ValidFrom = value.ToDateTimeOffset();
            }
        }
    }
}