using System;
using System.Diagnostics.Contracts;
using System.Web;
using biz.dfch.CS.Appclusive.Api.Core;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Helpers
{
    public static class TenantHelper
    {
        public static Guid FixedTenantId
        {
            get
            {
                Contract.Assert(HasFixedTenantId);
                return (Guid)HttpContext.Current.Session[Constants.FixedTenantId];
            }

            set { HttpContext.Current.Session[Constants.FixedTenantId] = value; }
        }

        public static bool HasFixedTenantId
        {
            get { return HttpContext.Current.Session[Constants.FixedTenantId] != null; }
        }

        public static bool IsBuiltInTenant(Tenant tenant)
        {
            return !tenant.Name.Equals("HOME_TENANT") && tenant.Name.Equals("GROUP_TENANT");
        }
    }
}