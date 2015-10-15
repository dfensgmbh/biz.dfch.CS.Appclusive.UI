using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class MyViewController : Controller
    {
        // GET: MyView
        public ActionResult Index()
        {
            return View();
        }

        // http://localhost:xxxx/myView/Welcome?name=Edgar&numtimes=42
        public ActionResult Welcome(string name, int numTimes = 1)
        {
            var diag = new biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics(new Uri("http://appclusive/api/Diagnostics"));
            diag.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            var names = diag.AuditTrails.ToList();

            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;

            return View();
        }

        // http://localhost:xxxx/myView/AuditTrail?numtimes=42
        public ActionResult AuditTrail(int numTimes = 1)
        {
            var diag = new biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics(new Uri("http://appclusive/api/Diagnostics"));
            diag.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
            var a = new biz.dfch.CS.Appclusive.Api.Diagnostics.AuditTrail();
            var auditTrails = diag.AuditTrails.ToList();

            ViewBag.Title = "AuditTrail";
            ViewBag.AuditTrails = auditTrails;

            return View();
        }
    }
}