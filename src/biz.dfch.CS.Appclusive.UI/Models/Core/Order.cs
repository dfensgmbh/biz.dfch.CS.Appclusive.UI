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

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Order : ViewModelBase, IAppcusiveEntityBase
    {
        public Order()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.OrderItems = new List<OrderItem>();
        }

        public CostCentre CostCentre { get; set; }
        
        public long? CostCentreId { get; set; }
        
        public DateTimeOffset Created { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string Description { get; set; }
        
        public long Id { get; set; }
        
        public DateTimeOffset Modified { get; set; }
        
        public string ModifiedBy { get; set; }
        
        public string Name { get; set; }
        
        public List<OrderItem> OrderItems { get; set; }
        
        public string Parameters { get; set; }
        
        public string Requester { get; set; }
        
        public byte[] RowVersion { get; set; }
        
        public string Status { get; set; }
        
        public string Tid { get; set; }

    }
}