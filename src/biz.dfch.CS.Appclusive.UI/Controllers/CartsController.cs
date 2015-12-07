using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CartsController : Controller
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

        // GET: Carts
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.Carts.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.Cart>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Cart>());
            }
        }

        #region Cart

        // GET: Carts/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.Carts.Expand("CartItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Cart>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Cart());
            }
        }


        #endregion

        #region edit CartItems

        public ActionResult ItemDetails(int id)
        {
            try
            {
                var item = CoreRepository.CartItems.Expand("Cart").Expand("CatalogueItem").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CartItem>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CartItem());
            }
        }

        #endregion
    }
}
