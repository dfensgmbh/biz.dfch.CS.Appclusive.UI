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
        long Id { get; set; }

        DateTimeOffset Created { get; set; }
        DateTimeOffset Modified { get; set; }

        byte[] RowVersion { get; set; }

        [Required]
        Guid Tid { get; set; }

        Tenant Tenant { get; set; }

        [Required]
        long CreatedById { get; set; }

        [Display(Name = "CreatedBy", ResourceType = typeof(GeneralResources))]
        User CreatedBy { get; set; }

        [Required]
        long ModifiedById { get; set; }

        [Display(Name = "ModifiedBy", ResourceType = typeof(GeneralResources))]
        User ModifiedBy { get; set; }

        [StringLength(1024)]
        [Required(ErrorMessageResourceName = "requiredName", ErrorMessageResourceType = typeof(ErrorResources))]
        string Name { get; set; }

        string Description { get; set; }
    }
}
