using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Acl : AppcusiveEntityViewModelBase
    {
        public Acl() : base()
        {
            this.Aces = new List<Ace>();
        }

        [Display(Name = "Aces", ResourceType = typeof(GeneralResources))] 
        public List<Ace> Aces { get; set; }

        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityId", ResourceType = typeof(GeneralResources))]
        public long EntityId { get; set; }

        [Range(1, long.MaxValue, ErrorMessageResourceName = "requiredField", ErrorMessageResourceType = typeof(ErrorResources))]
        [Display(Name = "EntityKindId", ResourceType = typeof(GeneralResources))] 
        public long EntityKindId { get; set; }

        [Display(Name = "EntityKind", ResourceType = typeof(GeneralResources))] 
        public EntityKind EntityKind { get; set; }

        [Display(Name = "NoInheritanceFromParent", ResourceType = typeof(GeneralResources))]
        public bool NoInheritanceFromParent { get; set; }

        /// <param name="coreRepository"></param>
        internal void ResolveNavigationProperties(biz.dfch.CS.Appclusive.Api.Core.Core coreRepository)
        {
            Contract.Requires(null != coreRepository);

            // EntityKind
            if (this.EntityKindId > 0)
            {
                Api.Core.EntityKind entityKind = coreRepository.EntityKinds
                     .Where(j => j.Id == this.EntityKindId)
                     .FirstOrDefault();
                Contract.Assert(null != entityKind, "no entityKind available");
                this.EntityKind = AutoMapper.Mapper.Map<EntityKind>(entityKind);
            }

        }

    }
}