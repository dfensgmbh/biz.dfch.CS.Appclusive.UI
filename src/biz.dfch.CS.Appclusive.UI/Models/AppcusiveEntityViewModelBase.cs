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

        [Display(Name = "Id", ResourceType = typeof(GeneralResources))]
        public long Id { get; set; }

        [Display(Name = "Created", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset Created { get; set; }

        [Display(Name = "Modified", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset Modified { get; set; }

        [Display(Name = "RowVersion", ResourceType = typeof(GeneralResources))]
        public byte[] RowVersion { get; set; }

        [Display(Name = "Tid", ResourceType = typeof(GeneralResources))]
        public Guid Tid { get; set; }

        [Display(Name = "Tenant", ResourceType = typeof(GeneralResources))]
        public Tenant Tenant { get; set; }

        [Display(Name = "CreatedById", ResourceType = typeof(GeneralResources))]
        public long CreatedById { get; set; }

        [Display(Name = "CreatedBy", ResourceType = typeof(GeneralResources))]
        public User CreatedBy { get; set; }

        [Display(Name = "ModifiedById", ResourceType = typeof(GeneralResources))]
        public long ModifiedById { get; set; }

        [Display(Name = "ModifiedBy", ResourceType = typeof(GeneralResources))]
        public User ModifiedBy { get; set; }

        [Display(Name = "Name", ResourceType = typeof(GeneralResources))]
        public string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GeneralResources))]
        public string Description { get; set; }

        #endregion

    }
}