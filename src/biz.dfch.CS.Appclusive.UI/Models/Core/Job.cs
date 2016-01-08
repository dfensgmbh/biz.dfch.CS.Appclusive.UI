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
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Job : AppcusiveEntityViewModelBase
    {

        [Display(Name = "Condition", ResourceType = typeof(GeneralResources))]
        public string Condition { get; set; }

        [Display(Name = "ConditionParameters", ResourceType = typeof(GeneralResources))]
        public string ConditionParameters { get; set; }

        [Display(Name = "EndTime", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset? EndTime { get; set; }

        [Display(Name = "Error", ResourceType = typeof(GeneralResources))]
        public string Error { get; set; }

        [Display(Name = "Parameters", ResourceType = typeof(GeneralResources))]
        public string Parameters { get; set; }

        [Display(Name = "Parent", ResourceType = typeof(GeneralResources))]
        public Job Parent { get; set; }

        [Display(Name = "ParentId", ResourceType = typeof(GeneralResources))]
        public long? ParentId { get; set; }
        
        [Display(Name = "ReferencedItemId", ResourceType = typeof(GeneralResources))]
        public string RefId { get; set; }

        [Display(Name = "Status", ResourceType = typeof(GeneralResources))]
        public string Status { get; set; }

        [Display(Name = "TenantId", ResourceType = typeof(GeneralResources))]
        public string TenantId { get; set; }

        [Display(Name = "Token", ResourceType = typeof(GeneralResources))]
        public string Token { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityKindId", ResourceType = typeof(GeneralResources))]
        public long EntityKindId { get; set; }

        [Display(Name = "EntityKind", ResourceType = typeof(GeneralResources))]
        public EntityKind EntityKind { get; set; }
    }
}