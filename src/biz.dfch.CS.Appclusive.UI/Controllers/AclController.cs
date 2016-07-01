using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using biz.dfch.CS.Appclusive.Public;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AclsController : CoreControllerBase<Api.Core.Acl, Models.Core.Acl, object>
    {
        protected override DataServiceQuery<Api.Core.Acl> BaseQuery { get { return CoreRepository.Acls; } }

        #region Acl

        // GET: Acls/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null, int d = 0)
        {
            #region delete message
            if (d > 0)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, string.Format(GeneralResources.ConfirmDeleted, d)));
            }
            #endregion
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Acl model = AutoMapper.Mapper.Map<Models.Core.Acl>(item);
                if (model != null)
                {
                    model.Aces = LoadAces(id, 1);
                    model.ResolveReferencedEntityName(this.CoreRepository);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Acl());
            }
        }

        // GET: Acls/Create
        public ActionResult Create(long? nodeId = null)
        {
            Models.Core.Acl acl = new Models.Core.Acl()
            {
                EntityKindId = Public.Constants.EntityKindId.Node.GetHashCode(),
                EntityId = nodeId.HasValue ? nodeId.Value : 0
            };
            acl.ResolveNavigationProperties(CoreRepository);
            return View(acl);
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
                acl.ResolveNavigationProperties(CoreRepository);
                return View(acl);
            }
        }

        // GET: Acls/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Acl model = AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem);
                if (model != null)
                {
                    model.Aces = LoadAces(id, 1);
                    model.ResolveReferencedEntityName(this.CoreRepository);
                    foreach (var ace in model.Aces)
                    {
                        ace.ResolveNavigationProperties(this.CoreRepository, model);
                    }
                }
                return View(model);
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
                    var apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = acl.Name;
                    apiItem.Description = acl.Description;
                    apiItem.EntityId = acl.EntityId.Value;
                    apiItem.EntityKindId = acl.EntityKindId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));

                    apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                    Models.Core.Acl model = AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem);
                    if (model != null)
                    {
                        model.Aces = LoadAces(id, 1);
                        model.ResolveReferencedEntityName(this.CoreRepository);
                        foreach (var ace in model.Aces)
                        {
                            ace.ResolveNavigationProperties(this.CoreRepository, model);
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                acl.ResolveNavigationProperties(CoreRepository);
                acl.Aces = LoadAces(id, 1);
                acl.ResolveReferencedEntityName(this.CoreRepository);
                foreach (var ace in acl.Aces)
                {
                    ace.ResolveNavigationProperties(this.CoreRepository, acl);
                }
                return View(acl);
            }
        }

        // GET: Acls/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Acl apiItem = null;
            try
            {
                apiItem = CoreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                Models.Core.Acl model = AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem);
                if (model != null)
                {
                    model.Aces = LoadAces(id, 1);
                    model.ResolveReferencedEntityName(this.CoreRepository);
                    foreach (var ace in model.Aces)
                    {
                        ace.ResolveNavigationProperties(this.CoreRepository, model);
                    }
                }
                return View(model);
            }
        }

        public ActionResult Inheritance(long id, bool enable)
        {
            Api.Core.Acl apiItem = null;
            try
            {
                if (enable)
                {
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.warn, "TODO: " + GeneralResources.InheritanceEnabled));
                }
                else
                {
                    CoreRepository.InvokeEntityActionWithVoidResult("Acls", id, "BreakInheritance", new
                    {
                        CopyAces = true
                    });
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.InheritanceDisabled));
                }

                // load detail
                var item = CoreRepository.Acls.Expand("EntityKind").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Acl model = AutoMapper.Mapper.Map<Models.Core.Acl>(item);
                if (model != null)
                {
                    model.Aces = LoadAces(id, 1);
                    model.ResolveReferencedEntityName(this.CoreRepository);
                    foreach (var ace in model.Aces)
                    {
                        ace.ResolveNavigationProperties(this.CoreRepository, model);
                    }
                }
                return View("Details", model);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                Models.Core.Acl model = AutoMapper.Mapper.Map<Models.Core.Acl>(apiItem);
                if (model != null)
                {
                    model.Aces = LoadAces(id, 1);
                    model.ResolveReferencedEntityName(this.CoreRepository);
                    foreach (var ace in model.Aces)
                    {
                        ace.ResolveNavigationProperties(this.CoreRepository, model);
                    }
                }
                return View("Details", model);
            }
        }

        #endregion

        #region Ace  list and search

        public PartialViewResult ItemIndex(long aclId, int pageNr = 1, string itemSearchTerm = null, string orderBy = null, bool Readonly = false, string ajaxPagingTargetId = null)
        {
            ViewBag.Readonly = Readonly;
            if (!string.IsNullOrEmpty(ajaxPagingTargetId))
            {
                ViewBag.AjaxPagingTargetId = ajaxPagingTargetId;
            }
            return PartialView("AceList", LoadAces(aclId, pageNr, false, itemSearchTerm, orderBy));
        }

        public ActionResult ItemSearch(long aclId, string term)
        {
            var aces = LoadAces(aclId, 1, true, term);
            List<AjaxOption> options = new List<AjaxOption>();
            foreach (var item in aces)
            {
                options.Add(new AjaxOption(item.Permission.Name, item.Permission.Name));
            }
            return this.Json(options, JsonRequestBehavior.AllowGet);
        }

        private List<Models.Core.Ace> LoadAces(long aclId, int pageNr, bool distinct = false, string itemSearchTerm = null, string orderBy = null)
        {
            PagingInfo pagingInfo;
            var aces = Models.Core.Acl.LoadAces(aclId, pageNr, out pagingInfo, distinct, itemSearchTerm, orderBy);
            ViewBag.ParentId = aclId;
            ViewBag.ItemSearchTerm = itemSearchTerm;
            ViewBag.AjaxPaging = pagingInfo;
            return aces;
        }


        #endregion
    }
}
