using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models.Core
{
    class AceSearchViewComparer : IEqualityComparer<Ace>
    {
        public bool Equals(Ace ac1, Ace ac2)
        {
            string n1 = (null != ac1 && null != ac1.Permission) ? ac1.Permission.Name : null;
            string n2 = (null != ac2 && null != ac2.Permission) ? ac2.Permission.Name : null;
            return n1 == n2;
        }

        public int GetHashCode(Ace ac)
        {
            return (null != ac && null != ac.Permission) ? ac.Permission.Name.GetHashCode() : 0;
        }
    }
}