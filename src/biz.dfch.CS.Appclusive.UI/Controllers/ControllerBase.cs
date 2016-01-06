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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public abstract class ControllerBase : Controller
    {
        public ControllerBase(Type itemType)
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();

            this.SearchConfiguration = SearchConfig.GetConfig(this.GetType().Name);
            this.ItemSearchConfiguration = SearchConfig.GetConfig(itemType.Name);
        }

        EntityElement SearchConfiguration;
        EntityElement ItemSearchConfiguration;

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
                List<M> models = AutoMapper.Mapper.Map<List<M>>(items);
                models.ForEach(m => this.OnBeforeRender<M>(m));

                return View(models);
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
            query = AddSelectFilter(query, term);

            QueryOperationResponse<T> items = query.AddQueryOption("$top", PortalConfig.Searchsize).Execute() as QueryOperationResponse<T>;

            return this.Json(CreateOptionList(items), JsonRequestBehavior.AllowGet);
        }
        
        protected ActionResult Select<T>(DataServiceQuery<T> query, string term)
        {
            query = AddSearchFilter(query, term);
            query = AddSelectFilter(query, term);

            QueryOperationResponse<T> items = query.AddQueryOption("$top", PortalConfig.Searchsize).Execute() as QueryOperationResponse<T>;

            return this.Json(CreateOptionList(items, this.SearchConfiguration.Display, false), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// called on vm results before rendering
        /// </summary>
        /// <typeparam name="M"></typeparam>
        /// <param name="model"></param>
        protected virtual void OnBeforeRender<M>(M model)
        {
            // method to override
        }

        /// <summary>
        /// consider implementing AddSelectFilter and AddSearchFilter as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual List<AjaxOption> CreateOptionList<T>(QueryOperationResponse<T> items)
        {
            return CreateOptionList(items, this.SearchConfiguration.Display, true);
        }
  
        /// <summary>
        /// consider implementing AddSelectFilter and AddSearchFilter as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="displayStringFormat">"{Name} - {Created} ({Created})"</param>
        /// <param name="distinctValuesOnly"></param>
        /// <returns></returns>
        protected List<AjaxOption> CreateOptionList<T>(QueryOperationResponse<T> items, string displayStringFormat, bool distinctValuesOnly)
        {
            Contract.Requires(null != items);
            Contract.Requires(null != displayStringFormat);

            string keyPropertyName = "Id"; // key must be present for ODATA and it is always the property Id            
            System.Reflection.PropertyInfo propId = typeof(T).GetProperty(keyPropertyName);
            Contract.Assert(null != propId);

            // parse display string
            FormatStringExpression exp = new FormatStringExpression(displayStringFormat);

            List<System.Reflection.PropertyInfo> valueProps = new List<System.Reflection.PropertyInfo>();
            foreach (string valuePropertyName in exp.PropertyNames)
            {
                System.Reflection.PropertyInfo propName = typeof(T).GetProperty(valuePropertyName);
                if (null != propName)
                {
                    valueProps.Add(propName);
                }
            }

            List<AjaxOption> options = new List<AjaxOption>();

            foreach (var item in items)
            {
                List<object> values = new List<object>();
                valueProps.ForEach(p => values.Add(p.GetValue(item)));
                string value = string.Format(exp.FormatString, values.ToArray());
                if (distinctValuesOnly)
                {
                    if (null == options.FirstOrDefault(o => o.value == value))
                    {
                        options.Add(new AjaxOption(0, value));
                    }
                }
                else
                {
                    options.Add(new AjaxOption((long)propId.GetValue(item), value));
                }
            }
            return options.OrderBy(o => o.value).ToList();
        }

        #endregion

        #region basic query filters

        /// <summary>
        /// consider implementing CreateOptionList and AddSearchFilter as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        protected virtual DataServiceQuery<T> AddSelectFilter<T>(DataServiceQuery<T> query, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.AddQueryOption("$select", this.SearchConfiguration.Select); // key must be present for ODATA and it is always the property Id
            }
            return query;
        }

        /// <summary>
        /// consider implementing AddSelectFilter and CreateOptionList as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        protected virtual DataServiceQuery<T> AddSearchFilter<T>(DataServiceQuery<T> query, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.AddQueryOption("$filter", string.Format(this.SearchConfiguration.Filter, searchTerm));
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

        #region Item Search

        protected PartialViewResult ItemIndex<T, M>(DataServiceQuery<T> query, string baseFilter, int pageNr = 1, string itemSearchTerm = null)
        {
            ViewBag.ItemSearchTerm = itemSearchTerm;
            try
            {
                query = AddItemSearchFilter(query, baseFilter, itemSearchTerm);
                query = AddPagingOptions(query, pageNr);

                QueryOperationResponse<T> items = query.Execute() as QueryOperationResponse<T>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                ViewBag.AjaxPaging = new PagingInfo(pageNr, items.TotalCount);
                return PartialView(AutoMapper.Mapper.Map<List<M>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return PartialView(new List<M>());
            }
        }

        protected ActionResult ItemSearch<T>(DataServiceQuery<T> itemQuery, string baseFilter, string term)
        {
            itemQuery = AddItemSearchFilter(itemQuery, baseFilter, term);
            itemQuery = AddItemSelectFilter(itemQuery, term);

            QueryOperationResponse<T> items = itemQuery.AddQueryOption("$top", PortalConfig.Searchsize).Execute() as QueryOperationResponse<T>;

            return this.Json(CreateItemOptionList(items), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// consider implementing CreateItemOptionList and AddItemSearchFilter as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        protected virtual DataServiceQuery<T> AddItemSelectFilter<T>(DataServiceQuery<T> query, string searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.AddQueryOption("$select", this.ItemSearchConfiguration.Select);// key must be present for ODATA and it is always the property Id
            }
            return query;
        }

        /// <summary>
        /// consider implementing AddItemSelectFilter and CreateItemOptionList as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        protected virtual DataServiceQuery<T> AddItemSearchFilter<T>(DataServiceQuery<T> query, string baseFilter, string searchTerm)
        {
            string filter = baseFilter;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    filter = string.Format(this.ItemSearchConfiguration.Filter, searchTerm);
                }
                else
                {
                    filter = string.Format("{1} and " + this.ItemSearchConfiguration.Filter, searchTerm, filter);
                }
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.AddQueryOption("$filter", filter);
            }
            return query;
        }

        /// <summary>
        /// consider implementing AddItemSelectFilter and AddItemSearchFilter as well,
        /// otherwise you load the wrong properties..
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        protected virtual List<AjaxOption> CreateItemOptionList<T>(QueryOperationResponse<T> items)
        {
            return CreateOptionList(items, this.ItemSearchConfiguration.Display, true);
        }

        #endregion
    }
}