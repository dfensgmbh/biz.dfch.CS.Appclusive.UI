using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class TenantsController : CoreControllerBase<Api.Core.Tenant, Models.Core.Tenant>
    {
        protected override DataServiceQuery<Api.Core.Tenant> BaseQuery { get { return CoreRepository.Tenants.Expand("Parent"); } }

        #region Tenant

        // GET: Tenants/Details/5
        public ActionResult Details(string id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                Guid guid = Guid.Parse(id);
                // no navigation properties yet on Api .Expand("CreatedBy").Expand("ModifiedBy")
                var item = CoreRepository.Tenants.Expand("Customer").Expand("Parent").Expand("Children").Where(c => c.Id == guid).FirstOrDefault();
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
            this.AddCustomerSeletionToViewBag();
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
                if (!ModelState.IsValid)
                {
                    this.AddTenantSeletionToViewBag(apiItem);
                    this.AddCustomerSeletionToViewBag();
                    return View(tenant);
                }
                else
                {
                    CoreRepository.AddToTenants(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddTenantSeletionToViewBag(apiItem);
                this.AddCustomerSeletionToViewBag();
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
                this.AddCustomerSeletionToViewBag();
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
                if (!ModelState.IsValid)
                {
                    this.AddTenantSeletionToViewBag(AutoMapper.Mapper.Map<Api.Core.Tenant>(tenant));
                    this.AddCustomerSeletionToViewBag(); 
                    return View(tenant);
                }
                else
                {
                    apiItem = CoreRepository.Tenants.Expand("Children").Where(c => c.Id == guid).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Id = tenant.Id; 
                    apiItem.Name = tenant.Name;
                    apiItem.Description = tenant.Description;
                    apiItem.ParentId = tenant.ParentId;
                    apiItem.ExternalType = tenant.ExternalType;
                    apiItem.ExternalId = tenant.ExternalId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    this.AddTenantSeletionToViewBag(apiItem);
                    this.AddCustomerSeletionToViewBag();
                    return View(AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddTenantSeletionToViewBag(apiItem); 
                this.AddCustomerSeletionToViewBag();
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
                apiItem = CoreRepository.Tenants.Expand("Customer").Expand("Parent").Expand("Children").Where(c => c.Id == guid).FirstOrDefault();
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



    }
}
