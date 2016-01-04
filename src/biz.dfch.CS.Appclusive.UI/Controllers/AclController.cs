using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AclsController : CoreControllerBase<Api.Core.Acl, Models.Core.Acl>
    {
        protected override DataServiceQuery<Api.Core.Acl> BaseQuery { get { return CoreRepository.Acls; } }
        
        #region Acl

        // GET: Acls/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Acls.Expand("EntityKind").Expand("Aces").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Acl>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Acl());
            }
        }

        // GET: Acls/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Acl());
        }

        // POST: Acls/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Acl acl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(acl);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Acl>(acl);

                    CoreRepository.AddToAcls(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(acl);
            }
        }

        // GET: Acls/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("Aces").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Acl());
            }
        }

        // POST: Acls/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Acl acl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(acl);
                }
                else
                {
                    var apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("Aces").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = acl.Name;
                    apiItem.Description = acl.Description;
                    apiItem.EntityKindId = acl.EntityKindId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));

                    apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("Aces").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                    return View(AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(acl);
            }
        }

        // GET: Acls/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Acl apiItem = null;
            try
            {
                apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("Aces").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem));
            }
        }


        #endregion

    }
}
