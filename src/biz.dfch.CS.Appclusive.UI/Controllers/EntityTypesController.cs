using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class EntityTypesController : CoreControllerBase
    {

        // GET: EntityTypes
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.EntityType> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.EntityTypes.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.EntityTypes.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.EntityType>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.EntityType>());
            }
        }

        #region EntityType

        // GET: EntityTypes/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.EntityTypes.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.EntityType>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.EntityType());
            }
        }

        // GET: EntityTypes/Create
        public ActionResult Create()
        {
            return View(new Models.Core.EntityType());
        }

        // POST: EntityTypes/Create
        [HttpPost]
        public ActionResult Create(Models.Core.EntityType entityType)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.EntityType>(entityType);

                CoreRepository.AddToEntityTypes(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(entityType);
            }
        }

        // GET: EntityTypes/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.EntityTypes.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.EntityType>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.EntityType());
            }
        }

        // POST: EntityTypes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.EntityType entityType)
        {
            try
            {
                var apiItem = CoreRepository.EntityTypes.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = entityType.Name;
                apiItem.Description = entityType.Description;
                apiItem.Parameters = entityType.Parameters;
                apiItem.Version = entityType.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.EntityType>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(entityType);
            }
        }

        // GET: EntityTypes/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.EntityType apiItem = null;
            try
            {
                apiItem = CoreRepository.EntityTypes.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.EntityType>(apiItem)));
            }
        }


        #endregion

    }
}
