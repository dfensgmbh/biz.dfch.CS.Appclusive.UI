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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class EntityKind : AppcusiveEntityViewModelBase
    {
        #region static and constant

        public const string VERSION_OF_Approval = "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Approval";
        public const string VERSION_OF_Job = "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Job";
        public const string VERSION_OF_Node = "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Node";
        public const string VERSION_OF_Order = "biz.dfch.CS.Appclusive.Core.OdataServices.Core.Order";

        public static long GetId(string version, Api.Core.Core coreRepository){
            if (!idCache.ContainsKey(version))
            {
                lock (idCache)
                {
                    if (!idCache.ContainsKey(version))
                    {
                        var ekind = coreRepository.EntityKinds.Where(e => e.Version == version).FirstOrDefault();
                        Contract.Assert(null==ekind);
                        idCache.Add(version, ekind.Id);
                    }
                }
            }
            return idCache[version];
        }
        static Dictionary<string, long> idCache = new Dictionary<string, long>();

        #endregion

        public EntityKind()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        [Display(Name = "Parameters", ResourceType = typeof(GeneralResources))]
        public string Parameters { get; set; }

        [Display(Name = "Version", ResourceType = typeof(GeneralResources))]
        public string Version { get; set; }

    }
}