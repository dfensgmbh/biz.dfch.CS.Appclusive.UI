﻿/**
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        #region basic list actions

        protected ActionResult Index<T,M>(DataServiceQuery<T> query, int pageNr = 1, string searchTerm = null)
        {
            ViewBag.SearchTerm = searchTerm;
            try
            {
                query = AddSearchFilter(query, searchTerm);
                query = AddPagingOptions(query, pageNr);

                QueryOperationResponse<T> items = query.Execute() as QueryOperationResponse<T>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                return View(AutoMapper.Mapper.Map<List<M>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<M>());
            }
        }

        protected ActionResult Search<T>(DataServiceQuery<T> query, string term)
        {
            query = AddSearchFilter(query, term);

            QueryOperationResponse<T> items = query.AddQueryOption("$top", PortalConfig.Searchsize).Execute() as QueryOperationResponse<T>;
            return this.Json(CreateOptionList(items), JsonRequestBehavior.AllowGet);
        }

        protected virtual List<AjaxOption> CreateOptionList<T>(QueryOperationResponse<T> items)
        {
            System.Reflection.PropertyInfo propId = typeof(T).GetProperty("Id");
            System.Reflection.PropertyInfo propName = typeof(T).GetProperty("Name");
            Contract.Assert(null != propId);
            Contract.Assert(null != propName);

            List<AjaxOption> options = new List<AjaxOption>();
            foreach (var item in items)
            {
                options.Add(new AjaxOption((long)propId.GetValue(item), (string)propName.GetValue(item)));
            }
            return options;
        }

        #endregion

        #region basic query filters

        protected virtual DataServiceQuery<T> AddSearchFilter<T>(DataServiceQuery<T> query, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.AddQueryOption("$filter", string.Format("substringof('{0}',Name)", searchTerm));
            }
            return query;
        }

        protected DataServiceQuery<T> AddPagingOptions<T>(DataServiceQuery<T> query, int pageNr)
        {
            if (pageNr>0)
            {
                query = query.AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", PortalConfig.Pagesize)
                    .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize);
            }
            return query;
        }
        #endregion

    }
}