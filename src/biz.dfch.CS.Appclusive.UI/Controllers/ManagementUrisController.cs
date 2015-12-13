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

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ManagementUrisController : CoreControllerBase
    {

        // GET: ManagementUris
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.ManagementUri> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.ManagementUris.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.ManagementUris.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.ManagementUri>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.ManagementUri>());
            }
        }

        #region ManagementUri

        // GET: ManagementUris/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.ManagementUris.Expand("ManagementCredential").Where(c => c.Id == id).FirstOrDefault();
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
                var apiItem = AutoMapper.Mapper.Map<Api.Core.ManagementUri>(managementUri);

                CoreRepository.AddToManagementUris(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddManagementCredentialSelectionToViewBag();
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Contract.Requires(id > 0);
                this.AddManagementCredentialSelectionToViewBag();
                var apiItem = CoreRepository.ManagementUris.Expand("ManagementCredential").Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult Edit(int id, Models.Core.ManagementUri managementUri)
        {
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != managementUri);
                this.AddManagementCredentialSelectionToViewBag();
                var apiItem = CoreRepository.ManagementUris.Where(c => c.Id == id).FirstOrDefault();

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
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.ManagementUri apiItem = null;
            try
            {
                apiItem = CoreRepository.ManagementUris.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(apiItem)));
            }
        }


        private void AddManagementCredentialSelectionToViewBag()
        {
            try
            {
                List<Api.Core.ManagementCredential> creds = new List<Api.Core.ManagementCredential>();
                creds.Add(new Api.Core.ManagementCredential() { Name = "-" });
                creds.AddRange(CoreRepository.ManagementCredentials.ToList());

                ViewBag.ManagementCredentialSelection = new SelectList(creds.Select(u => { return new { Id = u.Id > 0 ? (long?)u.Id : null, Name = u.Name }; }), "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

        #endregion

    }
}
