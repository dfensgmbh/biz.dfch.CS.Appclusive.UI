using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ManagementCredentialsController : CoreControllerBase
    {

        // GET: ManagementCredentials
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.ManagementCredentials.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.ManagementCredential>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.ManagementCredential>());
            }
        }

        #region ManagementCredential

        // GET: ManagementCredentials/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.ManagementCredentials.Expand("ManagementUris").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ManagementCredential());
            }
        }

        // GET: ManagementCredentials/Create
        public ActionResult Create()
        {
            return View(new Models.Core.ManagementCredential());
        }

        // POST: ManagementCredentials/Create
        [HttpPost]
        public ActionResult Create(Models.Core.ManagementCredential managementCredential)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.ManagementCredential>(managementCredential);

                CoreRepository.AddToManagementCredentials(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementCredential);
            }
        }

        // GET: ManagementCredentials/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.ManagementCredentials.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.ManagementCredential());
            }
        }

        // POST: ManagementCredentials/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.ManagementCredential managementCredential)
        {
            try
            {
                var apiItem = CoreRepository.ManagementCredentials.Expand("ManagementUris").Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = managementCredential.Name;
                apiItem.Description = managementCredential.Description;
                apiItem.Password = managementCredential.Password;
                apiItem.Username = managementCredential.Username;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                apiItem = CoreRepository.ManagementCredentials.Expand("ManagementUris").Where(c => c.Id == id).FirstOrDefault(); // because of data encryption
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(managementCredential);
            }
        }

        // GET: ManagementCredentials/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.ManagementCredential apiItem = null;
            try
            {
                apiItem = CoreRepository.ManagementCredentials.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.ManagementCredential>(apiItem)));
            }
        }


        #endregion

    }
}
