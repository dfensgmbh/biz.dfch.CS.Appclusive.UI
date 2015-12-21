using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AcesController : CoreControllerBase
    {

        // GET: Aces
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                QueryOperationResponse<Api.Core.Ace> items = CoreRepository.Aces
                        .AddQueryOption("$inlinecount", "allpages")
                        .AddQueryOption("$top", PortalConfig.Pagesize)
                        .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                        .Execute() as QueryOperationResponse<Api.Core.Ace>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Ace>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Ace>());
            }
        }

        #region Ace

        // GET: Aces/Details/5
        public ActionResult Details(long id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Aces.Expand("Acl").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Ace>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Ace());
            }
        }

        // GET: Aces/Create
        public ActionResult Create()
        {
            this.AddAclSeletionToViewBag();
            return View(new Models.Core.Ace());
        }

        // POST: Aces/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Ace ace)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ace);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Ace>(ace);

                    CoreRepository.AddToAces(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddAclSeletionToViewBag();
                return View(ace);
            }
        }

        // GET: Aces/Edit/5
        public ActionResult Edit(long id)
        {
            this.AddAclSeletionToViewBag();
            try
            {
                var apiItem = CoreRepository.Aces.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Ace());
            }
        }

        // POST: Aces/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Ace ace)
        {
            this.AddAclSeletionToViewBag();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ace);
                }
                else
                {
                    var apiItem = CoreRepository.Aces.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = ace.Name;
                    apiItem.Description = ace.Description;
                    apiItem.Trustee = ace.Trustee;
                    apiItem.Action = ace.Action;
                    apiItem.AclId = ace.AclId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(ace);
            }
        }

        // GET: Aces/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Ace apiItem = null;
            try
            {
                apiItem = CoreRepository.Aces.Expand("Acl").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddAclSeletionToViewBag();
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem));
            }
        }


        #endregion
        
    }
}
