using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CostCentreController : CoreControllerBase
    {

        // GET: CostCentres
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.CostCentre> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.CostCentres.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.CostCentres.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.CostCentre>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.CostCentre>());
            }
        }

        #region CostCentre

        // GET: CostCentres/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.CostCentres.Where(c => c.Id == id).FirstOrDefault();
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
                var apiItem = AutoMapper.Mapper.Map<Api.Core.CostCentre>(costCentre);

                CoreRepository.AddToCostCentres(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(costCentre);
            }
        }

        // GET: CostCentres/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.CostCentres.Where(c => c.Id == id).FirstOrDefault();
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
        public ActionResult Edit(int id, Models.Core.CostCentre costCentre)
        {
            try
            {
                var apiItem = CoreRepository.CostCentres.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = costCentre.Name;
                apiItem.Description = costCentre.Description;
                apiItem.Parameters = costCentre.Parameters;
                apiItem.Version = costCentre.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.CostCentre>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(costCentre);
            }
        }

        // GET: CostCentres/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.CostCentre apiItem = null;
            try
            {
                apiItem = CoreRepository.CostCentres.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.CostCentre>(apiItem)));
            }
        }


        #endregion

    }
}
