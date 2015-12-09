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
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Product> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Products.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Products.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
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
