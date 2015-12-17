using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.SpecialOperations
{
    public class SetCreatedByOperation : IOperationParams
    {
        public const string ACTION_NAME = "SetCreatedBy";

        [Required]
        public long EntityKindId { get; set; }

        [Required]
        public string EntityId { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        
        public object GetRequestPramsObject()
        {
            return this;
        }

    }
}