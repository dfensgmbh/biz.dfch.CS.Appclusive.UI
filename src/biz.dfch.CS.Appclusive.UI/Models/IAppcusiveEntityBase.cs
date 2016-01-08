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
using biz.dfch.CS.Appclusive.UI.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public interface IAppcusiveEntityBase
    {
        [Display(Name = "Id", ResourceType = typeof(GeneralResources))]
        long Id { get; set; }

        [Display(Name = "Created", ResourceType = typeof(GeneralResources))]
        DateTimeOffset Created { get; set; }

        [Display(Name = "Modified", ResourceType = typeof(GeneralResources))]
        DateTimeOffset Modified { get; set; }

        [Display(Name = "RowVersion", ResourceType = typeof(GeneralResources))]
        byte[] RowVersion { get; set; }

        [Required]
        [Display(Name = "Tid", ResourceType = typeof(GeneralResources))]
        Guid Tid { get; set; }

        [Display(Name = "Tenant", ResourceType = typeof(GeneralResources))]
        Tenant Tenant { get; set; }

        [Required]
        [Display(Name = "CreatedById", ResourceType = typeof(GeneralResources))]
        long CreatedById { get; set; }

        [Display(Name = "CreatedBy", ResourceType = typeof(GeneralResources))]
        User CreatedBy { get; set; }

        [Required]
        [Display(Name = "ModifiedById", ResourceType = typeof(GeneralResources))]
        long ModifiedById { get; set; }

        [Display(Name = "ModifiedBy", ResourceType = typeof(GeneralResources))]
        User ModifiedBy { get; set; }

        [StringLength(1024)]
        [Required(ErrorMessageResourceName = "requiredName", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Name", ResourceType = typeof(GeneralResources))]
        string Name { get; set; }

        [Display(Name = "Description", ResourceType = typeof(GeneralResources))]
        string Description { get; set; }
    }
}
