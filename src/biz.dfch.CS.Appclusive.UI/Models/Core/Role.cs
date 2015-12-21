using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class Role : AppcusiveEntityViewModelBase
    {
        public Role()
        {
            AppcusiveEntityBaseHelper.InitEntity(this);
            this.Permissions = new List<Permission>();
        }

        public List<Permission> Permissions { get; set; }

    }
}