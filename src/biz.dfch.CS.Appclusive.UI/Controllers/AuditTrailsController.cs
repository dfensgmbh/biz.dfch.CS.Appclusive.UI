using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AuditTrailsController : DiagnosticsControllerBase
    {
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Diagnostics.AuditTrail> items;
                if (pageNr > 1)
                {
                    items = DiagnosticsRepository.AuditTrails.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = DiagnosticsRepository.AuditTrails.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Diagnostics.AuditTrail>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Diagnostics.AuditTrail>());
            }
        }

        public ActionResult Details(int id)
        {
            try
            {
                var item = DiagnosticsRepository.AuditTrails.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Diagnostics.AuditTrail>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Diagnostics.AuditTrail());
            }
        }

    }
}