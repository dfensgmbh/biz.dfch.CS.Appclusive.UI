using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class NodesController : CoreControllerBase
    {
        // GET: Nodes
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.Nodes.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.Node>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Node>());
            }
        }

        // GET: Nodes/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Nodes.Expand("Children").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Node>(item));
        }

    }
}
