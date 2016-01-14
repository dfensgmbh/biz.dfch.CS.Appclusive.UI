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
    public class Node : AppcusiveEntityViewModelBase
    {
        public Node()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.IncomingAssocs = new List<Assoc>();
            this.OutgoingAssocs = new List<Assoc>();
            this.Children = new List<Node>();
        }

        [Display(Name = "Children", ResourceType = typeof(GeneralResources))]
        public List<Node> Children { get; set; }

        [Display(Name = "IncomingAssocs", ResourceType = typeof(GeneralResources))]
        public List<Assoc> IncomingAssocs { get; set; }

        [Display(Name = "OutgoingAssocs", ResourceType = typeof(GeneralResources))]
        public List<Assoc> OutgoingAssocs { get; set; }

        [Display(Name = "Parameters", ResourceType = typeof(GeneralResources))]
        public string Parameters { get; set; }

        [Display(Name = "Parent", ResourceType = typeof(GeneralResources))]
        public Node Parent { get; set; }

        [Display(Name = "ParentId", ResourceType = typeof(GeneralResources))]
        public long? ParentId { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityKindId", ResourceType = typeof(GeneralResources))]
        public long EntityKindId { get; set; }

        [Display(Name = "EntityKind", ResourceType = typeof(GeneralResources))]
        public EntityKind EntityKind { get; set; }

        [Display(Name = "EntityId", ResourceType = typeof(GeneralResources))]
        public long? EntityId { get; set; }

        [Display(Name = "Job", ResourceType = typeof(GeneralResources))]
        public Job Job { get; set; }

        /// <summary>
        /// Find Order by Approval 
        /// -> Job-Parent (Name = 'biz.dfch.CS.Appclusive.Core.OdataServices.Core.Approval') 
        /// </summary>
        /// <param name="coreRepository"></param>
        internal void ResolveJob(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            Api.Core.Job job = coreRepository.Jobs.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy")
                .Where(j => j.RefId == this.Id.ToString() && j.EntityKind.Version == EntityKind.VERSION_OF_Node)
                .FirstOrDefault();

            Contract.Assert(null != job, "no node-job available");
            this.Job = AutoMapper.Mapper.Map<Job>(job);
        }

    }
}