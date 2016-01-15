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
    public class Catalogue : AppcusiveEntityViewModelBase
    {
        public Catalogue()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.CatalogueItems = new List<CatalogueItem>();
        }

        public static string[] StatusSelection = { "Published", "Revoked", "Hidden" };

        [Display(Name = "CatalogueItems", ResourceType = typeof(GeneralResources))]
        public List<CatalogueItem> CatalogueItems { get; set; }

        [Required(ErrorMessageResourceName = "requiredStatus", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Status", ResourceType = typeof(GeneralResources))]
        public string Status { get; set; }

        [Required(ErrorMessageResourceName = "requiredVersion", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "Version", ResourceType = typeof(GeneralResources))]
        public string Version { get; set; }

    }
}