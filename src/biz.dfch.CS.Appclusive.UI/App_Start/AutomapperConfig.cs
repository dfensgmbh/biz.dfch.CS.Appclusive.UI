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

            Mapper.CreateMap<Api.Core.Node, Models.Core.Node>();
            Mapper.CreateMap<Api.Core.Assoc, Models.Core.Assoc>();

            Mapper.CreateMap<Api.Core.Order, Models.Core.Order>();
            Mapper.CreateMap<Api.Core.OrderItem, Models.Core.OrderItem>();

        }
    }
}