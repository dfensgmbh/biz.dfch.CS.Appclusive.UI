﻿/**
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
    public class Order : AppcusiveEntityViewModelBase
    {
        public Order()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.OrderItems = new List<OrderItem>();       
        }

        public CostCentre CostCentre { get; set; }

        public long? CostCentreId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public string Parameters { get; set; }

        public long? RequesterId { get; set; }

        public User Requester { get; set; }
        
        [Display(Name = "Status", ResourceType = typeof(GeneralResources))]
        public Job Job { get; set; }


        /// <summary>
        /// Find Job by Approval 
        /// -> Job-Parent (Name = 'biz.dfch.CS.Appclusive.Core.OdataServices.Core.Order') 
        /// </summary>
        /// <param name="coreRepository"></param>
        internal void ResolveJob(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            Api.Core.Job job = coreRepository.Jobs.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy")
                .Where(j => j.RefId == this.Id.ToString() && j.EntityKind.Version == EntityKind.VERSION_OF_Order)
                .FirstOrDefault();

            Contract.Assert(null != job, "no order-job available");
            this.Job = AutoMapper.Mapper.Map<Job>(job);
        }
    }
}