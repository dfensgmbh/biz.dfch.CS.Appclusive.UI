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
    public class Job : ViewModelBase, IAppcusiveEntityBase
    {
        
        public string Condition { get; set; }
        
        public string ConditionParameters { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public DateTimeOffset? EndTime { get; set; }
        
        public string Error { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public string Parameters { get; set; }
        
        public Job Parent { get; set; }

        [Display(Name = "ParentId", ResourceType = typeof(GeneralResources))]
        public long? ParentId { get; set; }
        
        [Display(Name = "ReferencedItemId", ResourceType = typeof(GeneralResources))]
        public string ReferencedItemId { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Status { get; set; }

        [Display(Name = "TenantId", ResourceType = typeof(GeneralResources))]
        public string TenantId { get; set; }
        
        public string Tid { get; set; }
        
        public string Token { get; set; }
    }
}