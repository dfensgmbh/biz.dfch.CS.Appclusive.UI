using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class JobsController : Controller
    {
        private biz.dfch.CS.Appclusive.Api.Core.Core CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.Api.Core.Core(new Uri(Properties.Settings.Default.AppculsiveApiCoreUrl));
                    coreRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Core.Core coreRepository;

        // GET: Jobs
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.Jobs.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.Job>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Job>());
            }
        }

        // GET: Jobs/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Jobs.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Job>(item));
        }

    }
}
