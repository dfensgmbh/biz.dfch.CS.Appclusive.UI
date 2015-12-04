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

        #region Catalogue 

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
                
                CoreRepository.AddToCatalogues(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch(Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
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
                ViewBag.InfoText = "Successfully saved";
                return View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem));
            }
            catch(Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View(catalogue);
            }
        }


        // GET: EntityTypes/Delete/5
        public ActionResult Delete(int id)
        { 
            Api.Core.Catalogue apiItem =null;
            try
            {
                apiItem = CoreRepository.Catalogues.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem)));
            }
        }

        #endregion

        #region CatalogItems

        public ActionResult ItemDetails(int id)
        {
            var item = CoreRepository.CatalogueItems.Expand("Catalogue").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(item));
        }

        public PartialViewResult AddToCart(int id)
        {
            AjaxNotificationViewModel vm = new AjaxNotificationViewModel();
            try
            {
                var catalogueItem = CoreRepository.CatalogueItems.Expand("Catalogue").Where(c => c.Id == id).FirstOrDefault();

                Models.Core.CartItem cartItem = new Models.Core.CartItem();
                cartItem.Name = catalogueItem.Name;
                cartItem.Description = catalogueItem.Description;
                cartItem.Quantity = 1;
                cartItem.CatalogueItemId = catalogueItem.Id;

                CoreRepository.AddToCartItems(AutoMapper.Mapper.Map<Api.Core.CartItem>(cartItem));
                CoreRepository.SaveChanges();
                vm.Level = ENotifyStyle.success;
                vm.Message = string.Format("Item {0} added to cart", catalogueItem.Name);

                return PartialView("AjaxNotification", new AjaxNotificationViewModel[] { vm });
            }
            catch (Exception ex)
            {
                return PartialView("AjaxNotification", ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

        /// <summary>
        /// Notification test
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public PartialViewResult Notify(string level)
        {
            AjaxNotificationViewModel vm = new AjaxNotificationViewModel();
            vm.Level = (ENotifyStyle)Enum.Parse(typeof(ENotifyStyle), level);
                vm.Message = string.Format("{0} called", level);
                return PartialView("AjaxNotification", new AjaxNotificationViewModel[] { vm });
        }

        #endregion
    }
}
