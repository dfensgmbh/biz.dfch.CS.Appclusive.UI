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

using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Api_Diagnostics = biz.dfch.CS.Appclusive.Core.OdataServices.Diagnostics;
using M = biz.dfch.CS.Appclusive.UI.Models.Diagnostics.AuditTrail;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AuditTrailsController : DiagnosticsControllerBase<Api_Diagnostics.AuditTrail, Models.Diagnostics.AuditTrail>
    {
        protected override DataServiceQuery<Api_Diagnostics.AuditTrail> BaseQuery { get { return DiagnosticsRepository.AuditTrails; } }

        protected override DataServiceQuery<T> AddSearchFilter<T>(DataServiceQuery<T> query, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.AddQueryOption("$filter", string.Format("substringof('{0}',EntityType)", searchTerm));
            }
            return query;
        }

        protected override List<AjaxOption> CreateOptionList<T>(QueryOperationResponse<T> items)
        {
            List<AjaxOption> options = new List<AjaxOption>();
            foreach (var item in items)
            {
                Api_Diagnostics.AuditTrail audit = item as Api_Diagnostics.AuditTrail;
                options.Add(new AjaxOption(audit.Id, audit.EntityType));//string.Format("{0} - {1}", audit.EntityType, audit.Modified)));
            }
            return options;
        }


        // GET: AuditTrails/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;

            try
            {
                var item = BaseQuery.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<M>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Diagnostics.Endpoint());
            }
        }
    }
}