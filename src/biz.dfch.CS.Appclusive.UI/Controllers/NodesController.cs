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

using biz.dfch.CS.Appclusive.UI.Config;
using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class NodesController : CoreControllerBase<Api.Core.Node, Models.Core.Node, Models.Core.Node>
    {
        protected override DataServiceQuery<Api.Core.Node> BaseQuery { get { return CoreRepository.Nodes.Expand("EntityKind"); } }

        // GET: Nodes/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            AddCheckNodePermissionObject(id);
            try
            {
                // load Node and Children
                var item = CoreRepository.Nodes.Expand("Parent").Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Node modelItem = AutoMapper.Mapper.Map<Models.Core.Node>(item);
                if (null != modelItem)
                {
                    modelItem.Children = LoadNodeChildren(id, 1);
                    modelItem.ResolveSecurity(this.CoreRepository);
                    try
                    {
                        modelItem.ResolveJob(this.CoreRepository);
                    }
                    catch (Exception ex) { ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex)); }
                }
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Node());
            }
        }
        
        #region Node-children list and search

        // GET: Nodes/ItemList
        public PartialViewResult ItemIndex(long parentId, int pageNr = 1, string itemSearchTerm = null, string orderBy = null)
        {
            ViewBag.ParentId = parentId;
            DataServiceQuery<Api.Core.Node> itemsBaseQuery = CoreRepository.Nodes;
            string itemsBaseFilter = "ParentId eq " + parentId;
            return base.ItemIndex<Api.Core.Node, Models.Core.Node>(itemsBaseQuery, itemsBaseFilter, pageNr, itemSearchTerm, orderBy);
        }

        private List<Models.Core.Node> LoadNodeChildren(long parentId, int pageNr)
        {
            QueryOperationResponse<Api.Core.Node> items = CoreRepository.Nodes
                    .AddQueryOption("$filter", "ParentId eq " + parentId)
                    .AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", PortalConfig.Pagesize)
                    .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                    .Execute() as QueryOperationResponse<Api.Core.Node>;

            ViewBag.ParentId = parentId;
            ViewBag.AjaxPaging = new PagingInfo(pageNr, items.TotalCount);

            return AutoMapper.Mapper.Map<List<Models.Core.Node>>(items);
        }

        public ActionResult ItemSearch(long parentId, string term)
        {
            DataServiceQuery<Api.Core.Node> itemsBaseQuery = CoreRepository.Nodes;
            string itemsBaseFilter = "ParentId eq " + parentId;
            return base.ItemSearch(itemsBaseQuery, itemsBaseFilter, term);
        }

        #endregion Node-children list

        [HttpPost]
        public PartialViewResult CheckPermission(Models.SpecialOperations.CheckPermission cp)
        {
            try
            {
                cp.ResolveNavigationProperties(this.CoreRepository);
                if (!ModelState.IsValid)
                {
                    return PartialView(cp);
                }
                else
                {
                    // TODO real validation through api
                    cp.Granted = cp.TrusteeType == 1;
                    return PartialView(cp);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return PartialView(cp);
            }
        }

        private void AddCheckNodePermissionObject(long nodeId)
        {
            var cp = new Models.SpecialOperations.CheckPermission()
            {
                NodeId = nodeId
            };
            cp.ResolveNavigationProperties(this.CoreRepository);
            ViewBag.CheckNodePermission = cp;
        }
    }
}
