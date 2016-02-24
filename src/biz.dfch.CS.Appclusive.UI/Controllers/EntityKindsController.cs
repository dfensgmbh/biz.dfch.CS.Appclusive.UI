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

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class EntityKindsController : CoreControllerBase<Api.Core.EntityKind, Models.Core.EntityKind, object>
    {
        protected override DataServiceQuery<Api.Core.EntityKind> BaseQuery { get { return CoreRepository.EntityKinds; } }

        #region EntityKind

        // GET: EntityKinds/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.EntityKinds.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.EntityKind>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.EntityKind());
            }
        }

        // GET: EntityKinds/Create
        public ActionResult Create()
        {
            return View(new Models.Core.EntityKind());
        }

        // POST: EntityKinds/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Models.Core.EntityKind entityKind)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entityKind);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.EntityKind>(entityKind);

                    CoreRepository.AddToEntityKinds(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(entityKind);
            }
        }

        // GET: EntityKinds/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.EntityKinds.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.EntityKind>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.EntityKind());
            }
        }

        // POST: EntityKinds/Edit/5
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(long id, Models.Core.EntityKind entityKind)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(entityKind);
                }
                else
                {
                    var apiItem = CoreRepository.EntityKinds.Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = entityKind.Name;
                    apiItem.Description = entityKind.Description;
                    apiItem.Parameters = entityKind.Parameters;
                    apiItem.Version = entityKind.Version;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    return View(AutoMapper.Mapper.Map<Models.Core.EntityKind>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(entityKind);
            }
        }

        // GET: EntityKinds/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.EntityKind apiItem = null;
            try
            {
                apiItem = CoreRepository.EntityKinds.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.EntityKind>(apiItem));
            }
        }


        #endregion

    }
}
