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
using System.Configuration;

namespace biz.dfch.CS.Appclusive.UI.Navigation
{
    public class NavEntryElement : ConfigurationElement
    {
        private const string NameAttribute = "name";
        private const string ActionAttribute = "action";
        private const string ControllerAttribute = "controller";
        private const string IconAttribute = "icon";
        private const string PermissionAttribute = "permission";
        private const string EntriesNode = "navigationEntries";

        /// <summary>
        /// Name
        /// </summary>
        [ConfigurationProperty(NameAttribute, IsRequired = true)]
        public string Name
        {
            get { return Convert.ToString(this[NameAttribute]); }
            set { this[NameAttribute] = value; }
        }

        /// <summary>
        /// Controller action
        /// </summary>
        [ConfigurationProperty(ActionAttribute, IsRequired = false)]
        public string Action
        {
            get { return Convert.ToString(this[ActionAttribute]); }
            set { this[ActionAttribute] = value; }
        }

        /// <summary>
        /// Controller
        /// </summary>
        [ConfigurationProperty(ControllerAttribute, IsRequired = false)]
        public string Controller
        {
            get { return Convert.ToString(this[ControllerAttribute]); }
            set { this[ControllerAttribute] = value; }
        }

        /// <summary>
        /// Icon (fa-cart)
        /// </summary>
        [ConfigurationProperty(IconAttribute, IsRequired = false)]
        public string Icon
        {
            get { return Convert.ToString(this[IconAttribute]); }
            set { this[IconAttribute] = value; }
        }

        /// <summary>
        /// Permission
        /// </summary>
        [ConfigurationProperty(PermissionAttribute, IsRequired = false)]
        public string Permission
        {
            get { return Convert.ToString(this[PermissionAttribute]); }
            set { this[PermissionAttribute] = value; }
        }

        [ConfigurationProperty(EntriesNode, IsRequired = false)]
        public NavEntryElementCollection NavEntryElements
        {
            get { return (NavEntryElementCollection)this[EntriesNode]; }
            set { this[EntriesNode] = value; }
        }
    }
}