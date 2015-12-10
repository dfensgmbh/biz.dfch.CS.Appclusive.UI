using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class KeyNameValuesController : CoreControllerBase
    {
        // GET: KeyNameValues
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.KeyNameValue> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.KeyNameValues.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.KeyNameValues.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.KeyNameValue>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.KeyNameValue>());
            }
        }

        #region KeyNameValue

        // GET: KeyNameValues/Details/5
        public ActionResult Details(long id)
        {
            try
            {
                var item = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.KeyNameValue());
            }
        }

        // GET: KeyNameValues/Create
        public ActionResult Create()
        {
            return View(new Models.Core.KeyNameValue());
        }

        // POST: KeyNameValues/Create
        [HttpPost]
        public ActionResult Create(Models.Core.KeyNameValue keyNameValue)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.KeyNameValue>(keyNameValue);

                CoreRepository.AddToKeyNameValues(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(keyNameValue);
            }
        }

        // GET: KeyNameValues/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.KeyNameValue());
            }
        }

        // POST: KeyNameValues/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.KeyNameValue keyNameValue)
        {
            try
            {
                var apiItem = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Key = keyNameValue.Key;
                apiItem.Name = keyNameValue.Name;
                apiItem.Description = keyNameValue.Description;
                apiItem.Value = keyNameValue.Value;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(keyNameValue);
            }
        }

        // GET: KeyNameValues/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.KeyNameValue apiItem = null;
            try
            {
                apiItem = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(apiItem));
            }
        }

        #endregion

    }
}
