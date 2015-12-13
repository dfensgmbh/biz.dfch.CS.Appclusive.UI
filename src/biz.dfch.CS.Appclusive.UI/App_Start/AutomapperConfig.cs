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
using AutoMapper;

namespace biz.dfch.CS.Appclusive.UI
{
    public class AutomapperConfig
    {
        public static void MapInit()
        {
            // mapping back to API not neede because API-Entities are tracked and convetion by Automapper does not include that

            Mapper.CreateMap<Api.Core.Catalogue, Models.Core.Catalogue>();
            Mapper.CreateMap<Models.Core.Catalogue, Api.Core.Catalogue>();
            Mapper.CreateMap<Api.Core.CatalogueItem, Models.Core.CatalogueItem>();
            Mapper.CreateMap<Models.Core.CatalogueItem, Api.Core.CatalogueItem>();

            Mapper.CreateMap<Api.Core.Node, Models.Core.Node>();
            Mapper.CreateMap<Api.Core.Assoc, Models.Core.Assoc>();

            Mapper.CreateMap<Api.Core.Cart, Models.Core.Cart>();
            Mapper.CreateMap<Models.Core.Cart, Api.Core.Cart>();
            Mapper.CreateMap<Api.Core.CartItem, Models.Core.CartItem>();
            Mapper.CreateMap<Models.Core.CartItem, Api.Core.CartItem>();

            Mapper.CreateMap<Api.Core.Order, Models.Core.Order>();
            Mapper.CreateMap<Models.Core.Order, Api.Core.Order>();
            Mapper.CreateMap<Api.Core.OrderItem, Models.Core.OrderItem>();
            Mapper.CreateMap<Models.Core.OrderItem, Api.Core.OrderItem>();

            Mapper.CreateMap<Api.Core.Product, Models.Core.Product>();
            Mapper.CreateMap<Models.Core.Product, Api.Core.Product>();

            Mapper.CreateMap<Api.Core.Job, Models.Core.Job>();
            Mapper.CreateMap<Api.Core.Approval, Models.Core.Approval>();
            
            Mapper.CreateMap<Api.Core.KeyNameValue, Models.Core.KeyNameValue>();
            Mapper.CreateMap<Models.Core.KeyNameValue, Api.Core.KeyNameValue>();

            Mapper.CreateMap<Api.Core.Gate, Models.Core.Gate>();
            Mapper.CreateMap<Models.Core.Gate, Api.Core.Gate>();

            Mapper.CreateMap<Api.Core.EntityType, Models.Core.EntityType>(); 
            Mapper.CreateMap<Models.Core.EntityType, Api.Core.EntityType>();

            Mapper.CreateMap<Api.Core.ManagementUri, Models.Core.ManagementUri>();
            Mapper.CreateMap<Models.Core.ManagementUri, Api.Core.ManagementUri>();

            Mapper.CreateMap<Api.Core.ManagementCredential, Models.Core.ManagementCredential>();
            Mapper.CreateMap<Models.Core.ManagementCredential, Api.Core.ManagementCredential>();

            Mapper.CreateMap<Api.Diagnostics.AuditTrail, Models.Diagnostics.AuditTrail>();
            Mapper.CreateMap<Api.Diagnostics.Endpoint, Models.Diagnostics.Endpoint>();

        }
    }
}