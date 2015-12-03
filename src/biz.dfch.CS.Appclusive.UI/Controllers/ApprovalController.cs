using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ApprovalsController : Controller
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

        // GET: Approvals
        public ActionResult Index()
        {
            var items = CoreRepository.Approvals.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.Approval>>(items));
        }

        #region Approval 

        // GET: Approvals/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Approval>(item));
        }
        
        // GET: Approvals/Edit/5
        public ActionResult Edit(int id)
        {
            var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Approval>(apiItem));
        }

        // POST: Approvals/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.Approval Approval)
        {
            try
            {
                var apiItem = CoreRepository.Approvals.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                // TODO: all edited properties
                //apiItem.Name = Approval.Name;
                //apiItem.Description = Approval.Description;
                //apiItem.Status = Approval.Status;
                //apiItem.Version = Approval.Version;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Approval.ErrorText = ex.Message;
                return View(Approval);
            }
        }

        // GET: Approvals/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Approvals/Delete/5
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
