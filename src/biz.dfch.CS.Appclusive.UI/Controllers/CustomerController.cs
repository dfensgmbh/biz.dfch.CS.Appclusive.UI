using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CustomersController : CoreControllerBase
    {

        // GET: Customers
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Customer> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Customers.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Customers.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Customer>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Customer>());
            }
        }

        #region Customer

        // GET: Customers/Details/5
        public ActionResult Details(long id)
        {
            try
            {
                var item = CoreRepository.Customers.Expand("ContractMappings").Expand("Tenants").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Customer>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Customer());
            }
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Customer());
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Customer customer)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Customer>(customer);

                CoreRepository.AddToCustomers(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(customer);
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Customers.Expand("ContractMappings").Expand("Tenants").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Customer>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Customer());
            }
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Customer customer)
        {
            try
            {
                var apiItem = CoreRepository.Customers.Expand("ContractMappings").Expand("Tenants").Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = customer.Name;
                apiItem.Description = customer.Description;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Customer>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                if (null == customer.ContractMappings) customer.ContractMappings = new List<Models.Core.ContractMapping>();
                if (null == customer.Tenants) customer.Tenants = new List<Models.Core.Tenant>();
                return View(customer);
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Customer apiItem = null;
            try
            {
                apiItem = CoreRepository.Customers.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Customer>(apiItem));
            }
        }


        #endregion

    }
}
