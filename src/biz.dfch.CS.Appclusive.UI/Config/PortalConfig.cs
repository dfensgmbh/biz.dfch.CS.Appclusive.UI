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
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Config
{
    public class PortalConfig
    {
        private const int _pageSize = 45; //restricted to 45 due to paging problems with Appclusive.Core
        public static int Pagesize = _pageSize;

        /// <summary>
        /// Number of options to displayed to the user
        /// </summary>
        public static int Searchsize = _pageSize;

        /// <summary>
        /// Number of records to load before distinct can be applied to options
        /// </summary>
        public static int SearchLoadSize = Properties.Settings.Default.SearchLoadSize;
    }
}