using System;
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
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.Tenants.Where(c => c.Id == id).FirstOrDefault();
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
            return View(new Models.Core.Tenant());
        }

        // POST: Tenants/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Tenant tenant)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Tenant>(tenant);

                CoreRepository.AddToTenants(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(tenant);
            }
        }

        // GET: Tenants/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.Tenants.Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult Edit(int id, Models.Core.Tenant tenant)
        {
            try
            {
                var apiItem = CoreRepository.Tenants.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = tenant.Name;
                apiItem.Description = tenant.Description;
                apiItem.Parameters = tenant.Parameters;
                apiItem.Version = tenant.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Tenant>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(tenant);
            }
        }
        
        #endregion

    }
}
