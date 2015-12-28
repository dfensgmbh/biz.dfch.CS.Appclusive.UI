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
    public class NodesController : CoreControllerBase<Api.Core.Node, Models.Core.Node>
    {
        public NodesController()
        {
            base.BaseQuery = CoreRepository.Nodes;
        }

        // GET: Nodes/Details/5
        public ActionResult Details(long id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Nodes.Expand("Children").Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                // find job to node
                var job = CoreRepository.Jobs.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(j => j.ReferencedItemId == id.ToString() && j.EntityKind.Name == Models.Core.EntityKind.NODE_ENTITYKIND_NAME).FirstOrDefault();
                ViewBag.NodeJob = AutoMapper.Mapper.Map<Models.Core.Job>(job);
                
                return View(AutoMapper.Mapper.Map<Models.Core.Node>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Node());
            }
        }

    }
}
