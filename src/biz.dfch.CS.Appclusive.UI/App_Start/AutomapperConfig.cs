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
            // toway mapping for Catalogues
            Mapper.CreateMap<Models.Core.Catalogue, Api.Core.Catalogue>();
            Mapper.CreateMap<Models.Core.CatalogueItem, Api.Core.CatalogueItem>();

            Mapper.CreateMap<Api.Core.Catalogue, Models.Core.Catalogue>();
            Mapper.CreateMap<Api.Core.CatalogueItem, Models.Core.CatalogueItem>();

            // no edit for inventory
            Mapper.CreateMap<Api.Core.Node, Models.Core.Node>();
            Mapper.CreateMap<Api.Core.Assoc, Models.Core.Assoc>();

        }
    }
}