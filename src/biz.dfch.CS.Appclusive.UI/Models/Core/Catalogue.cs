using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Catalogue : ViewModelBase, IAppcusiveEntityBase
    {
        public Catalogue()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.CatalogueItems = new List<CatalogueItem>();            
        }

        public static string[] StatusSelection = { "Published", "Revoked", "Hidden" };

        public List<CatalogueItem> CatalogueItems { get; set; }

        [Required(ErrorMessageResourceName = "requiredName", ErrorMessageResourceType = typeof(ErrorResources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "requiredStatus", ErrorMessageResourceType = typeof(ErrorResources))]
        public string Status { get; set; }

        [Required(ErrorMessageResourceName = "requiredVersion", ErrorMessageResourceType = typeof(ErrorResources))]
        public string Version { get; set; }
        
        public DateTimeOffset Created { get; set; }
        public string CreatedBy { get; set; }
        public string Description { get; set; }
        public long Id { get; set; }
        public DateTimeOffset Modified { get; set; }
        public string ModifiedBy { get; set; }
        public string Tid { get; set; }
        public byte[] RowVersion { get; set; }

    }
}