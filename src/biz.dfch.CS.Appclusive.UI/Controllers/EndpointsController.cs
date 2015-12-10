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
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Diagnostics.Endpoint> items;
                if (pageNr > 1)
                {
                    items = DiagnosticsRepository.Endpoints.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = DiagnosticsRepository.Endpoints.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Diagnostics.Endpoint>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Diagnostics.Endpoint>());
            }
        }

        // GET: Endpoints/Details/5
        public ActionResult Details(long id)
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
