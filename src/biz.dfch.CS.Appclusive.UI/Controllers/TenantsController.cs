﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class TenantsController : CoreControllerBase
    {

        // GET: Tenants
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Tenant> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Tenants.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Tenants.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                ViewBag.ShowEditLinks = true;
                return View(AutoMapper.Mapper.Map<List<Models.Core.Tenant>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Tenant>());
            }
        }

        #region Tenant

        // GET: Tenants/Details/5
        public ActionResult Details(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);
                var item = CoreRepository.Tenants.Expand("Children").Where(c => c.Id == guid).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Tenant>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Tenant());
            }
        }

        // GET: Tenants/Create
        public ActionResult Create()
        {
            Models.Core.Tenant tenant = new Models.Core.Tenant();
            tenant.Id = Guid.NewGuid();
            this.AddTenantSeletionToViewBag(null);
            return View(tenant);
        }

        // POST: Tenants/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Tenant tenant)
        {
            Api.Core.Tenant apiItem = null;
            try
            {
                apiItem = AutoMapper.Mapper.Map<Api.Core.Tenant>(tenant);
                CoreRepository.AddToTenants(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddTenantSeletionToViewBag(apiItem);
                return View(tenant);
            }
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);
                var apiItem = CoreRepository.Tenants.Expand("Children").Where(c => c.Id == guid).FirstOrDefault();
                this.AddTenantSeletionToViewBag(apiItem);
                return View(AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Tenant());
            }
        }

        // POST: Tenants/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, Models.Core.Tenant tenant)
        {
            Api.Core.Tenant apiItem = null;
            try
            {
                Guid guid = Guid.Parse(id);
                apiItem = CoreRepository.Tenants.Expand("Children").Where(c => c.Id == guid).FirstOrDefault();

                #region copy all edited properties

                apiItem.ExternalId = tenant.ExternalId;
                apiItem.ExternalType = tenant.ExternalType;
                apiItem.ParentId = tenant.ParentId;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddTenantSeletionToViewBag(apiItem);
                return View(tenant);
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            Api.Core.Tenant apiItem = null;
            try
            {
                Guid guid = Guid.Parse(id);
                apiItem = CoreRepository.Tenants.Expand("Children").Where(c => c.Id == guid).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem));
            }
        }

        #endregion


        private void AddTenantSeletionToViewBag(Api.Core.Tenant currentTenant)
        {
            try
            {
                List<Api.Core.Tenant> tenants = new List<Api.Core.Tenant>();
                tenants.Add(new Api.Core.Tenant());
                tenants.AddRange(CoreRepository.Tenants.ToList().Where(t => currentTenant==null || t.Id != currentTenant.Id));

                ViewBag.TenantSelection = new SelectList(tenants, "Id", "DisplayName");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

    }
}
