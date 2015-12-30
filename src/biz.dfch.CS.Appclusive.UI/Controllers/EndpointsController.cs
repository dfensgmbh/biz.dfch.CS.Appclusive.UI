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
using Api_Diagnostics = biz.dfch.CS.Appclusive.Core.OdataServices.Diagnostics;
using M = biz.dfch.CS.Appclusive.UI.Models.Diagnostics.Endpoint;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class EndpointsController : DiagnosticsControllerBase<Api_Diagnostics.Endpoint, Models.Diagnostics.Endpoint>
    {
        protected override DataServiceQuery<Api_Diagnostics.Endpoint> BaseQuery { get { return DiagnosticsRepository.Endpoints; } }

        protected override DataServiceQuery<T> AddSearchFilter<T>(DataServiceQuery<T> query, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.AddQueryOption("$filter", string.Format("substringof('{0}',tolower(Name))", searchTerm.ToLower()));
            }
            return query;
        }

        // GET: Endpoints/Details/5
        public ActionResult Details(long id, long rId = 0, string rAction = null, string rController = null)
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
