using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Diagnostics
{
    public class Endpoint : ViewModelBase, IAppcusiveEntityBase
    {
        
        public string Address { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public int Priority { get; set; }
        
        public string RoutePrefix { get; set; }
        
        public string RouteTemplate { get; set; }
        
        public byte[] RowVersion { get; set; }

        [Display(Name = "ServerRole", ResourceType = typeof(GeneralResources))]
        public string ServerRole { get; set; }
        
        public string Tid { get; set; }
        
        public string Version { get; set; }
    }
}