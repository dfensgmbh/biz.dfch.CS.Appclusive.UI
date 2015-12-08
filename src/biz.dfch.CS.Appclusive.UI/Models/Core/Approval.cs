using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Approval : ViewModelBase, IAppcusiveEntityBase
    {
        public Approval()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

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

        /// <summary>
        /// set through call of ResolveOrderId()
        /// </summary>
        public int OrderId { get; private set; }

        /// <summary>
        /// Find Order by Approval 
        /// -> Job-Parent (Name = 'biz.dfch.CS.Appclusive.Core.OdataServices.Core.Approval') 
        /// -> Job (Name== 'biz.dfch.CS.Appclusive.Core.OdataServices.Core.Order') 
        /// -> Order
        /// </summary>
        /// <param name="coreRepository"></param>
        internal void ResolveOrderId(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            var jobs = coreRepository.Jobs.Where(j => j.Name == "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Approval" & j.ReferencedItemId == this.Id.ToString());
            Api.Core.Job approvalJob = jobs.FirstOrDefault();
            Contract.Assert(null != approvalJob, "no approval-job available");
            Contract.Assert(approvalJob.ParentId.HasValue, "no approval-job parent available");

            jobs = coreRepository.Jobs.Where(j => j.Id == approvalJob.ParentId.Value && j.Name == "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Order");
            Api.Core.Job orderJob = jobs.FirstOrDefault();
            Contract.Assert(null != orderJob, "no Order-job available");

            int orderId = 0;
            int.TryParse(orderJob.ReferencedItemId, out orderId);
            this.OrderId = orderId;
        }

        public string OrderIdClass {
            get { return this.OrderId > 0 ? "" : " disabled"; }
        }
    }
}