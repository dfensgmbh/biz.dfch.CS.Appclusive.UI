using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class TenantsController : CoreControllerBase<Api.Core.Tenant, Models.Core.Tenant, object>
    {
        protected override DataServiceQuery<Api.Core.Tenant> BaseQuery { get { return CoreRepository.Tenants.Expand("Parent"); } }

        #region Tenant

        // GET: Tenants/Details/5
        public ActionResult Details(string id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                Guid guid = Guid.Parse(id);
                var item = CoreRepository.Tenants.Expand("Customer").Expand("Parent").Where(c => c.Id == guid).FirstOrDefault();
                Models.Core.Tenant modelItem = AutoMapper.Mapper.Map<Models.Core.Tenant>(item);
                if (null != modelItem)
                {
                    modelItem.Children = LoadChildren(id, 1);
                }
                return View(modelItem);
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
                var apiItem = CoreRepository.Tenants.Expand("Customer").Expand("Parent").Where(c => c.Id == guid).FirstOrDefault();
                this.AddTenantSeletionToViewBag(apiItem);
                this.AddCustomerSeletionToViewBag();
                Models.Core.Tenant modelItem = AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem);
                if (null != modelItem)
                {
                    modelItem.Children = LoadChildren(id, 1);
                }
                return View(AddUsers(modelItem));
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
                    if (tenant.Id == Guid.Empty) tenant.IdStr = id;
                    tenant.Children = LoadChildren(id, 1);
                    return View(AddUsers(tenant));
                }
                else
                {
                    apiItem = CoreRepository.Tenants.Where(c => c.Id == guid).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Id = tenant.Id;
                    apiItem.Name = tenant.Name;
                    apiItem.Description = tenant.Description;
                    apiItem.ParentId = tenant.ParentId;
                    apiItem.ExternalType = tenant.ExternalType;
                    apiItem.ExternalId = tenant.ExternalId;
                    apiItem.CustomerId = tenant.CustomerId;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    this.AddTenantSeletionToViewBag(apiItem);
                    this.AddCustomerSeletionToViewBag();
                    Models.Core.Tenant modelItem = AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem);
                    if (null != modelItem)
                    {
                        modelItem.Children = LoadChildren(id, 1);
                    }
                    return View(AddUsers(modelItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddTenantSeletionToViewBag(apiItem);
                this.AddCustomerSeletionToViewBag();
                return View(AddUsers(tenant));
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            Api.Core.Tenant apiItem = null;
            try
            {
                Guid guid = Guid.Parse(id);
                apiItem = CoreRepository.Tenants.Expand("Customer").Expand("Parent").Where(c => c.Id == guid).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                Models.Core.Tenant modelItem = AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem);
                if (null != modelItem)
                {
                    modelItem.Children = LoadChildren(id, 1);
                }
                return View("Details", AddUsers(modelItem));
            }
        }

        #endregion

        #region tenant-children list and search

        public PartialViewResult ItemIndex(string parentId, int skip = 0, string itemSearchTerm = null, string orderBy = null)
        {
            ViewBag.ParentId = parentId;
            DataServiceQuery<Api.Core.Tenant> itemsBaseQuery = CoreRepository.Tenants;
            string itemsBaseFilter = string.Format("ParentId eq guid'{0}'", parentId);
            return base.ItemIndex<Api.Core.Tenant, Models.Core.Tenant>(itemsBaseQuery, itemsBaseFilter, skip, itemSearchTerm, orderBy);
        }

        private List<Models.Core.Tenant> LoadChildren(string parentId, int skip)
        {
            QueryOperationResponse<Api.Core.Tenant> items = CoreRepository.Tenants
                    .AddQueryOption("$filter", string.Format("ParentId eq guid'{0}'", parentId))
                    .AddQueryOption("$inlinecount", "allpages")
                    .AddQueryOption("$top", PortalConfig.Pagesize)
                    .AddQueryOption("$skip", skip)
                    .Execute() as QueryOperationResponse<Api.Core.Tenant>;

            var result = AutoMapper.Mapper.Map<List<Models.Core.Tenant>>(items);

            ViewBag.ParentId = parentId;
            ViewBag.AjaxPaging = CreatePageFilterInfo(items);

            return result;
        }

        public ActionResult ItemSearch(string parentId, string term)
        {
            DataServiceQuery<Api.Core.Tenant> itemsBaseQuery = CoreRepository.Tenants;
            string itemsBaseFilter = string.Format("ParentId eq guid'{0}'", parentId);
            return base.ItemSearch(itemsBaseQuery, itemsBaseFilter, term);
        }

        #endregion tenant-children list

        /// <summary>
        /// Adds the user objects because they are not available through .Expand("CreatedBy").Expand("ModifiedBy")
        /// </summary>
        /// <param name="apiItem"></param>
        private Models.Core.Tenant AddUsers(Models.Core.Tenant modelItem)
        {
            if (null != modelItem)
            {
                if (modelItem.CreatedById > 0)
                {
                    modelItem.CreatedBy = AutoMapper.Mapper.Map<Models.Core.User>(CoreRepository.Users.Where(c => c.Id == modelItem.CreatedById).FirstOrDefault());
                }
                if (modelItem.ModifiedById > 0)
                {
                    modelItem.ModifiedBy = AutoMapper.Mapper.Map<Models.Core.User>(CoreRepository.Users.Where(c => c.Id == modelItem.ModifiedById).FirstOrDefault());
                }
            }
            return modelItem;
        }

    }
}
