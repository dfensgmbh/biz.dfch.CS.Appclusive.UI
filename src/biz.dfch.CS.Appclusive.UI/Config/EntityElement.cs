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

namespace biz.dfch.CS.Appclusive.UI.Config
{
    public class EntityElement : ConfigurationElement
    {
        private const string EntityNameAttribute = "entityname";
        private const string FilterAttribute = "filter";
        private const string DisplayAttribute = "display";
        private const string SearchKeyAttribute = "searchkey";
        private const string SelectAttribute = "select";


        /// <summary>
        /// Controller name
        /// </summary>
        [ConfigurationProperty(EntityNameAttribute, IsRequired = true)]
        public string EntityName
        {
            get { return Convert.ToString(this[EntityNameAttribute]); }
            set { this[EntityNameAttribute] = value; }
        }

        /// <summary>
        /// filter expression
        /// -> "substringof('{0}',Name)"
        /// -> {0} will be filled by the search term
        /// </summary>
        [ConfigurationProperty(FilterAttribute, IsRequired = true)]
        public string Filter
        {
            get { return Convert.ToString(this[FilterAttribute]); }
            set { this[FilterAttribute] = value; }
        }

        /// <summary>
        /// Expression or property name to show as result-Value "ToString()"
        /// -> Name
        /// </summary>
        [ConfigurationProperty(DisplayAttribute, IsRequired = true)]
        public string Display
        {
            get { return Convert.ToString(this[DisplayAttribute]); }
            set { this[DisplayAttribute] = value; }
        }

        /// <summary>
        /// Expression or property name to use as search-key when the result is selected
        /// -> Name
        /// </summary>
        [ConfigurationProperty(SearchKeyAttribute, IsRequired = true)]
        public string SearchKey
        {
            get { return Convert.ToString(this[SearchKeyAttribute]); }
            set { this[SearchKeyAttribute] = value; }
        }

        /// <summary>
        /// Select expression to load the necessary properties
        /// -> Id,Name
        /// </summary>
        [ConfigurationProperty(SelectAttribute, IsRequired = true)]
        public string Select
        {
            get { return Convert.ToString(this[SelectAttribute]); }
            set { this[SelectAttribute] = value; }
        }

    }
}
