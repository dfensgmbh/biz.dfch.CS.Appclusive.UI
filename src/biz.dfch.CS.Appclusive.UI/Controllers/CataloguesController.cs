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
using System.Diagnostics.Contracts;
using System.Data.Services.Client;
using biz.dfch.CS.Appclusive.UI.Config;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CataloguesController : CoreControllerBase<Api.Core.Catalogue, Models.Core.Catalogue, Models.Core.CatalogueItem>
    {
        protected override DataServiceQuery<Api.Core.Catalogue> BaseQuery { get { return CoreRepository.Catalogues; } }
                
        #region Catalogue 

        // GET: Catalogues/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Catalogues.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Catalogue modelItem = AutoMapper.Mapper.Map<Models.Core.Catalogue>(item);
                if (null != modelItem)
                {
                    modelItem.CatalogueItems = LoadCatalogueItems(id, 1);
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Catalogue());
            }
        }

        // GET: Catalogues/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Catalogue());
        }

        // POST: Catalogues/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Catalogue catalogue)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(catalogue);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Catalogue>(catalogue);

                    CoreRepository.AddToCatalogues(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch(Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(catalogue);
            }
        }

        // GET: Catalogues/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Catalogues.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Catalogue modelItem = AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem);
                if (null != modelItem)
                {
                    modelItem.CatalogueItems = LoadCatalogueItems(id, 1);
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Catalogue());
            }
        }

        // POST: Catalogues/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Catalogue catalogue)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    catalogue.CatalogueItems = AutoMapper.Mapper.Map<List<Models.Core.CatalogueItem>>(CoreRepository.CatalogueItems.Where(ci => ci.CatalogueId == id).ToList());
                    return View(catalogue);
                }
                else
                {
                    var apiItem = CoreRepository.Catalogues.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = catalogue.Name;
                    apiItem.Description = catalogue.Description;
                    apiItem.Status = catalogue.Status;
                    apiItem.Version = catalogue.Version;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));

                    Models.Core.Catalogue modelItem = AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem);
                    if (null != modelItem)
                    {
                        modelItem.CatalogueItems = LoadCatalogueItems(id, 1);
                    }
                    return View(modelItem);
                }
            }
            catch(Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(catalogue);
            }
        }

        public ActionResult Delete(long id)
        { 
            Api.Core.Catalogue apiItem =null;
            try
            {
                apiItem = CoreRepository.Catalogues.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem));
            }
        }

        #endregion

        #region CatalogItems list and search

        // GET: Catalogues/ItemList
        public PartialViewResult ItemIndex(long catalogueId, int pageNr = 1, string itemSearchTerm = null, string orderBy = null)
        {
            ViewBag.ParentId = catalogueId;
            DataServiceQuery<Api.Core.CatalogueItem> itemsBaseQuery = CoreRepository.CatalogueItems;
            // .AddQueryOption("$filter", "CatalogueId eq " + catalogueId);
            string itemsBaseFilter = string.Format("CatalogueId eq {0}", catalogueId);
            return base.ItemIndex<Api.Core.CatalogueItem, Models.Core.CatalogueItem>(itemsBaseQuery, itemsBaseFilter, pageNr, itemSearchTerm, orderBy);
        }

        private List<Models.Core.CatalogueItem> LoadCatalogueItems(long catalogueId, int pageNr)
        {
            QueryOperationResponse<Api.Core.CatalogueItem> items = CoreRepository.CatalogueItems
                    .AddQueryOption("$filter", "CatalogueId eq " + catalogueId)
                    .AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", PortalConfig.Pagesize)
                    .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                    .Execute() as QueryOperationResponse<Api.Core.CatalogueItem>;

            ViewBag.ParentId = catalogueId;
            ViewBag.AjaxPaging = new PagingInfo(pageNr, items.TotalCount);

            return AutoMapper.Mapper.Map<List<Models.Core.CatalogueItem>>(items);
        }

        public ActionResult ItemSearch(long catalogueId, string term)
        {
            DataServiceQuery<Api.Core.CatalogueItem> itemsBaseQuery = CoreRepository.CatalogueItems;
            string itemsBaseFilter = "CatalogueId eq " + catalogueId; //           .AddQueryOption("$filter", "CatalogueId eq " + catalogueId);
            return base.ItemSearch(itemsBaseQuery, itemsBaseFilter, term);
        }

        #endregion CatalogItems list

        #region edit CatalogItems

        public ActionResult ItemDetails(long id)
        {
            try
            {
                var item = CoreRepository.CatalogueItems.Expand("Catalogue").Expand("Product").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CatalogueItem());
            }
        }

        public PartialViewResult AddToCart(long id, string elementId)
        {
            AjaxNotificationViewModel vm = new AjaxNotificationViewModel();
            try
            {
                var catalogueItem = CoreRepository.CatalogueItems.Expand("Catalogue").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                Models.Core.CartItem cartItem = new Models.Core.CartItem();
                cartItem.Name = catalogueItem.Name;
                cartItem.Description = catalogueItem.Description;
                cartItem.Quantity = 1;
                cartItem.CatalogueItemId = catalogueItem.Id;

                CoreRepository.AddToCartItems(AutoMapper.Mapper.Map<Api.Core.CartItem>(cartItem));
                CoreRepository.SaveChanges();
                vm.Level = ENotifyStyle.success;
                vm.Message = string.Format("Item {0} added to cart", catalogueItem.Name);
                vm.ElementId = elementId;

                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(vm);

                return PartialView("AddToCart");
            }
            catch (Exception ex)
            {
                return PartialView("AjaxNotification", ExceptionHelper.GetAjaxNotifications(ex));
            }
        }



        // GET: Catalogues/Create
        public ActionResult ItemCreate(int catalogueId)
        {
            try
            {
                Contract.Requires(catalogueId > 0);
                this.AddProductSeletionToViewBag();
                var apiCatalog = CoreRepository.Catalogues.Where(c => c.Id == catalogueId).FirstOrDefault();
                var catalog = AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiCatalog);
                Contract.Assert(null != catalog, "No catalog for item loaded");

                var modelItem = new Models.Core.CatalogueItem();
                modelItem.Catalogue = catalog;
                modelItem.CatalogueId = catalogueId;
                
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CatalogueItem());
            }
        }

        // POST: Catalogues/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult ItemCreate(Models.Core.CatalogueItem catalogueItem)
        {
            try
            {
                Contract.Requires(null != catalogueItem);
                this.AddProductSeletionToViewBag();
                if (!ModelState.IsValid)
                {
                    return View(catalogueItem);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.CatalogueItem>(catalogueItem);

                    CoreRepository.AddToCatalogueItems(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("ItemDetails", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                var apiCatalog = CoreRepository.Catalogues.Where(c => c.Id == catalogueItem.CatalogueId).FirstOrDefault();
                catalogueItem.Catalogue = AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiCatalog);
                return View(catalogueItem);
            }
        }

        // GET: Catalogues/Edit/5
        public ActionResult ItemEdit(long id)
        {
            try
            {
                Contract.Requires(id > 0);
                this.AddProductSeletionToViewBag();
                var apiItem = CoreRepository.CatalogueItems.Expand("Catalogue").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CatalogueItem());
            }
        }

        // POST: Catalogues/ItemEdit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult ItemEdit(long id, Models.Core.CatalogueItem catalogueItem)
        {
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != catalogueItem);
                this.AddProductSeletionToViewBag();

                if (!ModelState.IsValid)
                {
                    catalogueItem.Catalogue = AutoMapper.Mapper.Map<Models.Core.Catalogue>(CoreRepository.Catalogues.Where(ci => ci.Id == id).ToList());
                    return View(catalogueItem);
                }
                else
                {
                    var apiItem = CoreRepository.CatalogueItems.Expand("Catalogue").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                    Contract.Assert(null != apiItem);

                    #region copy all edited properties

                    apiItem.Name = catalogueItem.Name;
                    apiItem.ValidFrom = catalogueItem.ValidFrom;
                    apiItem.ValidUntil = catalogueItem.ValidUntil;
                    apiItem.EndOfLife = catalogueItem.EndOfLife;
                    apiItem.Description = catalogueItem.Description;
                    apiItem.Parameters = catalogueItem.Parameters;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    return View(catalogueItem);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(catalogueItem);
            }
        }

        public ActionResult ItemDelete(long id)
        {
            Api.Core.CatalogueItem apiItem = null;
            try
            {
                Contract.Requires(id > 0);
                apiItem = CoreRepository.CatalogueItems.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Contract.Assert(null != apiItem);
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Details", new { Id = apiItem.CatalogueId });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("ItemDetails", View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(apiItem)));
            }
        }

        #endregion

    }
}
