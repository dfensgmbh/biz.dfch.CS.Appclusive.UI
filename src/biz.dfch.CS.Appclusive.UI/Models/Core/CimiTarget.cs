using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class CimiTarget : AppcusiveEntityBase
    {
        public CimiTarget()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        [Required]
        public string CimiId { get; set; }

        [Required]
        public string CimiType { get; set; }

        [Required]
        public long CatalogueItemId { get; set; }

    }
}