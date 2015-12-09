using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Diagnostics
{
    public class AuditTrail : ViewModelBase, IAppcusiveEntityBase
    {
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Current { get; set; }
        
        public string Description { get; set; }
        
        public string EntityId { get; set; }
        
        public string EntityState { get; set; }
        
        public string EntityType { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public string Original { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Tid { get; set; }

    }
}