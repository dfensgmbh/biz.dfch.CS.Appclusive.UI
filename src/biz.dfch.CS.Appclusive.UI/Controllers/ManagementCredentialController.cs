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
using System.Data.Services.Client;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ManagementCredentialsController : CoreControllerBase<Api.Core.ManagementCredential, Models.Core.ManagementCredential, object>
    {
        protected override DataServiceQuery<Api.Core.ManagementCredential> BaseQuery { get { return CoreRepository.ManagementCredentials; } }

        #region ManagementCredential

        // GET: ManagementCredentials/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null, int d = 0)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            #region delete message
            if (d > 0)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, string.Format(GeneralResources.ConfirmDeleted, d)));
            }
            #endregion
            try
            {
                var item = CoreRepository.ManagementCredentials.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.ManagementCredential modelItem = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(item);
                if (null != modelItem)
                {
                    modelItem.ManagementUris = LoadManagementUris(id, 1);
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ManagementCredential());
            }
        }

        // GET: ManagementCredentials/Create
        public ActionResult Create()
        {
            return View(new Models.Core.ManagementCredential());
        }

        // POST: ManagementCredentials/Create
        [HttpPost]
        public ActionResult Create(Models.Core.ManagementCredential managementCredential)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(managementCredential);
                }
                else
                {
                    managementCredential.EncryptedPassword = managementCredential.Password;
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.ManagementCredential>(managementCredential);

                    CoreRepository.AddToManagementCredentials(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementCredential);
            }
        }

        // GET: ManagementCredentials/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.ManagementCredentials.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.ManagementCredential modelItem = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem);
                if (null != modelItem)
                {
                    modelItem.ManagementUris = LoadManagementUris(id, 1);
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ManagementCredential());
            }
        }

        // POST: ManagementCredentials/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.ManagementCredential managementCredential)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    if (null != managementCredential)
                    {
                        managementCredential.ManagementUris = LoadManagementUris(id, 1);
                    }
                    return View(managementCredential);
                }
                else
                {
                    var apiItem = CoreRepository.ManagementCredentials.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = managementCredential.Name;
                    apiItem.Description = managementCredential.Description;
                    apiItem.Password = managementCredential.Password;
                    apiItem.Username = managementCredential.Username;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    apiItem = CoreRepository.ManagementCredentials.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault(); // because of data encryption
                    Models.Core.ManagementCredential modelItem = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem);
                    if (null != modelItem)
                    {
                        modelItem.ManagementUris = LoadManagementUris(id, 1);
                    }
                    return View(modelItem);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementCredential);
            }
        }

        // GET: ManagementCredentials/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.ManagementCredential apiItem = null;
            try
            {
                apiItem = CoreRepository.ManagementCredentials.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                Models.Core.ManagementCredential modelItem = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem);
                if (null != modelItem)
                {
                    modelItem.ManagementUris = LoadManagementUris(id, 1);
                }
                return View("Details", modelItem);
            }
        }


        #endregion

        #region ManagementUris list and search
        // GET: Nodes/ItemList
        public PartialViewResult ItemIndex(long managementCredentialId, int skip = 0, string itemSearchTerm = null, string orderBy = null)
        {
            ViewBag.ParentId = managementCredentialId;
            DataServiceQuery<Api.Core.ManagementUri> itemsBaseQuery = CoreRepository.ManagementUris;
            string itemsBaseFilter = "ManagementCredentialId eq " + managementCredentialId;
            return base.ItemIndex<Api.Core.ManagementUri, Models.Core.ManagementUri>(itemsBaseQuery, itemsBaseFilter, skip, itemSearchTerm, orderBy);
        }

        private List<Models.Core.ManagementUri> LoadManagementUris(long managementCredentialId, int skip)
        {
            QueryOperationResponse<Api.Core.ManagementUri> items = CoreRepository.ManagementUris
                    .AddQueryOption("$filter", "ManagementCredentialId eq " + managementCredentialId)
                    .AddQueryOption("$skip", skip)
                    .Execute() as QueryOperationResponse<Api.Core.ManagementUri>;

            var result = AutoMapper.Mapper.Map<List<Models.Core.ManagementUri>>(items);

            ViewBag.ParentId = managementCredentialId;
            ViewBag.AjaxPaging = CreatePageFilterInfo(items);

            return result;
        }

        public ActionResult ItemSearch(long managementCredentialId, string term)
        {
            DataServiceQuery<Api.Core.ManagementUri> itemsBaseQuery = CoreRepository.ManagementUris;
            string itemsBaseFilter = "ManagementCredentialId eq " + managementCredentialId; 
            return base.ItemSearch(itemsBaseQuery, itemsBaseFilter, term);
        }

        #endregion 

    }
}
