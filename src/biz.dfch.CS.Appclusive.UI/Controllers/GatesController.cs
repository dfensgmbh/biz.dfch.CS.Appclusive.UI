using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class GatesController : Controller
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

        // GET: Gates
        public ActionResult Index()
        {
            var items = CoreRepository.Gates.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.Gate>>(items));
        }

        #region Gate 

        // GET: Gates/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Gate>(item));
        }

        // GET: Gates/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Gate());
        }

        // POST: Gates/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Gate gate)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Gate>(gate);
                
                CoreRepository.AddToGates(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch(Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(gate);
            }
        }

        // GET: Gates/Edit/5
        public ActionResult Edit(int id)
        {
            var apiItem = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
        }

        // POST: Gates/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.Gate gate)
        {
            try
            {
                var apiItem = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                // TODO: set edited properties
                //apiItem.Name = Gate.Name;
                //apiItem.Description = Gate.Description;
                //apiItem.Status = Gate.Status;
                //apiItem.Version = Gate.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ViewBag.InfoText = "Successfully saved";
                return View(AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(gate);
            }
        }

        // GET: Gates/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var apiItem = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();
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
