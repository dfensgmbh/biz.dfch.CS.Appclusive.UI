using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class CatalogueItem : ViewModelBase, IAppcusiveEntityBase
    {
        public CatalogueItem()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        public Catalogue Catalogue { get; set; }
        public long CatalogueId { get; set; }
        public DateTimeOffset Created { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }

        [Display(Name = "EndOfLife", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset EndOfLife { get; set; }

        [Display(Name = "EndOfSale", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset EndOfSale { get; set; }

        public long Id { get; set; }
        public DateTimeOffset Modified { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }
        public byte[] RowVersion { get; set; }
        public string Tid { get; set; }

        [Display(Name = "ValidFrom", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidFrom { get; set; }

        [Display(Name = "ValidUntil", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidUntil { get; set; }
    }
}