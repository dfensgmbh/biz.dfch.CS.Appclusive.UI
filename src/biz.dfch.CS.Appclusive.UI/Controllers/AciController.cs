using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class AcisController : CoreControllerBase
    {

        // GET: Acis
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Aci> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Acis.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Acis.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Aci>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Aci>());
            }
        }

        #region Aci

        // GET: Acis/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.Acis.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Aci>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Aci());
            }
        }

        // GET: Acis/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Aci());
        }

        // POST: Acis/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Aci aci)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Aci>(aci);

                CoreRepository.AddToAcis(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(aci);
            }
        }

        // GET: Acis/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.Acis.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Aci>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Aci());
            }
        }

        // POST: Acis/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.Aci aci)
        {
            try
            {
                var apiItem = CoreRepository.Acis.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = aci.Name;
                apiItem.Description = aci.Description;
                apiItem.Parameters = aci.Parameters;
                apiItem.Version = aci.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Aci>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(aci);
            }
        }

        // GET: Acis/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.Aci apiItem = null;
            try
            {
                apiItem = CoreRepository.Acis.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.Aci>(apiItem)));
            }
        }


        #endregion

    }
}
