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
using System.Configuration;
using System.Linq;
using System.Text;

namespace biz.dfch.CS.Appclusive.UI.Config
{
    public static class SearchConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerName">classname</param>
        /// <returns></returns>
        public static EntityElement GetConfig(string controllerName)
        {
            string entityname = GetEntityNameFromControllerName(controllerName);
            if (!Configs.ContainsKey(entityname))
            {
                entityname = "default-values";
                #region add default values

                if (!Configs.ContainsKey(entityname))
                {
                    lock (SearchConfig.locker)
                    {
                        if (!Configs.ContainsKey(entityname))
                        {
                            //<add entityname="default-values" filter="substringof('{0}',Name)" display="Name" searchkey="Name" select="Id,Name"/>
                            Configs.Add(entityname, new EntityElement()
                                {
                                    EntityName = "default-values",
                                    Display = "{Name}",
                                    Filter = "substringof('{0}',Name)",
                                    SearchKey = "Name",
                                });
                        }
                    }
                }

                #endregion
            }
            return Configs[entityname];
        }

        internal static string GetEntityNameFromControllerName(string controllerName)
        {
            string entityname = controllerName;
            if (controllerName.EndsWith("Controller"))
            {
                entityname = entityname.Substring(0, entityname.Length - "Controller".Length);
                if (!Configs.ContainsKey(entityname) && entityname.EndsWith("s"))
                {
                    entityname = entityname.Substring(0, entityname.Length - 1);
                }
            }
            return entityname;
        }

        #region infrastructure


        static SearchConfig()
        {
            EntitySearchConfigurationSection configSection = (EntitySearchConfigurationSection)ConfigurationManager.GetSection(EntitySearchConfigurationSection.SectionName);
            Configs = new Dictionary<string, EntityElement>();
            foreach (EntityElement entitySearchConfig in configSection.Entities)
            {
                if (!String.IsNullOrWhiteSpace(entitySearchConfig.EntityName))
                {
                    Configs.Add(entitySearchConfig.EntityName, entitySearchConfig);
                }
            }
        }
        static Dictionary<string,EntityElement> Configs;

        static object locker = new object();

        #endregion
    }
}