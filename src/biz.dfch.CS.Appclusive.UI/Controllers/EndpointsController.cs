using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class EndpointsController : DiagnosticsControllerBase
    {
        // GET: Endpoints
        public ActionResult Index()
        {
            try
            {
                var items = DiagnosticsRepository.Endpoints.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Diagnostics.Endpoint>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Diagnostics.Endpoint>());
            }
        }

        // GET: Endpoints/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = DiagnosticsRepository.Endpoints.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Diagnostics.Endpoint>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Diagnostics.Endpoint());
            }
        }

    }
}
