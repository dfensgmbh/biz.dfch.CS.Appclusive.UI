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

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Assoc : ViewModelBase, IAppcusiveEntityBase
    {
        public Assoc()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
        }
       
        public DateTimeOffset Created { get; set; }
       
        public string CreatedBy { get; set; }
       
        public string Description { get; set; }
       
        public Node Destination { get; set; }
       
        public long DestinationId { get; set; }
       
        public long Id { get; set; }
       
        public DateTimeOffset Modified { get; set; }
       
        public string ModifiedBy { get; set; }
       
        public string Name { get; set; }
       
        public long Order { get; set; }
       
        public byte[] RowVersion { get; set; }
       
        public Node Source { get; set; }
       
        public long SourceId { get; set; }
       
        public string Tid { get; set; }

    }
}