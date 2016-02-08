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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">API-Type</typeparam>
    /// <typeparam name="M">(View-) Model-Type</typeparam>
    /// <typeparam name="I">ModelItem-Type (if no items: object)</typeparam>
    public abstract class CoreControllerBase<T, M, I> : GenericControllerBase<T, M, I>
    {
        /// <summary>
        /// Service context of query to load continuations
        /// </summary>
        override protected DataServiceContext Repository { get { return this.CoreRepository; } }

        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        protected biz.dfch.CS.Appclusive.Api.Core.Core CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.Api.Core.Core(new Uri(Properties.Settings.Default.AppculsiveApiBaseUrl + "Core"));
                    coreRepository.IgnoreMissingProperties = true;
                    coreRepository.Format.UseJson();
                    coreRepository.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
                    coreRepository.MergeOption = MergeOption.PreserveChanges;
                    coreRepository.TenantID = biz.dfch.CS.Appclusive.UI.Navigation.PermissionDecisions.Current.Tenant.Id.ToString();

                    System.Net.NetworkCredential apiCreds = Session["LoginData"] as System.Net.NetworkCredential;
                    if (null != apiCreds)
                    {
                        coreRepository.Credentials = apiCreds;
                    }
                    else
                    {
                        coreRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    }
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Core.Core coreRepository;
        
        #region selection lists

        protected void AddProductSeletionToViewBag()
        {
            try
            {
                List<Api.Core.Product> products = new List<Api.Core.Product>();

                #region load all products

                var query = coreRepository.Products.AddQueryOption("$top", 10000);
                QueryOperationResponse<Api.Core.Product> queryResponse = query.Execute() as QueryOperationResponse<Api.Core.Product>;
                while (null != queryResponse)
                {
                    products.AddRange(queryResponse.ToList());
                    DataServiceQueryContinuation<Api.Core.Product> cont = queryResponse.GetContinuation();
                    if (null != cont)
                    {
                        queryResponse = coreRepository.Execute<Api.Core.Product>(cont) as QueryOperationResponse<Api.Core.Product>;
                    }
                    else
                    {
                        queryResponse = null;
                    }
                }

                #endregion

                ViewBag.ProductSelection = new SelectList(products, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }
        
        protected void AddTenantSeletionToViewBag(Api.Core.Tenant currentTenant, bool includeEmpty = false)
        {
            try
            {
                List<Models.Core.Tenant> tenants = new List<Models.Core.Tenant>();
                if (includeEmpty)
                {
                    tenants.Add(new Models.Core.Tenant());
                }
                #region load all tenants

                List<Models.Core.Tenant> allTenants = new List<Models.Core.Tenant>();
                var query = coreRepository.Tenants.AddQueryOption("$top", 10000);
                QueryOperationResponse<Api.Core.Tenant> queryResponse = query.Execute() as QueryOperationResponse<Api.Core.Tenant>;
                while (null != queryResponse)
                {
                    allTenants.AddRange(AutoMapper.Mapper.Map<List<Models.Core.Tenant>>(queryResponse.ToList()));
                    DataServiceQueryContinuation<Api.Core.Tenant> cont = queryResponse.GetContinuation();
                    if (null != cont)
                    {
                        queryResponse = coreRepository.Execute<Api.Core.Tenant>(cont) as QueryOperationResponse<Api.Core.Tenant>;
                    }
                    else
                    {
                        queryResponse = null;
                    }
                }

                #endregion
                if (null == currentTenant || currentTenant.ParentId == currentTenant.Id)// special seed entry in DB
                {
                    tenants.AddRange(allTenants);
                }
                else
                {
                    tenants.AddRange(allTenants.Where(t => t.Id != currentTenant.Id));
                }

                ViewBag.TenantSelection = new SelectList(tenants, "IdStr", "Id");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

        protected void AddCustomerSeletionToViewBag()
        {
            try
            {
                List<Api.Core.Customer> customers = new List<Api.Core.Customer>();
                customers.Add(new Api.Core.Customer());

                #region load all customer

                var query = coreRepository.Customers.AddQueryOption("$top", 10000);
                QueryOperationResponse<Api.Core.Customer> queryResponse = query.Execute() as QueryOperationResponse<Api.Core.Customer>;
                while (null != queryResponse)
                {
                    customers.AddRange(queryResponse.ToList());
                    DataServiceQueryContinuation<Api.Core.Customer> cont = queryResponse.GetContinuation();
                    if (null != cont)
                    {
                        queryResponse = coreRepository.Execute<Api.Core.Customer>(cont) as QueryOperationResponse<Api.Core.Customer>;
                    }
                    else
                    {
                        queryResponse = null;
                    }
                }

                #endregion

                ViewBag.CustomerSelection = new SelectList(customers, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }
        
        #endregion
    }
}