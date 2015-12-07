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
            Mapper.CreateMap<Api.Core.CatalogueItem, Models.Core.CatalogueItem>();
            Mapper.CreateMap<Models.Core.Catalogue, Api.Core.Catalogue>();
            Mapper.CreateMap<Models.Core.CatalogueItem, Api.Core.CatalogueItem>();

            Mapper.CreateMap<Api.Core.Node, Models.Core.Node>();
            Mapper.CreateMap<Api.Core.Assoc, Models.Core.Assoc>();

            Mapper.CreateMap<Api.Core.Cart, Models.Core.Cart>();
            Mapper.CreateMap<Api.Core.CartItem, Models.Core.CartItem>();
            Mapper.CreateMap<Models.Core.CartItem, Api.Core.CartItem>();

            Mapper.CreateMap<Api.Core.Order, Models.Core.Order>();
            Mapper.CreateMap<Api.Core.OrderItem, Models.Core.OrderItem>();
            Mapper.CreateMap<Models.Core.Order, Api.Core.Order>();
            Mapper.CreateMap<Models.Core.OrderItem, Api.Core.OrderItem>();

            Mapper.CreateMap<Api.Core.Product, Models.Core.Product>();

            Mapper.CreateMap<Api.Core.Job, Models.Core.Job>();
            Mapper.CreateMap<Api.Core.Approval, Models.Core.Approval>();            
            Mapper.CreateMap<Api.Core.KeyNameValue, Models.Core.KeyNameValue>();            
            Mapper.CreateMap<Api.Core.Gate, Models.Core.Gate>();


            Mapper.CreateMap<Api.Core.EntityType, Models.Core.EntityType>(); 
            Mapper.CreateMap<Models.Core.EntityType, Api.Core.EntityType>();
            
            Mapper.CreateMap<Api.Core.ManagementUri, Models.Core.ManagementUri>();
            Mapper.CreateMap<Api.Core.ManagementCredential, Models.Core.ManagementCredential>();

            Mapper.CreateMap<Api.Diagnostics.AuditTrail, Models.Diagnostics.AuditTrail>();
            Mapper.CreateMap<Api.Diagnostics.Endpoint, Models.Diagnostics.Endpoint>();

        }
    }
}