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
using System.Reflection;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class EntityKind : AppcusiveEntityViewModelBase
    {
        public EntityKind()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }

        [Display(Name = "Parameters", ResourceType = typeof(GeneralResources))]
        public string Parameters { get; set; }

        [Display(Name = "Version", ResourceType = typeof(GeneralResources))]
        public string Version { get; set; }

        /// <summary>
        /// if there is a UI controller for this entity-kind then this property is not empty
        /// </summary>
        public string UiController
        {
            get
            {
                if (null == controllerName) {
                    controllerName = "";
                    if (!string.IsNullOrWhiteSpace(this.Version))
                    {
                        string localTypeName = this.Version.Split('.').Last();

                        if (!string.IsNullOrWhiteSpace(localTypeName))
                        {
                            Assembly modelAss = typeof(Models.Core.EntityKind).Assembly;

                            // find model
                            Type modelType = modelAss.GetTypes()
                                .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith("biz.dfch.CS.Appclusive.UI.Models") && t.Name == localTypeName)
                                .FirstOrDefault();

                            // find controller
                            if (null != modelType)
                            {
                                controllerName = localTypeName + "Controller";
                                Type controllerType = modelAss.GetTypes()
                                    .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith("biz.dfch.CS.Appclusive.UI.Controllers") && t.Name == controllerName)
                                    .FirstOrDefault();

                                if (null == controllerType)
                                {
                                    controllerName = localTypeName + "sController";
                                    controllerType = modelAss.GetTypes()
                                        .Where(t => !string.IsNullOrEmpty(t.Namespace) && t.Namespace.StartsWith("biz.dfch.CS.Appclusive.UI.Controllers") && t.Name == controllerName)
                                        .FirstOrDefault();
                                }

                                if (null == controllerType)
                                {
                                    controllerName = "";
                                }
                            }
                        }
                    }
                }
                return controllerName;
            }
        }
        string controllerName = null;
    }
}