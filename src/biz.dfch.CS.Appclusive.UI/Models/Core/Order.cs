using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Order : ViewModelBase, IAppcusiveEntityBase
    {
        public Order()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.OrderItems = new List<OrderItem>();
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
        
        public List<OrderItem> OrderItems { get; set; }
        
        public string Parameters { get; set; }
        
        public string Requester { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Status { get; set; }
        
        public string Tid { get; set; }

    }
}