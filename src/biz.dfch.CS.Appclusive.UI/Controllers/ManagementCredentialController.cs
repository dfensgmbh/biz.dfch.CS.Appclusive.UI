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

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ManagementCredentialsController : CoreControllerBase
    {

        // GET: ManagementCredentials
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                QueryOperationResponse<Api.Core.ManagementCredential> items = CoreRepository.ManagementCredentials
                        .AddQueryOption("$inlinecount", "allpages")
                        .AddQueryOption("$top", PortalConfig.Pagesize)
                        .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                        .Execute() as QueryOperationResponse<Api.Core.ManagementCredential>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                return View(AutoMapper.Mapper.Map<List<Models.Core.ManagementCredential>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.ManagementCredential>());
            }
        }

        #region ManagementCredential

        // GET: ManagementCredentials/Details/5
        public ActionResult Details(long id)
        {
            try
            {
                var item = CoreRepository.ManagementCredentials.Expand("ManagementUris").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(item));
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
                var apiItem = AutoMapper.Mapper.Map<Api.Core.ManagementCredential>(managementCredential);

                CoreRepository.AddToManagementCredentials(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
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
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem));
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
                var apiItem = CoreRepository.ManagementCredentials.Expand("ManagementUris").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = managementCredential.Name;
                apiItem.Description = managementCredential.Description;
                apiItem.Password = managementCredential.Password;
                apiItem.Username = managementCredential.Username;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                apiItem = CoreRepository.ManagementCredentials.Expand("ManagementUris").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault(); // because of data encryption
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem));
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem));
            }
        }


        #endregion

    }
}
