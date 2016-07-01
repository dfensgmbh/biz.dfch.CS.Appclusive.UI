using System;
using biz.dfch.CS.Appclusive.Api.Cmp;

namespace biz.dfch.CS.Appclusive.UI.Managers
{
    public class AuthenticatedCmpApi : Cmp
    {
        public AuthenticatedCmpApi()
            : base(new Uri(string.Format("{0}{1}", Properties.Settings.Default.AppclusiveApiBaseUrl, "Cmp")))
        {
            this.ConfigureForAppclusive();
        }
    }
}