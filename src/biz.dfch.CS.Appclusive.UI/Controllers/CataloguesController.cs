using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CataloguesController : Controller
    {
        private biz.dfch.CS.Appclusive.Api.Core.Core CoreRepository
        {
            get
            {
                if (coreRepository == null)
                {
                    coreRepository = new biz.dfch.CS.Appclusive.Api.Core.Core(new Uri(Properties.Settings.Default.AppculsiveApiCoreUrl));
                    coreRepository.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                }
                return coreRepository;
            }
        }
        private biz.dfch.CS.Appclusive.Api.Core.Core coreRepository;

        // GET: Catalogues
        public ActionResult Index()
        {
            var items = CoreRepository.Catalogues.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.Catalogue>>(items));
        }

        // GET: Catalogues/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Catalogues.Expand("CatalogueItems").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(item));
        }

        // GET: Catalogues/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Catalogue());
        }

        // POST: Catalogues/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Catalogue catalogue)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Catalogue>(catalogue);

                //#region add default

                //apiItem.Name = catalogue.Name;
                //apiItem.Description = catalogue.Description;
                //apiItem.Status = catalogue.Status;
                //apiItem.Version = catalogue.Version;

                //#endregion

                CoreRepository.AddToCatalogues(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Index", new { id = apiItem.Id });
            }
            catch(Exception ex)
            {
                catalogue.ErrorText = ex.Message;
                return View(catalogue);
            }
        }

        // GET: Catalogues/Edit/5
        public ActionResult Edit(int id)
        {
            var apiItem = CoreRepository.Catalogues.Expand("CatalogueItems").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem));
        }

        // POST: Catalogues/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.Catalogue catalogue)
        {
            try
            {
                var apiItem = CoreRepository.Catalogues.Expand("CatalogueItems").Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = catalogue.Name;
                apiItem.Description = catalogue.Description;
                apiItem.Status = catalogue.Status;
                apiItem.Version = catalogue.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                catalogue.ErrorText = ex.Message;
                return View(catalogue);
            }
        }

        // GET: Catalogues/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Catalogues/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
