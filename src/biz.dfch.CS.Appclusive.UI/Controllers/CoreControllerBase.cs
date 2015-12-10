using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CoreControllerBase : Controller
    {

        /// <summary>
        /// biz.dfch.CS.Appclusive.Api.Core.Core
        /// </summary>
        protected biz.dfch.CS.Appclusive.UI._fake.CoreRepositoryMock CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.UI._fake.CoreRepositoryMock(new Uri(Properties.Settings.Default.AppculsiveApiCoreUrl));
                    coreRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    coreRepository.IgnoreMissingProperties = true;
                    coreRepository.Format.UseJson();
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.UI._fake.CoreRepositoryMock coreRepository;

        //private biz.dfch.CS.Appclusive.Api.Core.Core coreRepository;


        public CoreControllerBase()
            : base()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }

    }
}