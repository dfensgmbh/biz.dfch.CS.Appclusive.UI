using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ContractMappingsController : CoreControllerBase<Api.Core.ContractMapping, Models.Core.ContractMapping>
    {
        protected override DataServiceQuery<Api.Core.ContractMapping> BaseQuery { get { return CoreRepository.ContractMappings; } }
        
        #region ContractMapping

        // GET: ContractMappings/Details/5
        public ActionResult Details(long id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.ContractMappings.Expand("Customer").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ContractMapping>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ContractMapping());
            }
        }

        // GET: ContractMappings/Create
        public ActionResult Create()
        {
            this.AddCustomerSeletionToViewBag();
            return View(new Models.Core.ContractMapping());
        }

        // POST: ContractMappings/Create
        [HttpPost]
        public ActionResult Create(Models.Core.ContractMapping contractMapping)
        {
            try
            {
                this.AddCustomerSeletionToViewBag(); 
                if (!ModelState.IsValid)
                {
                    return View(contractMapping);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.ContractMapping>(contractMapping);

                    CoreRepository.AddToContractMappings(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(contractMapping);
            }
        }

        // GET: ContractMappings/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                this.AddCustomerSeletionToViewBag();
                var apiItem = CoreRepository.ContractMappings.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ContractMapping>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ContractMapping());
            }
        }

        // POST: ContractMappings/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.ContractMapping contractMapping)
        {
            try
            {
                this.AddCustomerSeletionToViewBag();
                if (!ModelState.IsValid)
                {
                    return View(contractMapping);
                }
                else
                {
                    var apiItem = CoreRepository.ContractMappings.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = contractMapping.Name;
                    apiItem.Description = contractMapping.Description;
                    apiItem.ExternalType = contractMapping.ExternalType;
                    apiItem.ExternalId = contractMapping.ExternalId;
                    apiItem.ValidFrom = contractMapping.ValidFrom;
                    apiItem.ValidUntil = contractMapping.ValidUntil;
                    apiItem.CustomerId = contractMapping.CustomerId;
                    apiItem.Parameters = contractMapping.Parameters;
                    apiItem.IsPrimary = contractMapping.IsPrimary;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(AutoMapper.Mapper.Map<Models.Core.ContractMapping>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(contractMapping);
            }
        }

        // GET: ContractMappings/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.ContractMapping apiItem = null;
            try
            {
                apiItem = CoreRepository.ContractMappings.Expand("Customer").Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.ContractMapping>(apiItem));
            }
        }


        #endregion

    }
}
