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
    public class CoreControllerBaseMock : Controller
    {

        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        protected biz.dfch.CS.Appclusive.UI._mocked.CoreRepositoryMock CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.UI._mocked.CoreRepositoryMock(new Uri(Properties.Settings.Default.AppculsiveApiCoreUrl));
                    coreRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    coreRepository.IgnoreMissingProperties = true;
                    coreRepository.Format.UseJson();
                    coreRepository.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.UI._mocked.CoreRepositoryMock coreRepository;

        public CoreControllerBaseMock()
            : base()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
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
    }
}