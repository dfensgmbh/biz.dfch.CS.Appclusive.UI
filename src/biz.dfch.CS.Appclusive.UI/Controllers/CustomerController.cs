using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CustomersController : CoreControllerBase<Api.Core.Customer, Models.Core.Customer, object>
    {
        protected override DataServiceQuery<Api.Core.Customer> BaseQuery { get { return CoreRepository.Customers; } }
        
        #region Customer

        // GET: Customers/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Customers.Expand("ContractMappings").Expand("Tenants").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Customer modelItem = AutoMapper.Mapper.Map<Models.Core.Customer>(item);
                if (null != modelItem)
                {
                    modelItem.Tenants = LoadTenants(id, 1);
                }
                return View(modelItem);
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
                if (!ModelState.IsValid)
                {
                    return View(customer);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.Customer>(customer);

                    CoreRepository.AddToCustomers(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
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
                var apiItem = CoreRepository.Customers.Expand("ContractMappings").Expand("Tenants").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                Models.Core.Customer modelItem = AutoMapper.Mapper.Map<Models.Core.Customer>(apiItem);
                if (null != modelItem)
                {
                    modelItem.Tenants = LoadTenants(id, 1);
                }
                return View(modelItem);
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
                if (!ModelState.IsValid)
                {
                    customer.ContractMappings = AutoMapper.Mapper.Map<List<Models.Core.ContractMapping>>(CoreRepository.ContractMappings.Where(ci => ci.CustomerId == id).ToList());
                    return View(customer);
                }
                else
                {
                    var apiItem = CoreRepository.Customers.Expand("ContractMappings").Expand("Tenants").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = customer.Name;
                    apiItem.Description = customer.Description;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));

                    Models.Core.Customer modelItem = AutoMapper.Mapper.Map<Models.Core.Customer>(apiItem);
                    if (null != modelItem)
                    {
                        modelItem.Tenants = LoadTenants(id, 1);
                    }
                    return View(modelItem);
                }
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
                apiItem = CoreRepository.Customers.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
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


        private List<Models.Core.Tenant> LoadTenants(long customerId, int pageNr)
        {
            List<Api.Core.Tenant> items = CoreRepository.Tenants.Where(t=>t.CustomerId==customerId).ToList();
            return AutoMapper.Mapper.Map<List<Models.Core.Tenant>>(items);
        }

        #endregion

    }
}
