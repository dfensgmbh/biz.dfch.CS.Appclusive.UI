using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;
using biz.dfch.CS.Appclusive.UI.App_LocalResources;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CostCentresController : CoreControllerBase<Api.Core.CostCentre, Models.Core.CostCentre, object>
    {
        protected override DataServiceQuery<Api.Core.CostCentre> BaseQuery { get { return CoreRepository.CostCentres; } }
        
        #region CostCentre

        // GET: CostCentres/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.CostCentres.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CostCentre>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CostCentre());
            }
        }

        // GET: CostCentres/Create
        public ActionResult Create()
        {
            return View(new Models.Core.CostCentre());
        }

        // POST: CostCentres/Create
        [HttpPost]
        public ActionResult Create(Models.Core.CostCentre costCentre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(costCentre);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.CostCentre>(costCentre);

                    CoreRepository.AddToCostCentres(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(costCentre);
            }
        }

        // GET: CostCentres/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.CostCentres.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CostCentre>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CostCentre());
            }
        }

        // POST: CostCentres/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.CostCentre costCentre)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(costCentre);
                }
                else
                {
                    var apiItem = CoreRepository.CostCentres.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = costCentre.Name;
                    apiItem.Description = costCentre.Description;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, GeneralResources.SuccessfullySaved));
                    return View(AutoMapper.Mapper.Map<Models.Core.CostCentre>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(costCentre);
            }
        }

        // GET: CostCentres/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.CostCentre apiItem = null;
            try
            {
                apiItem = CoreRepository.CostCentres.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.CostCentre>(apiItem));
            }
        }


        #endregion

    }
}
