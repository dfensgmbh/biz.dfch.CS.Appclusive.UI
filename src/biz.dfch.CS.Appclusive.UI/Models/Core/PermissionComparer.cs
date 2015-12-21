using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    public class PermissionComparer : IEqualityComparer<Api.Core.Permission>
    {
        public bool Equals(Api.Core.Permission x, Api.Core.Permission y)
        {
            return x != null && y != null && x.Id == y.Id;
        }

        public int GetHashCode(Api.Core.Permission obj)
        {
            return null == obj ? 0 : obj.Id.GetHashCode();
        }
    }
}