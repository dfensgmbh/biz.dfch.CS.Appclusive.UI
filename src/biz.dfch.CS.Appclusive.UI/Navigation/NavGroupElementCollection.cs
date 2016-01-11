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
using System.Configuration;

namespace biz.dfch.CS.Appclusive.UI.Navigation
{
    [ConfigurationCollection(typeof(NavGroupElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class NavGroupElementCollection : ConfigurationElementCollection
    {
        public NavGroupElement this[int index]
        {
            get
            {
                return (NavGroupElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new NavGroupElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NavGroupElement)element).Name;
        }
    }
}
