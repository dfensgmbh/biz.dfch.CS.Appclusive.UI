using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.SpecialOperations
{
    public class SetTenantOperation : IOperationParams
    {
        public const string ACTION_NAME = "SetTenantOperation";

        public SetTenantOperation()
        {
            TenantId = string.Empty;
        }

        [Required]
        public string EntitySet { get; set; }

        [Required]
        public string EntityId { get; set; }

        [Required]
        public string TenantId { get; set; }


        public object GetRequestPramsObject()
        {
            return this;
        }
    }
}