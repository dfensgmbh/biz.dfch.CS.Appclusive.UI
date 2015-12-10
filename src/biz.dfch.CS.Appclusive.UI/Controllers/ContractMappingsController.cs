using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ContractMappingsController : CoreControllerBase
    {

        // GET: ContractMappings
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.ContractMapping> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.ContractMappings.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.ContractMappings.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.ContractMapping>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.ContractMapping>());
            }
        }

        #region ContractMapping

        // GET: ContractMappings/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.ContractMappings.Where(c => c.Id == id).FirstOrDefault();
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
            return View(new Models.Core.ContractMapping());
        }

        // POST: ContractMappings/Create
        [HttpPost]
        public ActionResult Create(Models.Core.ContractMapping contractMapping)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.ContractMapping>(contractMapping);

                CoreRepository.AddToContractMappings(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(contractMapping);
            }
        }

        // GET: ContractMappings/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.ContractMappings.Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult Edit(int id, Models.Core.ContractMapping contractMapping)
        {
            try
            {
                var apiItem = CoreRepository.ContractMappings.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = contractMapping.Name;
                apiItem.Description = contractMapping.Description;
                apiItem.Parameters = contractMapping.Parameters;
                apiItem.Version = contractMapping.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.ContractMapping>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(contractMapping);
            }
        }

        // GET: ContractMappings/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.ContractMapping apiItem = null;
            try
            {
                apiItem = CoreRepository.ContractMappings.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.ContractMapping>(apiItem)));
            }
        }


        #endregion

    }
}
