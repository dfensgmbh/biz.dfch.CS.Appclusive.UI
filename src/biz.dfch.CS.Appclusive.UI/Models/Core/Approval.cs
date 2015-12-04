using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Approval : ViewModelBase, IAppcusiveEntityBase
    {
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }

        [Display(Name = "Expires at")]
        public DateTimeOffset ExpiresAt { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }

        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        [Display(Name="Not before")]
        public DateTimeOffset NotBefore { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Status { get; set; }
        
        public string Tid { get; set; }

        #region approve/decline

        public const string DECLINED_STATUS_CHANGE = "Cancel";
        public const string APPROVED_STATUS_CHANGE = "Continue";
        public const string CREATED_STATUS = "Created";

        [Display(Name = "Help Text")]
        public string HelpText { get; set; }

        public string ActionText
        {
            get
            {
                return (this.Status == DECLINED_STATUS_CHANGE) ?
                    "Decline"
                    :
                    "Approve";
            }
        }

        #endregion
    }
}