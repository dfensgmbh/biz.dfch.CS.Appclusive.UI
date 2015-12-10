using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CimiTargetsController : CoreControllerBase
    {

        // GET: CimiTargets
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.CimiTarget> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.CimiTargets.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.CimiTargets.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.CimiTarget>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.CimiTarget>());
            }
        }

        // GET: CimiTargets/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.CimiTargets.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CimiTarget>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CimiTarget());
            }
        }
    }
}
