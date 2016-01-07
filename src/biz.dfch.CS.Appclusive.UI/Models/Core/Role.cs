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
            this.Users = new List<User>();
        }

        public List<Permission> Permissions { get; set; }

        public List<User> Users { get; set; }

    }
}