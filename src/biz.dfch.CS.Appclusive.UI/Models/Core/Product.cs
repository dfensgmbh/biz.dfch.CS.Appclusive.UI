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
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Product : AppcusiveEntityViewModelBase
    {

        public Product()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.CatalogueItems = new List<CatalogueItem>();
        }

        [Display(Name = "CatalogueItems", ResourceType = typeof(GeneralResources))]
        public List<CatalogueItem> CatalogueItems { get; set; }
        
        [Display(Name = "EndOfLife", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset EndOfLife { get; set; }

        [Display(Name = "Parameters", ResourceType = typeof(GeneralResources))]
        public string Parameters { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Type", ResourceType = typeof(GeneralResources))]
        public string Type { get; set; }

        [Display(Name = "ValidFrom", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidFrom { get; set; }

        /// <summary>
        /// needed when the date is allowed to be MinValue and a Date-Picker is used
        /// </summary>
        [Display(Name = "ValidFrom", ResourceType = typeof(GeneralResources))]
        public DateTime ValidFromDateTime {
            get
            {
                return ValidFrom.ToDateTime();
            }
            set
            {
                ValidFrom = value.ToDateTimeOffset();
            } 
        }

        [Display(Name = "ValidUntil", ResourceType = typeof(GeneralResources))]
        public DateTimeOffset ValidUntil { get; set; }

        [Required(ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityKindId", ResourceType = typeof(GeneralResources))]
        public long EntityKindId { get; set; }

        [Display(Name = "EntityKind", ResourceType = typeof(GeneralResources))]
        public EntityKind EntityKind { get; set; }
        
    }
}