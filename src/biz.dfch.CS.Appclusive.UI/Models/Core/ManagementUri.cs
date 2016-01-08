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
    public class ManagementUri : AppcusiveEntityViewModelBase
    {
        public ManagementUri()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        [Display(Name = "ManagementCredential", ResourceType = typeof(GeneralResources))]
        public ManagementCredential ManagementCredential { get; set; }

        [Display(Name = "ManagementCredentialId", ResourceType = typeof(GeneralResources))]
        public long? ManagementCredentialId { get; set; }

        [Display(Name = "Type", ResourceType = typeof(GeneralResources))]
        public string Type { get; set; }

        [Display(Name = "ValueDisplay", ResourceType = typeof(GeneralResources))]
        public string Value { get; set; }

        public string[] TypeSelection
        {
            get
            {
                List<string> list = new List<string>() { "json", "string" };
                if (null == this.Type || !list.Contains(this.Type))
                {
                    list.Add(this.Type);
                }
                return list.ToArray();
            }
        }
    }
}