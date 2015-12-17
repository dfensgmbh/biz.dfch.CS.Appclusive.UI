﻿/**
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

namespace biz.dfch.CS.Appclusive.UI.Models
{
    static public class AppcusiveEntityBaseHelper
    {
        /// <summary>
        /// setzt all the neccessary default values
        /// </summary>
        public static  void InitEntity(IAppcusiveEntityBase entity) {
            
            entity.Created = DateTimeOffset.Now;
            entity.Modified = DateTimeOffset.Now;
            entity.CreatedById = 1;
            entity.ModifiedById = 1;
            entity.Tid = System.Guid.Parse( "11111111-1111-1111-1111-111111111111");
            entity.Name = "new";
        }
    }
}