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

using System;
using biz.dfch.CS.Appclusive.Public;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Node : AppcusiveEntityViewModelBase, IEntityReference
    {
        public Node()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.IncomingAssocs = new List<Assoc>();
            this.OutgoingAssocs = new List<Assoc>();
            this.Children = new List<Node>();
            this.EffectivAces = new List<Ace>();
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

        [Display(Name = "ExplicitAcl", ResourceType = typeof(GeneralResources))]
        public Acl Acl { get; set; }

        [Display(Name = "EffectivAces", ResourceType = typeof(GeneralResources))]
        public List<Ace> EffectivAces { get; set; }

        public string EntityName { get; set; }

        /// <summary>
        /// Find Order by Approval 
        /// -> Job-Parent (Name = 'biz.dfch.CS.Appclusive.Core.OdataServices.Core.Approval') 
        /// </summary>
        /// <param name="coreRepository"></param>
        internal void ResolveJob(Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            Api.Core.Job job = coreRepository.Jobs
                .Where(j => j.RefId == this.Id.ToString() && j.EntityKindId == Constants.EntityKindId.Node.GetHashCode())
                .FirstOrDefault();

            Contract.Assert(null != job, "No job available for this node");
            this.Job = AutoMapper.Mapper.Map<Job>(job);
        }

        internal void ResolveSecurity(out PagingFilterInfo explicitPagingFilterInfo, out PagingFilterInfo effectivePagingFilterInfo, Uri uri, int skip = 0, string itemSearchTerm = null, string orderBy = null)
        {
            explicitPagingFilterInfo = new PagingFilterInfo();
            effectivePagingFilterInfo = new PagingFilterInfo();

            if (biz.dfch.CS.Appclusive.UI.Navigation.PermissionDecisions.Current.CanRead(typeof(Acl)))
            {
                biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();

                // explicit permissions
                Api.Core.Acl acl = coreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy")
                    .Where(a => a.EntityId == this.Id && a.EntityKindId == Constants.EntityKindId.Node.GetHashCode())
                    .FirstOrDefault();

                if (null != acl)
                {
                    this.Acl = AutoMapper.Mapper.Map<Acl>(acl);
                    this.Acl.Aces = Models.Core.Acl.LoadAces(acl.Id, 1, out explicitPagingFilterInfo, uri, false);
                }

                // effectiv permissions
                this.EffectivAces = LoadEffectivePermissions(out effectivePagingFilterInfo, uri, this.Id, 1);
            }
        }
        internal static List<Ace> LoadEffectivePermissions(out PagingFilterInfo effectivePagingFilterInfo, Uri uri, long nodeId, int skip = 0, string itemSearchTerm = null, string orderBy = null)
        {
            List<Ace> aces;
            if (biz.dfch.CS.Appclusive.UI.Navigation.PermissionDecisions.Current.CanRead(typeof(Acl)))
            {
                biz.dfch.CS.Appclusive.Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.CoreRepositoryGet();
                
                // effectiv permissions
                var apiList = coreRepository.InvokeEntityActionWithListResult<Api.Core.Ace>("Nodes", nodeId, "GetEffectivePermissions", null);
                aces = AutoMapper.Mapper.Map<List<Models.Core.Ace>>(apiList);
                aces = Ace.SortAndFilter(aces, out effectivePagingFilterInfo, uri, skip, itemSearchTerm, orderBy);
            }
            else
            {
                effectivePagingFilterInfo = new PagingFilterInfo();
                aces = new List<Ace>();
            }
            return aces;
        }
    }
}