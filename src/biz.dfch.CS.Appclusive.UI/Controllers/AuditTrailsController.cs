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

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AuditTrailsController : DiagnosticsControllerBase
    {
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                QueryOperationResponse<Api.Diagnostics.AuditTrail> items = DiagnosticsRepository.AuditTrails
                        .AddQueryOption("$inlinecount", "allpages")
                        .AddQueryOption("$top", PortalConfig.Pagesize)
                        .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                        .Execute() as QueryOperationResponse<Api.Diagnostics.AuditTrail>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                return View(AutoMapper.Mapper.Map<List<Models.Diagnostics.AuditTrail>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Diagnostics.AuditTrail>());
            }
        }

        public ActionResult Details(long id)
        {
            try
            {
                var item = DiagnosticsRepository.AuditTrails.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Diagnostics.AuditTrail>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Diagnostics.AuditTrail());
            }
        }

    }
}