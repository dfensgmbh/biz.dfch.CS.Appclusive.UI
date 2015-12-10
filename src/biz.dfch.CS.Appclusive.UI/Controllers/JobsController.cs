using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class JobsController : CoreControllerBase
    {
        // GET: Jobs
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Job> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Jobs.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Jobs.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Job>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Job>());
            }
        }

        // GET: Jobs/Details/5
        public ActionResult Details(long id)
        {
            try
            {
                var item = CoreRepository.Jobs.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Job>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Job());
            }
        }

    }
}
