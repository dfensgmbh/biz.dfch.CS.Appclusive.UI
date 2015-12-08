using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Product : ViewModelBase, IAppcusiveEntityBase
    {
        
        public List<CatalogueItem> CatalogueItems { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "End of life")]
        public DateTimeOffset EndOfLife { get; set; }

        [Display(Name = "End of sale")]
        public DateTimeOffset EndOfSale { get; set; }

        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public string Parameters { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Tid { get; set; }
        
        public string Type { get; set; }

        [Display(Name = "Valid from")]
        public DateTimeOffset ValidFrom { get; set; }

        [Display(Name = "Valid until")]
        public DateTimeOffset ValidUntil { get; set; }
        
        public string Version { get; set; }

    }
}