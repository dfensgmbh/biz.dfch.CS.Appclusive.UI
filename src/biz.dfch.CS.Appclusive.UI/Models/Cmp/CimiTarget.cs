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
using System.ComponentModel.DataAnnotations;
using biz.dfch.CS.Appclusive.UI.Models.Core;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Models.Cmp
{
    public class CimiTarget : AppcusiveEntityViewModelBase
    {
        public CimiTarget()
        {
            CimiId = "required";
            CimiType = "required";
            CatalogueItemId = 0;
        }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "CimiId", ResourceType = typeof(GeneralResources))] 
        public string CimiId { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "CimiType", ResourceType = typeof(GeneralResources))] 
        public string CimiType { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "CatalogueItemId", ResourceType = typeof(GeneralResources))] 
        public long CatalogueItemId { get; set; }

    }
}
