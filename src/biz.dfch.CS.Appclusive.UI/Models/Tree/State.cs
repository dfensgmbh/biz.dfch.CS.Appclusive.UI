using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Tree
{
    public class State
    {
        public bool @checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }
}