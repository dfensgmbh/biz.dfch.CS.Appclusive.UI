using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Diagnostics.Contracts;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CartsController : CoreControllerBase
    {
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

        // GET: Carts/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.Carts.Expand("CartItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Cart>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Cart());
            }
        }

        // POST: Carts/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.Cart cart)
        {
            try
            {
                var apiItem = CoreRepository.Carts.Expand("CartItems").Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = cart.Name;
                apiItem.Description = cart.Description;
                
                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Cart>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                if (null == cart.CartItems)
                {
                    cart.CartItems = new List<Models.Core.CartItem>();
                }
                return View(cart);
            }
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.Cart apiItem = null;
            try
            {
                apiItem = CoreRepository.Carts.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.Cart>(apiItem)));
            }
        }
        
        public ActionResult CheckoutCart(int id)
        {
            try
            {
                Models.Core.Order order = new Models.Core.Order();
                order.Name = "DefaultOrder";
                order.Parameters = "{}";

                CoreRepository.AddToOrders(AutoMapper.Mapper.Map<Api.Core.Order>(order));
                CoreRepository.SaveChanges();

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

        // GET: Carts/Edit/5
        public ActionResult ItemEdit(int id)
        {
            try
            {
                Contract.Requires(id > 0);
                var apiItem = CoreRepository.CartItems.Expand("Cart").Expand("CatalogueItem").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CartItem>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CartItem());
            }
        }

        // POST: Carts/ItemEdit/5
        [HttpPost]
        public ActionResult ItemEdit(int id, Models.Core.CartItem cartItem)
        {
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != cartItem);
                var apiItem = CoreRepository.CartItems.Expand("Cart").Where(c => c.Id == id).FirstOrDefault();
                Contract.Assert(null != apiItem);

                #region copy all edited properties

                apiItem.Name = cartItem.Name;
                apiItem.Quantity = cartItem.Quantity;
                apiItem.Description = cartItem.Description;
                apiItem.Parameters = cartItem.Parameters;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(cartItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(cartItem);
            }
        }


        // GET: Carts/ItemDelete/5
        public ActionResult ItemDelete(int id)
        {
            Api.Core.CartItem apiItem = null;
            try
            {
                Contract.Requires(id > 0);
                apiItem = CoreRepository.CartItems.Where(c => c.Id == id).FirstOrDefault();
                Contract.Assert(null != apiItem);
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Details", new { Id = apiItem.CartId });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("ItemDetails", View(AutoMapper.Mapper.Map<Models.Core.CartItem>(apiItem)));
            }
        }
        #endregion
    }
}
