using System;

namespace biz.dfch.CS.Appclusive.UI.Managers
{
    public class AuthenticatedCoreApi : Api.Core.Core
    {
        public AuthenticatedCoreApi()
            : base(new Uri(string.Format("{0}{1}", Properties.Settings.Default.AppclusiveApiBaseUrl, "Core")))
        {
            this.ConfigureForAppclusive();
        }
    }
}