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
                var products = CoreRepository.Products.ToList();
                ViewBag.ProductSelection = new SelectList(products, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }
        
        //protected void AddEntityKindSeletionToViewBag()
        //{
        //    try
        //    {
        //        var entityKinds = CoreRepository.EntityKinds.ToList();
        //        ViewBag.EntityKindSelection = new SelectList(entityKinds, "Id", "Name");
        //    }
        //    catch (Exception ex)
        //    {
        //        ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
        //    }
        //}

        protected void AddManagementCredentialSelectionToViewBag()
        {
            try
            {
                List<Api.Core.ManagementCredential> creds = new List<Api.Core.ManagementCredential>();
                creds.Add(new Api.Core.ManagementCredential() { Name = "-" });
                creds.AddRange(CoreRepository.ManagementCredentials.ToList());

                ViewBag.ManagementCredentialSelection = new SelectList(creds.Select(u => { return new { Id = u.Id > 0 ? (long?)u.Id : null, Name = u.Name }; }), "Id", "Name");
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
                List<Api.Core.Tenant> tenants = new List<Api.Core.Tenant>();
                if (includeEmpty)
                {
                    tenants.Add(new Api.Core.Tenant());
                }
                if (null == currentTenant || currentTenant.ParentId == currentTenant.Id)// special seed entry in DB
                {
                    tenants.AddRange(CoreRepository.Tenants);
                }
                else
                {
                    tenants.AddRange(CoreRepository.Tenants.Where(t => t.Id != currentTenant.Id));
                }

                ViewBag.TenantSelection = new SelectList(AutoMapper.Mapper.Map<List<Models.Core.Tenant>>(tenants), "IdStr", "Id");
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
                customers.AddRange(CoreRepository.Customers);

                ViewBag.CustomerSelection = new SelectList(customers, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

        protected void AddAclSeletionToViewBag()
        {
            try
            {
                // ACL
                ViewBag.AclSelection = new SelectList(CoreRepository.Acls, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }
        
        #endregion
    }
}