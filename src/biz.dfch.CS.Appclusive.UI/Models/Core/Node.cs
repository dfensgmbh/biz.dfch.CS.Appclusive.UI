using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Node : ViewModelBase, IAppcusiveEntityBase
    {
        public Node()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.IncomingAssocs = new List<Assoc>();
            this.OutgoingAssocs = new List<Assoc>();
            this.Children = new List<Node>();
        }

        public List<Node> Children { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public long Id { get; set; }
        
        public List<Assoc> IncomingAssocs { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public List<Assoc> OutgoingAssocs { get; set; }
        
        public string Parameters { get; set; }
        
        public Node Parent { get; set; }

        [Display(Name = "Parent Id")]
        public long? ParentId { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Tid { get; set; }
        
        public string Type { get; set; }

    }
}