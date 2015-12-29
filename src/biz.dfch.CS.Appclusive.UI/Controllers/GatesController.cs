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
    public class GatesController  : CoreControllerBase<Api.Core.Gate, Models.Core.Gate>
    {
        protected override DataServiceQuery<Api.Core.Gate> BaseQuery { get { return CoreRepository.Gates; } }
        
        #region Gate

        // GET: Gates/Details/5
        public ActionResult Details(long id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Gates.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Gate>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.EntityKind());
            }
        }

        // GET: Gates/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Gate());
        }

        // POST: Gates/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Gate gate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(gate);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Gate>(gate);

                    CoreRepository.AddToGates(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(gate);
            }
        }

        // GET: Gates/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Gates.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Gate());
            }
        }

        // POST: Gates/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Gate gate)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(gate);
                }
                else
                {
                    var apiItem = CoreRepository.Gates.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = gate.Name;
                    apiItem.Description = gate.Description;
                    apiItem.Parameters = gate.Parameters;
                    apiItem.Status = gate.Status;
                    apiItem.Type = gate.Type;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(gate);
            }
        }

        // GET: Gates/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Gate apiItem = null;
            try
            {
                apiItem = CoreRepository.Gates.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
            }
        }


        #endregion

    }
}
