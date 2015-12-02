using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Assoc : ViewModelBase, IAppcusiveEntityBase
    {
        public Assoc()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }
       
        public DateTimeOffset Created { get; set; }
       
        public string CreatedBy { get; set; }
       
        public string Description { get; set; }
       
        public Node Destination { get; set; }
       
        public long DestinationId { get; set; }
       
        public long Id { get; set; }
       
        public DateTimeOffset Modified { get; set; }
       
        public string ModifiedBy { get; set; }
       
        public string Name { get; set; }
       
        public long Order { get; set; }
       
        public byte[] RowVersion { get; set; }
       
        public Node Source { get; set; }
       
        public long SourceId { get; set; }
       
        public string Tid { get; set; }

    }
}