using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class EndpointsController : Controller
    {
        private biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics DiagnosticsRepository
        {
            get
            {
                if (diagnosticsRepository == null)
                {
                    diagnosticsRepository = new biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics(new Uri(Properties.Settings.Default.AppculsiveApiDiagnosticsUrl));
                    diagnosticsRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                return diagnosticsRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics diagnosticsRepository;

        // GET: Endpoints
        public ActionResult Index()
        {
            var items = DiagnosticsRepository.Endpoints.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Diagnostics.Endpoint>>(items));
        }
        
        // GET: Endpoints/Details/5
        public ActionResult Details(int id)
        {
            var item = DiagnosticsRepository.Endpoints.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Diagnostics.Endpoint>(item));
        }

    }
}
