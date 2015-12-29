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

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ManagementUrisController : CoreControllerBase<Api.Core.ManagementUri, Models.Core.ManagementUri>
    {
        public ManagementUrisController()
        {
            base.BaseQuery = CoreRepository.ManagementUris;
        }

        #region ManagementUri

        // GET: ManagementUris/Details/5
        public ActionResult Details(long id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.ManagementUris.Expand("ManagementCredential").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ManagementUri());
            }
        }

        // GET: ManagementUris/Create
        public ActionResult Create()
        {
            this.AddManagementCredentialSelectionToViewBag();
            return View(new Models.Core.ManagementUri());
        }

        // POST: ManagementUris/Create
        [HttpPost]
        public ActionResult Create(Models.Core.ManagementUri managementUri)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(managementUri);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.ManagementUri>(managementUri);

                    CoreRepository.AddToManagementUris(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddManagementCredentialSelectionToViewBag();
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                Contract.Requires(id > 0);
                this.AddManagementCredentialSelectionToViewBag();
                var apiItem = CoreRepository.ManagementUris.Expand("ManagementCredential").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ManagementUri());
            }
        }

        // POST: ManagementUris/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.ManagementUri managementUri)
        {
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != managementUri);
                
                this.AddManagementCredentialSelectionToViewBag();
                if (!ModelState.IsValid)
                {
                    managementUri.ManagementCredential = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(CoreRepository.ManagementCredentials.Where(ci => ci.Id == id).ToList());
                    return View(managementUri);
                }
                else
                {
                    var apiItem = CoreRepository.ManagementUris.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = managementUri.Name;
                    apiItem.Description = managementUri.Description;
                    apiItem.Type = managementUri.Type;
                    apiItem.Value = managementUri.Value;
                    apiItem.ManagementCredentialId = managementUri.ManagementCredentialId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.ManagementUri apiItem = null;
            try
            {
                apiItem = CoreRepository.ManagementUris.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.ManagementUri>(apiItem));
            }
        }
        
        #endregion

    }
}
