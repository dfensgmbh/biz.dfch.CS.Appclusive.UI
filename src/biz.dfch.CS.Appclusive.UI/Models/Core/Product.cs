using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Product : ViewModelBase, IAppcusiveEntityBase
    {

        public Product()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.CatalogueItems = new List<CatalogueItem>();
        }

        public List<CatalogueItem> CatalogueItems { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "EndOfLife", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset EndOfLife { get; set; }

        [Display(Name = "EndOfSale", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset EndOfSale { get; set; }

        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public string Parameters { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Tid { get; set; }

        [Required]
        public string Type { get; set; }

        [DataType("DateTime")]
        [Display(Name = "ValidFrom", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidFrom { get; set; }

        [Display(Name = "ValidUntil", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidUntil { get; set; }
        
        public string Version { get; set; }

    }
}