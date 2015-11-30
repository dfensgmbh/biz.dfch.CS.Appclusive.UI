using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class CatalogueItem
    {

        
        public Catalogue Catalogue { get; set; }        
        public long CatalogueId { get; set; }        
        public DateTimeOffset Created { get; set; }        
        public string CreatedBy { get; set; }        
        public string Description { get; set; }        
        public DateTimeOffset EndOfLife { get; set; }
                public DateTimeOffset EndOfSale { get; set; }
                public long Id { get; set; }
                public DateTimeOffset Modified { get; set; }
                public string ModifiedBy { get; set; }
                public string Name { get; set; }
                public string Parameters { get; set; }
                public Product Product { get; set; }
                public long ProductId { get; set; }
                public byte[] RowVersion { get; set; }
                public string Tid { get; set; }
                public DateTimeOffset ValidFrom { get; set; }
                public DateTimeOffset ValidUntil { get; set; }
    }
}