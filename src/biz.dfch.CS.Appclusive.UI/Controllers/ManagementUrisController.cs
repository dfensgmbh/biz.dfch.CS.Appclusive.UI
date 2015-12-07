using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ManagementUrisController : CoreControllerBase
    {

        // GET: ManagementUris
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.ManagementUris.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.ManagementUri>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.ManagementUri>());
            }
        }

        #region ManagementUri

        // GET: ManagementUris/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.ManagementUris.Expand("ManagementCredential").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(item));
        }

        // GET: ManagementUris/Create
        public ActionResult Create()
        {
            return View(new Models.Core.ManagementUri());
        }

        // POST: ManagementUris/Create
        [HttpPost]
        public ActionResult Create(Models.Core.ManagementUri managementUri)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.ManagementUri>(managementUri);

                CoreRepository.AddToManagementUris(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Edit/5
        public ActionResult Edit(int id)
        {
            var apiItem = CoreRepository.ManagementUris.Expand("ManagementCredential").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(apiItem));
        }

        // POST: ManagementUris/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.ManagementUri managementUri)
        {
            try
            {
                var apiItem = CoreRepository.ManagementUris.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                // TODO: set edited properties
                //apiItem.Name = ManagementUri.Name;
                //apiItem.Description = ManagementUri.Description;
                //apiItem.Status = ManagementUri.Status;
                //apiItem.Version = ManagementUri.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ViewBag.InfoText = "Successfully saved";
                return View(AutoMapper.Mapper.Map<Models.Core.ManagementUri>(apiItem));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(managementUri);
            }
        }

        // GET: ManagementUris/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var apiItem = CoreRepository.ManagementUris.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                return Index();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View();
            }
        }


        #endregion

    }
}
