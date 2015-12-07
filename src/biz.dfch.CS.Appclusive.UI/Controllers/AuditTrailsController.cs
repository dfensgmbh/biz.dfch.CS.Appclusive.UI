using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AuditTrailsController : CoreControllerBase
    {
        // http://localhost:xxxx/myView/AuditTrail?numtimes=42
        public ActionResult Index(int numTimes = 1)
        {
            try
            {
                var diag = new biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics(new Uri(Properties.Settings.Default.AppculsiveApiDiagnosticsUrl));
                diag.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                var a = new biz.dfch.CS.Appclusive.Api.Diagnostics.AuditTrail();
                var auditTrails = diag.AuditTrails.ToList();

                ViewBag.Title = "AuditTrail";
                ViewBag.AuditTrails = auditTrails;

                return View();
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Diagnostics.AuditTrail>());
            }
        }
    }
}