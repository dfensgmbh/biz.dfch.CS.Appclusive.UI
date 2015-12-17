using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class AppcusiveEntityViewModelBase : IAppcusiveEntityBase
    {
        public AppcusiveEntityViewModelBase()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        #region IAppcusiveEntityBase

        public long Id { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }

        public byte[] RowVersion { get; set; }

        [Required]
        public Guid Tid { get; set; }
        
        public Tenant Tenant { get; set; }
        
        [Required]
        public long CreatedById { get; set; }
        
        public User CreatedBy { get; set; }
        
        [Required]
        public long ModifiedById { get; set; }
        
        public User ModifiedBy { get; set; }
                
        #endregion

        [StringLength(1024)]
        [Required(ErrorMessageResourceName = "requiredName", ErrorMessageResourceType = typeof(ErrorResources))]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}