using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AcesController : CoreControllerBase
    {

        // GET: Aces
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Ace> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Aces.Expand("Acl").Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Aces.Expand("Acl").Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                ViewBag.ShowEditLinks = true;
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
                var item = CoreRepository.Aces.Expand("Acl").Where(c => c.Id == id).FirstOrDefault();
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
            this.AddSeletionListsToViewBag();
            return View(new Models.Core.Ace());
        }

        // POST: Aces/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Ace Ace)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Ace>(Ace);

                CoreRepository.AddToAces(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddSeletionListsToViewBag();
                return View(Ace);
            }
        }

        // GET: Aces/Edit/5
        public ActionResult Edit(long id)
        {
            this.AddSeletionListsToViewBag();
            try
            {
                var apiItem = CoreRepository.Aces.Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult Edit(long id, Models.Core.Ace Ace)
        {
            this.AddSeletionListsToViewBag();
            try
            {
                var apiItem = CoreRepository.Aces.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = Ace.Name;
                apiItem.Description = Ace.Description;
                apiItem.Trustee = Ace.Trustee;
                apiItem.Action = Ace.Action;
                apiItem.AclId = Ace.AclId;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(Ace);
            }
        }

        // GET: Aces/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Ace apiItem = null;
            try
            {
                apiItem = CoreRepository.Aces.Expand("Acl").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddSeletionListsToViewBag();
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem));
            }
        }


        #endregion

        private void AddSeletionListsToViewBag()
        {
            try
            {
                // Action enum
                ViewBag.AceActionSelection = new SelectList(Enum.GetNames(typeof(Models.Core.AceActionEnum)));

                // ACL
                ViewBag.AclSelection = new SelectList(CoreRepository.Acls, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

        private void AddCustomerSeletionToViewBag()
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
    }
}
