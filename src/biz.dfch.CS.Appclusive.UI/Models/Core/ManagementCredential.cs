using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class ManagementCredential : ViewModelBase, IAppcusiveEntityBase
    {
        public ManagementCredential()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.ManagementUris = new List<ManagementUri>();
        }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public string EncryptedPassword { get; set; }
        
        public long Id { get; set; }
        
        public List<ManagementUri> ManagementUris { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public string Password { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Tid { get; set; }
        
        public string Username { get; set; }
    }
}