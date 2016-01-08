/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Approval : AppcusiveEntityViewModelBase
    {
        public Approval()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        [Display(Name = "ExpiresAt", ResourceType = typeof(GeneralResources))] 
        public DateTimeOffset ExpiresAt { get; set; }
        
        [Display(Name = "NotBefore", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset NotBefore { get; set; }

        [Display(Name = "Status", ResourceType = typeof(GeneralResources))]
        public string Continue { get; set; }

        [Display(Name = "Status", ResourceType = typeof(GeneralResources))]
        public Job Job { get; set; }
        
        #region approve/decline

        public const string DECLINED_STATUS_CHANGE = "Cancel";
        public const string APPROVED_STATUS_CHANGE = "Continue";
        public const string CREATED_STATUS = "Created";

        [Display(Name = "HelpText", ResourceType = typeof(GeneralResources))]
        public string HelpText { get; set; }

        [Display(Name = "ActionText", ResourceType = typeof(GeneralResources))] 
        public string ActionText
        {
            get
            {
                if (null == this.Job) return ErrorResources.NoJobAvailable;
                return (this.Job.Status == DECLINED_STATUS_CHANGE) ?
                    GeneralResources.Decline
                    :
                    GeneralResources.Approve;
            }
        }

        #endregion

        /// <summary>
        /// set through call of ResolveOrderId()
        /// </summary>
        [Display(Name = "OrderId", ResourceType = typeof(GeneralResources))] 
        public int OrderId { get; private set; }
        
        /// <summary>
        /// Find Order by Approval 
        /// -> Job-Parent (Name = 'biz.dfch.CS.Appclusive.Core.OdataServices.Core.Approval') 
        /// </summary>
        /// <param name="coreRepository"></param>
        internal void ResolveJob(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            Api.Core.Job job = coreRepository.Jobs.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy")
                .Where(j => j.RefId == this.Id.ToString() && j.EntityKind.Version == EntityKind.VERSION_OF_Approval)
                .FirstOrDefault();

            Contract.Assert(null != job, "no approval-job available");
            this.Job = AutoMapper.Mapper.Map<Job>(job);
        }

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

            if (null == this.Job)
            {
                this.ResolveJob(coreRepository);
            }

            Api.Core.Job orderJob = coreRepository.Jobs.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy")
                .Where(j => j.RefId == this.Id.ToString() && j.EntityKind.Version == EntityKind.VERSION_OF_Order)
                .FirstOrDefault();

            Contract.Assert(null != orderJob, "no Order-job available");

            int orderId = 0;
            int.TryParse(orderJob.RefId, out orderId);
            this.OrderId = orderId;
        }

        public string OrderIdClass {
            get { return this.OrderId > 0 ? "" : " disabled"; }
        }
    }
}