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
            var items = CoreRepository.Carts.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.Cart>>(items));
        }

        #region Cart 

        // GET: Carts/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Carts.Expand("CartItems").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Cart>(item));
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        
        public ActionResult CheckoutCart(int id)
        {
            try
            {
                Models.Core.Order order = new Models.Core.Order();
                order.Name = "DefaultOrder";
                order.Parameters = "{}";

                CoreRepository.AddToOrders(AutoMapper.Mapper.Map<Api.Core.Order>(order));
                coreRepository.SaveChanges();

                AjaxNotificationViewModel notification = new AjaxNotificationViewModel();
                notification.Level = ENotifyStyle.success;
                notification.Message = "Order has been placed";
                ViewBag.Notifications = new AjaxNotificationViewModel[] { notification };

                return View("Details", (Models.Core.Cart)null);
            }
            catch (Exception ex)
            {
                ViewBag.Notifications = ExceptionHelper.GetAjaxNotifications(ex);
                Api.Core.Cart item = null;
                try
                {
                    item = CoreRepository.Carts.Expand("CartItems").Where(c => c.Id == id).FirstOrDefault();
                }
                catch { }
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Cart>(item));
            }
        }

        #endregion

        #region edit CartItems

        public ActionResult ItemDetails(int id)
        {
            var item = CoreRepository.CartItems.Expand("Cart").Expand("CatalogueItem").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.CartItem>(item));
        }

        #endregion
    }
}
