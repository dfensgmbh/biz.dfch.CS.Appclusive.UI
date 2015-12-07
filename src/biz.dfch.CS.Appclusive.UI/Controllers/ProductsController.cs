using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Diagnostics.Contracts;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ProductsController : CoreControllerBase
    {
        public ProductsController()
            : base()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }
        // GET: Products
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.Products.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.Product>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Product>());
            }
        }

        #region Product

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.Products.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Product>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Product());
            }
        }
        
        #endregion
    }
}
