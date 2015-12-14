using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class User : AppcusiveEntityBase
    {
        [Required]
        public string ExternalId { get; set; }
        [Required]
        public string Type { get; set; }

    }
}