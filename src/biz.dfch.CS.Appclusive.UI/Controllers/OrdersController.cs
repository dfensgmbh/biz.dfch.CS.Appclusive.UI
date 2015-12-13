﻿/**
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

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class OrdersController : CoreControllerBase
    {
        // GET: Orders
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Order> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Orders.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Orders.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Order>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Order>());
            }
        }

        #region Order

        // GET: Orders/Details/5
        public ActionResult Details(int id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Orders.Expand("OrderItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Order>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Order());
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.Order apiItem = null;
            try
            {
                apiItem = CoreRepository.Orders.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Order>(apiItem));
            }
        }

        #endregion

        #region edit OrderItems

        public ActionResult ItemDetails(int id)
        {
            var item = CoreRepository.OrderItems.Expand("Order").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.OrderItem>(item));
        }

        // GET: Orders/Delete/5
        public ActionResult ItemDelete(int id)
        {
            Api.Core.OrderItem apiItem = null;
            try
            {
                apiItem = CoreRepository.OrderItems.Expand("Order").Where(c => c.Id == id).FirstOrDefault();
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
