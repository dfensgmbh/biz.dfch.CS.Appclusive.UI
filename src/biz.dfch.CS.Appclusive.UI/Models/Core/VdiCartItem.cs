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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class VdiCartItem : CartItem
    {
        // same values like namne of the CatalogItem in the database
        public const string VDI_PERSONAL_NAME = "VDI Personal";
        public const string VDI_TECHNICAL_NAME = "VDI Technical";

        [Display(Name = "VdiName", ResourceType = typeof(GeneralResources))]
        public string VdiName { get; set; }

        [Display(Name = "HelpText", ResourceType = typeof(GeneralResources))]
        public string HelpText
        {
            get
            {
                switch (this.VdiName)
                {
                    case VDI_PERSONAL_NAME:
                        return GeneralResources.VDI_PERSONAL_HelpText;
                    case VDI_TECHNICAL_NAME:
                        return GeneralResources.VDI_TECHNICAL_HelpText;
                    default:
                        return GeneralResources.VDI_INVALID_HelpText;
                }
            }
        }

        [Display(Name = "Title", ResourceType = typeof(GeneralResources))]
        public string Title
        {
            get
            {
                switch (this.VdiName)
                {
                    case VDI_PERSONAL_NAME:
                        return "Add VDI for personal use";
                    case VDI_TECHNICAL_NAME:
                        return "Add VDI for another user";
                    default:
                        return "invalid VDI";
                }
            }
        }

        [Display(Name = "Requester", ResourceType = typeof(GeneralResources))]
        public User Requester { get; set; }

        [Display(Name = "Requester", ResourceType = typeof(GeneralResources))]
        public long RequesterId { get; set; }

        internal void ResolveRequester()
        {
            Api.Core.Core coreRepository = Navigation.PermissionDecisions.Current.GetCoreRepository();
            if (null == this.Requester && this.RequesterId>0)
            {
                this.Requester = AutoMapper.Mapper.Map<User>(coreRepository.Users.Where(o => o.Id == RequesterId).FirstOrDefault());
            }
        }

        internal string RequesterToParameters()
        {
            if (this.VdiName == VDI_TECHNICAL_NAME)
            {
                // {"RequesterId":"25"}
                var obj = new { RequesterId = this.RequesterId };
                string json = JsonConvert.SerializeObject(obj);
                return json;
            }
            else
            {
                return null;
            }
        }
    }
}