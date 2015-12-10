using biz.dfch.CS.Appclusive.Api.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public static class Extension
    {
        public static List<Customer> Expand(this List<Customer> list, string propertyName)
        {
            return list;
        }
        public static List<CimiTarget> Expand(this List<CimiTarget> list, string propertyName)
        {
            return list;
        }
        public static List<ContractMapping> Expand(this List<ContractMapping> list, string propertyName)
        {
            return list;
        }
        public static List<Tenant> Expand(this List<Tenant> list, string propertyName)
        {
            return list;
        }
        public static List<User> Expand(this List<User> list, string propertyName)
        {
            return list;
        }
    }
}