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

using biz.dfch.CS.Appclusive.UI.Config;
using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class OrdersController : CoreControllerBase<Api.Core.Order, Models.Core.Order, Models.Core.OrderItem>
    {
        protected override DataServiceQuery<Api.Core.Order> BaseQuery { get { return CoreRepository.Orders.Expand("CostCentre").Expand("Requester"); } }

        protected override void OnBeforeRender<M>(M model)
        {
            Models.Core.Order m = model as Models.Core.Order;
            try
            {
                m.ResolveJob(this.CoreRepository);
            }
            catch (Exception ex) { ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex)); }
        }

        #region Order

        // GET: Orders/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Orders.Expand("CostCentre").Expand("Requester").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
               
                Models.Core.Order modelItem = AutoMapper.Mapper.Map<Models.Core.Order>(item);
                if (null != modelItem)
                {
                    modelItem.OrderItems = LoadOrderItems(id, 1);
                    try
                    {
                        modelItem.ResolveJob(this.CoreRepository);
                    }
                    catch (Exception ex) { ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex)); }
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Order());
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Order apiItem = null;
            try
            {
                apiItem = CoreRepository.Orders.Expand("Requester").Expand("CostCentre").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Order>(apiItem));
            }
        }

        #endregion

        #region Order Items list and search

        // GET: Nodes/ItemList
        public PartialViewResult ItemIndex(long orderId, int pageNr = 1, string itemSearchTerm = null, string orderBy = null)
        {
            ViewBag.ParentId = orderId;
            DataServiceQuery<Api.Core.OrderItem> itemsBaseQuery = CoreRepository.OrderItems;
            string itemsBaseFilter = "OrderId eq " + orderId;
            return base.ItemIndex<Api.Core.OrderItem, Models.Core.OrderItem>(itemsBaseQuery, itemsBaseFilter, pageNr, itemSearchTerm, orderBy);
        }

        private List<Models.Core.OrderItem> LoadOrderItems(long orderId, int pageNr)
        {
            QueryOperationResponse<Api.Core.OrderItem> items = CoreRepository.OrderItems
                    .AddQueryOption("$filter", "OrderId eq " + orderId)
                    .AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", PortalConfig.Pagesize)
                    .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                    .Execute() as QueryOperationResponse<Api.Core.OrderItem>;

            ViewBag.ParentId = orderId;
            ViewBag.AjaxPaging = new PagingInfo(pageNr, items.TotalCount);

            return AutoMapper.Mapper.Map<List<Models.Core.OrderItem>>(items);
        }

        public ActionResult ItemSearch(long orderId, string term)
        {
            DataServiceQuery<Api.Core.OrderItem> itemsBaseQuery = CoreRepository.OrderItems;
            string itemsBaseFilter = "OrderId eq " + orderId;
            return base.ItemSearch(itemsBaseQuery, itemsBaseFilter, term);
        }

        #endregion Order Items list

        #region edit OrderItems

        public ActionResult ItemDetails(long id)
        {
            var item = CoreRepository.OrderItems.Expand("Order").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.OrderItem>(item));
        }

        // GET: Orders/Delete/5
        public ActionResult ItemDelete(long id)
        {
            Api.Core.OrderItem apiItem = null;
            try
            {
                apiItem = CoreRepository.OrderItems.Expand("Order").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Details", new { id = apiItem.OrderId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("ItemDetails", AutoMapper.Mapper.Map<Models.Core.OrderItem>(apiItem));
            }
        }

        #endregion
    }
}
