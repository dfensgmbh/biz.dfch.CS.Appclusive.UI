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
    public class ContextController : Controller
    {
        public ContextController()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }

        // GET: Context
        public ActionResult Index(string tenantId, string returnUrl=null)
        {
            ViewBag.ReturnUrl = returnUrl;
            try
            {
                Models.Core.Tenant tenant = null;
                Guid tid = Guid.Parse(tenantId);
                if (tid == Guid.Empty)
                {
                    tenant = new Models.Core.Tenant() { Id = tid, Name = "" };
                }
                else
                {
                    tenant = AutoMapper.Mapper.Map<Models.Core.Tenant>(CoreRepository.Tenants.Where(t => t.Id == tid).FirstOrDefault());
                }
                if (null != tenant)
                {
                    biz.dfch.CS.Appclusive.UI.Navigation.PermissionDecisions.Current.Tenant = tenant;
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
            return View();
        }

        protected biz.dfch.CS.Appclusive.Api.Core.Core CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.Api.Core.Core(new Uri(Properties.Settings.Default.AppclusiveApiBaseUrl + "Core"));
                    coreRepository.IgnoreMissingProperties = true;
                    coreRepository.Format.UseJson();
                    coreRepository.SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate;
                    coreRepository.MergeOption = MergeOption.PreserveChanges;
                   
                    System.Net.NetworkCredential apiCreds = Session["LoginData"] as System.Net.NetworkCredential;
                    Contract.Assert(null != apiCreds);
                    coreRepository.Credentials = apiCreds;
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Core.Core coreRepository;
    }
}