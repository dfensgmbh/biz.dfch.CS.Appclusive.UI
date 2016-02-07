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
    public class ManagementUrisController : CoreControllerBase<Api.Core.ManagementUri, Models.Core.ManagementUri, object>
    {
        protected override DataServiceQuery<Api.Core.ManagementUri> BaseQuery { get { return CoreRepository.ManagementUris; } }

        #region ManagementUri

        // GET: ManagementUris/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
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
        public ActionResult Create(long managementCredentialId = 0, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;

            Models.Core.ManagementUri uri = new Models.Core.ManagementUri()
            {
                ManagementCredentialId = managementCredentialId
            };
            if (managementCredentialId > 0)
            {
                uri.ManagementCredential = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(this.CoreRepository.ManagementCredentials.Where(mc => mc.Id == managementCredentialId).FirstOrDefault());
            }
            return View(uri);
        }

        // POST: ManagementUris/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Models.Core.ManagementUri managementUri, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
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

                    return RedirectToAction("Details", new { id = apiItem.Id, rId = rId, rAction = rAction, rController = rController });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Edit/5
        public ActionResult Edit(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                Contract.Requires(id > 0);
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
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(long id, Models.Core.ManagementUri managementUri, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != managementUri);
                
                if (!ModelState.IsValid)
                {
                    managementUri.ManagementCredential = AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(CoreRepository.ManagementCredentials.Where(ci => ci.Id == id).ToList());
                    return View(managementUri);
                }
                else
                {
                    var apiItem = CoreRepository.ManagementUris.Expand("ManagementCredential").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = managementUri.Name;
                    apiItem.Description = managementUri.Description;
                    apiItem.Type = managementUri.Type;
                    apiItem.Value = managementUri.Value;
                    apiItem.ManagementCredentialId = managementUri.ManagementCredentialId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, biz.dfch.CS.Appclusive.UI.App_LocalResources.GeneralResources.SuccessfullySaved));
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
        public ActionResult Delete(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            Api.Core.ManagementUri apiItem = null;
            try
            {
                apiItem = CoreRepository.ManagementUris.Expand("ManagementCredential").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                if (string.IsNullOrEmpty(rId) || string.IsNullOrEmpty(rAction) || string.IsNullOrEmpty(rController))
                {
                    return RedirectToAction("Index", new { d = id });
                }
                else
                {
                    return RedirectToAction(rAction, rController, new { id = rId, d = id });
                }
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
