using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class KeyNameValuesController : Controller
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

        // GET: KeyNameValues
        public ActionResult Index()
        {
            var items = CoreRepository.KeyNameValues.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.KeyNameValue>>(items));
        }

        #region KeyNameValue 

        // GET: KeyNameValues/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(item));
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
            catch(Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(keyNameValue);
            }
        }

        // GET: KeyNameValues/Edit/5
        public ActionResult Edit(int id)
        {
            var apiItem = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(apiItem));
        }

        // POST: KeyNameValues/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.KeyNameValue keyNameValue)
        {
            try
            {
                var apiItem = CoreRepository.KeyNameValues.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                // TODO: set edited properties
                //apiItem.Name = KeyNameValue.Name;
                //apiItem.Description = KeyNameValue.Description;
                //apiItem.Status = KeyNameValue.Status;
                //apiItem.Version = KeyNameValue.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ViewBag.InfoText = "Successfully saved";
                return View(AutoMapper.Mapper.Map<Models.Core.KeyNameValue>(apiItem));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(keyNameValue);
            }
        }

        // GET: KeyNameValues/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: KeyNameValues/Delete/5
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

        #endregion

    }
}
