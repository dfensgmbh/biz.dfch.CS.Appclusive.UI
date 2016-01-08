﻿using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Display(Name = "EntityKindId", ResourceType = typeof(GeneralResources))] 
        public long EntityKindId { get; set; }

        [Display(Name = "EntityKind", ResourceType = typeof(GeneralResources))] 
        public EntityKind EntityKind { get; set; }
    }
}