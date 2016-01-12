/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Diagnostics.Contracts;
using System.Data.Services.Client;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CartsController : CoreControllerBase<Api.Core.Cart, Models.Core.Cart, Models.Core.CartItem>
    {
        protected override DataServiceQuery<Api.Core.Cart> BaseQuery { get { return CoreRepository.Carts; } }

        #region Cart

        // GET: Carts/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Carts.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                if (null == item)
                {
                    return View(new Models.Core.Cart());
                }
                else
                {
                    Models.Core.Cart modelItem = AutoMapper.Mapper.Map<Models.Core.Cart>(item);
                    if (null != modelItem)
                    {
                        modelItem.CartItems = LoadCartItems(id, 1);
                    }
                    return View(modelItem);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Cart());
            }
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var item = CoreRepository.Carts.Expand("CartItems").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Cart modelItem = AutoMapper.Mapper.Map<Models.Core.Cart>(item);
                if (null != modelItem)
                {
                    modelItem.CartItems = LoadCartItems(id, 1);
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Cart());
            }
        }

        // POST: Carts/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Cart cart)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    cart.CartItems = AutoMapper.Mapper.Map<List<Models.Core.CartItem>>(CoreRepository.CartItems.Where(ci => ci.CartId == id).ToList());
                    return View(cart);
                }
                else
                {
                    var apiItem = CoreRepository.Carts.Expand("CartItems").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = cart.Name;
                    apiItem.Description = cart.Description;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    
                    Models.Core.Cart modelItem = AutoMapper.Mapper.Map<Models.Core.Cart>(apiItem);
                    if (null != modelItem)
                    {
                        modelItem.CartItems = LoadCartItems(id, 1);
                    }
                    return View(modelItem);
                }
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
        public ActionResult Delete(long id)
        {
            Api.Core.Cart apiItem = null;
            try
            {
                apiItem = CoreRepository.Carts.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Cart>(apiItem));
            }
        }
        
        public ActionResult CheckoutCart(long id)
        {
            try
            {
                Models.Core.Order order = new Models.Core.Order();
                order.Name = "DefaultOrder";
                order.Parameters = "{}";

                CoreRepository.AddToOrders(AutoMapper.Mapper.Map<Api.Core.Order>(order));
                CoreRepository.SaveChanges();

                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Order has been placed"));

                return View("Details", (Models.Core.Cart)null);
            }
            catch (Exception ex)
            {
                ViewBag.Notifications = ExceptionHelper.GetAjaxNotifications(ex);
                Api.Core.Cart item = null;
                try
                {
                    item = CoreRepository.Carts.Expand("CartItems").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                }
                catch { }
                if (item == null) item = new Api.Core.Cart();
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Cart>(item));
            }
        }
        
        #endregion

        #region Node-children list and search

        // GET: Nodes/ItemList
        public PartialViewResult ItemIndex(long cartId, int pageNr = 1, string itemSearchTerm = null, string orderBy = null)
        {
            ViewBag.ParentId = cartId;
            DataServiceQuery<Api.Core.CartItem> itemsBaseQuery = CoreRepository.CartItems;
            string itemsBaseFilter = "CartId eq " + cartId;
            return base.ItemIndex<Api.Core.CartItem, Models.Core.CartItem>(itemsBaseQuery, itemsBaseFilter, pageNr, itemSearchTerm, orderBy);
        }

        private List<Models.Core.CartItem> LoadCartItems(long cartId, int pageNr)
        {
            QueryOperationResponse<Api.Core.CartItem> items = CoreRepository.CartItems
                    .AddQueryOption("$filter", "CartId eq " + cartId)
                    .AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", PortalConfig.Pagesize)
                    .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                    .Execute() as QueryOperationResponse<Api.Core.CartItem>;

            ViewBag.ParentId = cartId;
            ViewBag.AjaxPaging = new PagingInfo(pageNr, items.TotalCount);

            return AutoMapper.Mapper.Map<List<Models.Core.CartItem>>(items);
        }

        public ActionResult ItemSearch(long cartId, string term)
        {
            DataServiceQuery<Api.Core.CartItem> itemsBaseQuery = CoreRepository.CartItems;
            string itemsBaseFilter = "CartId eq " + cartId;
            return base.ItemSearch(itemsBaseQuery, itemsBaseFilter, term);
        }

        #endregion Node-children list

        #region edit CartItems

        public ActionResult ItemDetails(long id)
        {
            try
            {
                var item = CoreRepository.CartItems.Expand("Cart").Expand("CatalogueItem").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CartItem>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CartItem());
            }
        }

        // GET: Carts/Edit/5
        public ActionResult ItemEdit(long id)
        {
            try
            {
                Contract.Requires(id > 0);
                var apiItem = CoreRepository.CartItems.Expand("Cart").Expand("CatalogueItem").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult ItemEdit(long id, Models.Core.CartItem cartItem)
        {
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != cartItem);
                if (!ModelState.IsValid)
                {
                    return View(cartItem);
                }
                else
                {
                    var apiItem = CoreRepository.CartItems.Expand("Cart").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
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
                    return View(AutoMapper.Mapper.Map<Models.Core.CartItem>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(cartItem);
            }
        }


        // GET: Carts/ItemDelete/5
        public ActionResult ItemDelete(long id)
        {
            Api.Core.CartItem apiItem = null;
            try
            {
                Contract.Requires(id > 0);
                apiItem = CoreRepository.CartItems.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
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

        #region VDI

        public ActionResult VdiPersonal()
        {
            return RedirectToAction("VdiCreate", new { vdiName = biz.dfch.CS.Appclusive.UI.Models.Core.VdiCartItem.VDI_PERSONAL_NAME });
        }
        public ActionResult VdiTechnical()
        {
            return RedirectToAction("VdiCreate", new { vdiName = biz.dfch.CS.Appclusive.UI.Models.Core.VdiCartItem.VDI_TECHNICAL_NAME });
        }
        public ActionResult VdiCreate(string vdiName)
        {
            try
            {
                Contract.Requires(vdiName == Models.Core.VdiCartItem.VDI_PERSONAL_NAME || vdiName == Models.Core.VdiCartItem.VDI_TECHNICAL_NAME, "no valid vdi-name");

                Models.Core.VdiCartItem cartItem = new Models.Core.VdiCartItem();
                cartItem.Name = vdiName;
                cartItem.VdiName = vdiName;
                cartItem.Quantity = 1;

                return View(cartItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("VdiDetails", new Models.Core.VdiCartItem());
            }
        }

        [HttpPost]
        public ActionResult VdiCreate(string vdiName, Models.Core.VdiCartItem cartItem)
        {
            try
            {
                Contract.Requires(vdiName == Models.Core.VdiCartItem.VDI_PERSONAL_NAME || vdiName == Models.Core.VdiCartItem.VDI_TECHNICAL_NAME, "no valid vdi-name");
                var catalogueItem = CoreRepository.CatalogueItems.Where(c => c.Name == vdiName).FirstOrDefault();
                Contract.Assert(null != catalogueItem, "No catalog item for " + vdiName);

                if (!ModelState.IsValid)
                {
                    return View(cartItem);
                }
                else
                {
                    cartItem.CatalogueItemId = catalogueItem.Id;
                    cartItem.VdiName = vdiName;
                    cartItem.Quantity = 1;
                    cartItem.Parameters = cartItem.RequesterToParameters();

                    CoreRepository.AddToCartItems(AutoMapper.Mapper.Map<Api.Core.CartItem>(cartItem));
                    CoreRepository.SaveChanges();

                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, string.Format("Item {0} added to cart", catalogueItem.Name)));
                    return View("VdiSave", cartItem);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(cartItem);
            }
        }

        #endregion
    }
}
