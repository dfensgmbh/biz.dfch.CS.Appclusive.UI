using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ApprovalsController : CoreControllerBase
    {       
        // GET: Approvals
        public ActionResult Index(int page = 1, string status = null)
        {
            try
            {
                IEnumerable<Api.Core.Approval> items;
                if (string.IsNullOrWhiteSpace(status))
                {
                    int skipCount = page > 1 ? (page - 1) * PortalConfig.Pagesize : 0;
                    items = CoreRepository.Approvals.Skip(skipCount).Take(PortalConfig.Pagesize).ToList();
                    ViewBag.StatusFilter = status;
                }
                else
                {
                    items = CoreRepository.Approvals.Where(a => a.Status == status);
                }
                switch (status)
                {
                    case "Created": ViewBag.CreatedLinkClass = "active"; break;
                    case "Approved": ViewBag.ApprovedLinkClass = "active"; break;
                    case "Declined": ViewBag.DeclinedLinkClass = "active"; break;
                    default: ViewBag.AllLinkClass = "active"; break;
                }
                return View(AutoMapper.Mapper.Map<List<Models.Core.Approval>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Approval>());
            }
        }

        #region Approval

        // GET: Approvals/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Approval>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Approval());
            }
        }

        // GET: Approvals/Approve/5
        public ActionResult Approve(int id)
        {
            try
            {
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Approval approval = AutoMapper.Mapper.Map<Models.Core.Approval>(apiItem);
                approval.Status = Models.Core.Approval.APPROVED_STATUS_CHANGE;
                approval.HelpText = "The request will be approved when you click the 'Approve' button. You can optionally add a explanation or reason for approval.";
                return View("Edit", approval);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Edit", new Models.Core.Approval());
            }
        }

        // GET: Approvals/Decline/5
        public ActionResult Decline(int id)
        {
            try
            {
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Approval approval = AutoMapper.Mapper.Map<Models.Core.Approval>(apiItem);
                approval.Status = Models.Core.Approval.DECLINED_STATUS_CHANGE;
                approval.HelpText = "The request will be declined when you click the 'Decline' button. You can optionally add a explanation or reason for approval.";
                return View("Edit", approval);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Edit", new Models.Core.Approval());
            }
        }

        // POST: Approvals/Approve/5
        [HttpPost]
        public ActionResult Approve(int id, Models.Core.Approval approval)
        {
            return Edit(id, approval);
        }
        // POST: Approvals/Decline/5
        [HttpPost]
        public ActionResult Decline(int id, Models.Core.Approval approval)
        {
            return Edit(id, approval);
        }

        private ActionResult Edit(int id, Models.Core.Approval approval)
        {
            try
            {
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Status = approval.Status;
                apiItem.Description = approval.Description;

                #endregion

                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("Edit", approval);
            }
        }


        #endregion

    }
}
