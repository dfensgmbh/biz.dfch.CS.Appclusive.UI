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

namespace biz.dfch.CS.Appclusive.UI.Models.Diagnostics
{
    public class AuditTrail : AppcusiveEntityViewModelBase
    {

        [Display(Name = "Current", ResourceType = typeof(GeneralResources))] 
        public string Current { get; set; }

        [Display(Name = "EntityId",ResourceType = typeof(GeneralResources))] 
        public string EntityId { get; set; }

        [Display(Name = "EntityState", ResourceType = typeof(GeneralResources))] 
        public string EntityState { get; set; }

        [Display(Name = "EntityType", ResourceType = typeof(GeneralResources))] 
        public string EntityType { get; set; }

        [Display(Name = "Original", ResourceType = typeof(GeneralResources))] 
        public string Original { get; set; }

        /// <summary>
        /// Gets creted by method ParseChangedPropertyTable()
        /// </summary>
        [Display(Name = "ChangedPropertiesTable", ResourceType = typeof(GeneralResources))]
        public Dictionary<string, object[]> ChangedPropertiesTable { get; set; }

        public void ParseChangedPropertyTable()
        {
            ChangedPropertiesTable = new Dictionary<string, object[]>();

            try
            {
                if (!string.IsNullOrWhiteSpace(this.Original))
                {
                    Newtonsoft.Json.Linq.JObject before = Newtonsoft.Json.Linq.JObject.Parse(this.Original);
                    foreach (var prop in before.Properties())
                    {
                        if (!ChangedPropertiesTable.ContainsKey(prop.Name))
                        {
                            ChangedPropertiesTable.Add(prop.Name, new object[2]);
                        }
                        ChangedPropertiesTable[prop.Name][0] = prop.Value;
                    }
                }
            }
            catch { }

            try
            {
                if (!string.IsNullOrWhiteSpace(this.Current))
                {
                    Newtonsoft.Json.Linq.JObject after = Newtonsoft.Json.Linq.JObject.Parse(this.Current);
                    foreach (var prop in after.Properties())
                    {
                        if (!ChangedPropertiesTable.ContainsKey(prop.Name))
                        {
                            ChangedPropertiesTable.Add(prop.Name, new object[2]);
                        }
                        ChangedPropertiesTable[prop.Name][1] = prop.Value;
                    }
                }
            }
            catch { }

        }
    }
}