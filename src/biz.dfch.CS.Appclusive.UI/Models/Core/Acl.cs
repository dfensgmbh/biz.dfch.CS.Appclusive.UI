using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Acl : AppcusiveEntityBase
    {
        public Acl() : base()
        {
            this.Aces = new List<Ace>();
        }

        public List<Ace> Aces { get; set; }
    }
}