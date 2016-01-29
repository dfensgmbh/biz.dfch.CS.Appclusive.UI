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
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ProductsController : CoreControllerBase<Api.Core.Product, Models.Core.Product, object>
    {
        protected override DataServiceQuery<Api.Core.Product> BaseQuery { get { return CoreRepository.Products; } }

        #region Product

        // GET: Products/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Products.Expand("EntityKind").Expand("CatalogueItems").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Product>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Product());
            }
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Product());
        }

        // POST: Products/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Models.Core.Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Product>(product);

                    CoreRepository.AddToProducts(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Products.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Product>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Product());
            }
        }

        // POST: Products/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(long id, Models.Core.Product product)
        {
            try
            {                
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    var apiItem = CoreRepository.Products.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = product.Name;
                    apiItem.Description = product.Description;
                    apiItem.Parameters = product.Parameters;
                    apiItem.EndOfLife = product.EndOfLife;
                    apiItem.EntityKindId = product.EntityKindId;
                    apiItem.ValidFrom = product.ValidFrom;
                    apiItem.ValidUntil = product.ValidUntil;
                    apiItem.Type = product.Type;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    
                    apiItem = CoreRepository.Products.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                    return View(AutoMapper.Mapper.Map<Models.Core.Product>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(product);
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Product apiItem = null;
            try
            {
                apiItem = CoreRepository.Products.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Product>(apiItem));
            }
        }
        #endregion
    }
}
