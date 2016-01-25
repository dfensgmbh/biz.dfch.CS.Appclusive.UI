using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AcesController : CoreControllerBase<Api.Core.Ace, Models.Core.Ace, object>
    {
        protected override DataServiceQuery<Api.Core.Ace> BaseQuery { get { return CoreRepository.Aces; } }
        
        protected override void OnBeforeRender<M>(M model)
        {
            Models.Core.Ace m = model as Models.Core.Ace;
            try
            {
                m.ResolveNavigationProperties(this.CoreRepository);
            }
            catch (Exception ex) { ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex)); }
        }

        #region Ace

        // GET: Aces/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Aces.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                var model = AutoMapper.Mapper.Map<Models.Core.Ace>(item);
                model.ResolveNavigationProperties(this.CoreRepository);
                return View(model);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Ace());
            }
        }

        // GET: Aces/Create
        public ActionResult Create(long aclId = 0)
        {
            this.AddAclSeletionToViewBag();
            Models.Core.Ace ace = new Models.Core.Ace(){
                AclId = aclId,
                Type = Models.Core.AceTypeEnum.ALLOW.GetHashCode()
            };
            return View(ace);
        }

        // POST: Aces/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Ace ace)
        {
            try
            {
                this.AddAclSeletionToViewBag();
                if (!ModelState.IsValid)
                {
                    return View(ace);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Ace>(ace);

                    CoreRepository.AddToAces(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", "Acls", new { id = apiItem.AclId });
                    //return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                ace.ResolveNavigationProperties(CoreRepository);
                return View(ace);
            }
        }

        // GET: Aces/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Aces.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                var model = AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem);
                model.ResolveNavigationProperties(this.CoreRepository);
                return View(model);
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
            try
            {
                if (!ModelState.IsValid)
                {
                    ace.ResolveNavigationProperties(this.CoreRepository);
                    return View(ace);
                }
                else
                {
                    var apiItem = CoreRepository.Aces.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = ace.Name;
                    apiItem.Description = ace.Description;
                    apiItem.TrusteeId = ace.TrusteeId;
                    apiItem.TrusteeType = ace.TrusteeType;
                    apiItem.Type = ace.Type;
                    apiItem.PermissionId = ace.PermissionId;
                    apiItem.TrusteeType = ace.TrusteeType;
                    apiItem.AclId = ace.AclId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));

                    return RedirectToAction("Details", "Acls", new { id = apiItem.AclId });
                    //var model = AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem);
                    //model.ResolveNavigationProperties(this.CoreRepository);
                    //return View(model);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                ace.ResolveNavigationProperties(CoreRepository);
                return View(ace);
            }
        }

        // GET: Aces/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Ace apiItem = null;
            try
            {
                apiItem = CoreRepository.Aces.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Details", "Acls", new { id = apiItem.AclId, d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                var model = AutoMapper.Mapper.Map<Models.Core.Ace>(apiItem);
                if (apiItem != null)
                {
                    model.ResolveNavigationProperties(this.CoreRepository);
                    return View("Details", model);
                }
                else
                {
                    return View("Details", new Models.Core.Ace());
                }
            }
        }


        #endregion
        
    }
}
