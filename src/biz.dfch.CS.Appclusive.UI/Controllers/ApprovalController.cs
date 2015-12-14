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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Diagnostics.Contracts;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

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
        public ActionResult Details(long id)
        {
            Models.Core.Approval approval = new Models.Core.Approval();
            try
            {
                Contract.Requires(id > 0);
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
                approval = AutoMapper.Mapper.Map<Models.Core.Approval>(apiItem);
                approval.ResolveOrderId(this.CoreRepository);
                return View(approval);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(approval);
            }
        }

        // GET: Approvals/Approve/5
        public ActionResult Approve(long id)
        {
            Models.Core.Approval approval = new Models.Core.Approval();
            try
            {
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
                approval = AutoMapper.Mapper.Map<Models.Core.Approval>(apiItem);
                approval.Status = Models.Core.Approval.APPROVED_STATUS_CHANGE;
                approval.HelpText = GeneralResources.HelpTextApprove;
                approval.ResolveOrderId(this.CoreRepository);
                return View("Edit", approval);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Edit", approval);
            }
        }

        // GET: Approvals/Decline/5
        public ActionResult Decline(long id)
        {
            Models.Core.Approval approval = new Models.Core.Approval();
            try
            {
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
                approval = AutoMapper.Mapper.Map<Models.Core.Approval>(apiItem);
                approval.Status = Models.Core.Approval.DECLINED_STATUS_CHANGE;
                approval.HelpText = GeneralResources.HelpTextDecline; 
                approval.ResolveOrderId(this.CoreRepository);
                return View("Edit", approval);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Edit", approval);
            }
        }

        // POST: Approvals/Approve/5
        [HttpPost]
        public ActionResult Approve(long id, Models.Core.Approval approval)
        {
            return Edit(id, approval);
        }
        // POST: Approvals/Decline/5
        [HttpPost]
        public ActionResult Decline(long id, Models.Core.Approval approval)
        {
            return Edit(id, approval);
        }

        private ActionResult Edit(long id, Models.Core.Approval approval)
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
                //TODO: once server api sends a readable response for approval updates use the outcommented lines instead
                // ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                // return View("Edit", approval);
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully " + approval.Status));
                return View("Details", approval);
            }
        }

        #endregion

    }
}
