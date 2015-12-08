using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class OrderItem : ViewModelBase, IAppcusiveEntityBase
    {
        public OrderItem()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }
        public CostCentre CostCentre { get; set; }
        
        public long? CostCentreId { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public Order Order { get; set; }

        [Display(Name = "Order id")]
        public long OrderId { get; set; }
        
        public string Parameters { get; set; }
        
        public int Quantity { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Tid { get; set; }
        
        public string Type { get; set; }
        
        public string Version { get; set; }
    }
}