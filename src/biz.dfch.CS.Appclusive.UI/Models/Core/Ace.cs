using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Ace : AppcusiveEntityBase
    {
        [Required]
        [MaxLength(64)]
        public string Trustee { get; set; }

        private AceActionEnum _action;
        [Required]
        [MaxLength(64)]
        public string Action
        {
            get
            {
                return _action.ToString();
            }
            set
            {
                _action = (AceActionEnum)Enum.Parse(typeof(AceActionEnum), value, true);
            }
        }
    }
}