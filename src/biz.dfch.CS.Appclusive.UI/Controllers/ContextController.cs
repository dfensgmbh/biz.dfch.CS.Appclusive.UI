using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Managers;

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

        protected Api.Core.Core CoreRepository
        {
            get { return coreRepository ?? (coreRepository = new AuthenticatedCoreApi()); }
        }
        private Api.Core.Core coreRepository;
    }
}