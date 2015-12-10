﻿using System;
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

            Mapper.CreateMap<Api.Core.Ace, Models.Core.Ace>();
            Mapper.CreateMap<Models.Core.Ace, Api.Core.Ace>();

            Mapper.CreateMap<Api.Core.Acl, Models.Core.Acl>();
            Mapper.CreateMap<Models.Core.Acl, Api.Core.Acl>();

            Mapper.CreateMap<Api.Core.User, Models.Core.User>();
            Mapper.CreateMap<Models.Core.User, Api.Core.User>();

            Mapper.CreateMap<Api.Core.Tenant, Models.Core.Tenant>();
            Mapper.CreateMap<Models.Core.Tenant, Api.Core.Tenant>();

            Mapper.CreateMap<Api.Core.ContractMapping, Models.Core.ContractMapping>();
            Mapper.CreateMap<Models.Core.ContractMapping, Api.Core.ContractMapping>();

            Mapper.CreateMap<Api.Core.CostCentre, Models.Core.CostCentre>();
            Mapper.CreateMap<Models.Core.CostCentre, Api.Core.CostCentre>();

            Mapper.CreateMap<Api.Core.CimiTarget, Models.Core.CimiTarget>();
            Mapper.CreateMap<Models.Core.CimiTarget, Api.Core.CimiTarget>();
            
            Mapper.CreateMap<Api.Core.Product, Models.Core.Product>();
            Mapper.CreateMap<Models.Core.Product, Api.Core.Product>();

            Mapper.CreateMap<Api.Core.Customer, Models.Core.Customer>();
            Mapper.CreateMap<Models.Core.Customer, Api.Core.Customer>();
        }
    }
}