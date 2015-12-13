using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class DiagnosticsControllerBase : Controller
    {
        protected biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics DiagnosticsRepository
        {
            get
            {
                if (diagnosticsRepository == null)
                {
                    diagnosticsRepository = new biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics(new Uri(Properties.Settings.Default.AppculsiveApiDiagnosticsUrl));
                    diagnosticsRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    diagnosticsRepository.IgnoreMissingProperties = true;
                    diagnosticsRepository.Format.UseJson();
                    diagnosticsRepository.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
                }
                return diagnosticsRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics diagnosticsRepository;


        public DiagnosticsControllerBase()
            : base()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }
    }
}