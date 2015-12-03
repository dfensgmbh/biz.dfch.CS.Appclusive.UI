using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class CartItem : ViewModelBase, IAppcusiveEntityBase
    {
       
        public Cart Cart { get; set; }
       
        public long CartId { get; set; }
       
        public CatalogueItem CatalogueItem { get; set; }
       
        public long CatalogueItemId { get; set; }
       
        public DateTimeOffset Created { get; set; }
       
        public string CreatedBy { get; set; }
       
        public string Description { get; set; }
       
        public long Id { get; set; }
       
        public DateTimeOffset Modified { get; set; }
       
        public string ModifiedBy { get; set; }
       
        public string Name { get; set; }
       
        public string Parameters { get; set; }
       
        public int Quantity { get; set; }
       
        public byte[] RowVersion { get; set; }
       
        public string Tid { get; set; }
    }
}