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
    public abstract class DiagnosticsControllerBase<T, M> : GenericControllerBase<T, M, object>
    {

        /// <summary>
        /// Service context of query to load continuations
        /// </summary>
        override protected DataServiceContext Repository { get { return this.DiagnosticsRepository; } }

        protected biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics DiagnosticsRepository
        {
            get
            {
                if (diagnosticsRepository == null)
                {
                    diagnosticsRepository = new biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics(new Uri(Properties.Settings.Default.AppclusiveApiBaseUrl + "Diagnostics"));
                    diagnosticsRepository.IgnoreMissingProperties = true;
                    //diagnosticsRepository.Format.UseJson();
                    diagnosticsRepository.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
                    diagnosticsRepository.MergeOption = MergeOption.PreserveChanges;
                    diagnosticsRepository.TenantID = biz.dfch.CS.Appclusive.UI.Navigation.PermissionDecisions.Current.Tenant.Id.ToString();

                    System.Net.NetworkCredential apiCreds = Session["LoginData"] as System.Net.NetworkCredential;
                    Contract.Assert(null != apiCreds);
                    diagnosticsRepository.Credentials = apiCreds;
                }
                return diagnosticsRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Diagnostics.Diagnostics diagnosticsRepository;

    }
}