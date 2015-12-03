using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Gate : ViewModelBase, IAppcusiveEntityBase
    {
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public string Parameters { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Status { get; set; }
        
        public string Tid { get; set; }
        
        public string Type { get; set; }

    }
}