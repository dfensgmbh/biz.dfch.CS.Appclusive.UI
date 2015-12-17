using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using biz.dfch.CS.Appclusive.UI.Models.SpecialOperations;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class SpecialOperationsController : CoreControllerBase
    {

        // GET: SpecialOperations
        public ActionResult Index()
        {
            return View();
        }

        // POST: SpecialOperations/SetCreatedBy
        [HttpPost]
        public PartialViewResult SetCreatedBy(SetCreatedByOperation vm)
        {
            if (ModelState.IsValid)
            {
                return HandleInvokeEntitySetActionWithoutVoidResult("SetCreatedBy", vm);
            }
            else
            {
                return PartialView(vm);
            }
        }

        // POST: SpecialOperations/SetTenant 
        [HttpPost]
        public PartialViewResult SetTenant(SetTenantOperation vm)
        {
            if (ModelState.IsValid)
            {
                return HandleInvokeEntitySetActionWithoutVoidResult("SetTenant", vm);
            }
            else
            {
                return PartialView(vm);
            }
        }

        // GET: SpecialOperations/ClearAuditLog
        public PartialViewResult ClearAuditLog()
        {
            return HandleInvokeEntitySetActionWithoutVoidResult("ClearAuditLog", null);
        }

        public PartialViewResult ReloadProducts()
        {
            return HandleInvokeEntitySetActionWithoutVoidResult("ReloadProducts", null);
        }

        public PartialViewResult RaiseUpdateConfigurationEvent()
        {
            return HandleInvokeEntitySetActionWithoutVoidResult("RaiseUpdateConfigurationEvent", null);
        }

        private PartialViewResult HandleInvokeEntitySetActionWithoutVoidResult(string action, IOperationParams inputViewModel)
        {
            try
            {
                CoreRepository.InvokeEntitySetActionWithVoidResult("SpecialOperations", action, inputViewModel != null ? inputViewModel.GetRequestPramsObject() : null);
                ViewBag.Notifications.Add(new AjaxNotificationViewModel(
                    ENotifyStyle.success,
                    string.Format("Operation {0} called successfully", action)
                    ));
                return PartialView(action, inputViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.Notifications = ExceptionHelper.GetAjaxNotifications(ex);
                return PartialView(action, inputViewModel);
            }
        }


    }
}
