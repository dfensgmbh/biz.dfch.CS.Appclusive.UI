using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class AppcusiveEntityBase : IAppcusiveEntityBase
    {
        #region

        public DateTimeOffset Created { get; set; }

        public string CreatedBy { get; set; }

        public long Id { get; set; }

        public DateTimeOffset Modified { get; set; }

        public string ModifiedBy { get; set; }

        public byte[] RowVersion { get; set; }

        public string Tid { get; set; }

        #endregion

        [Required]
        [MaxLength(1024)]
        public string Name { get; set; }

        public string Description { get; set; }

    }
}