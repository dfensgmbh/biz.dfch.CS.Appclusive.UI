using System;
using System.Net;
using biz.dfch.CS.Appclusive.Api.Diagnostics;

namespace biz.dfch.CS.Appclusive.UI.Managers
{
    public class AuthenticatedDiagnosticsApi : Diagnostics
    {
        public AuthenticatedDiagnosticsApi(NetworkCredential credential)
            : base(new Uri(string.Format("{0}{1}", Properties.Settings.Default.AppclusiveApiBaseUrl, "Diagnostics")))
        {
            this.ConfigureForAppclusive();
            Credentials = credential;
        }

        public AuthenticatedDiagnosticsApi()
            : base(new Uri(string.Format("{0}{1}", Properties.Settings.Default.AppclusiveApiBaseUrl, "Diagnostics")))
        {
            this.ConfigureForAppclusive();
        }
    }
}