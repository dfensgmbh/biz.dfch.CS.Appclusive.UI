using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Diagnostics.Contracts;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CataloguesController : CoreControllerBase
    {
        // GET: Catalogues
        public ActionResult Index()
        {
            try
            {
                var items = CoreRepository.Catalogues.Take(PortalConfig.Pagesize).ToList();
                return View(AutoMapper.Mapper.Map<List<Models.Core.Catalogue>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Catalogue>());
            }
        }

        #region Catalogue 

        // GET: Catalogues/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var item = CoreRepository.Catalogues.Expand("CatalogueItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Catalogue());
            }
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
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(catalogue);
            }
        }

        // GET: Catalogues/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.Catalogues.Expand("CatalogueItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Catalogue());
            }
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
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem));
            }
            catch(Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
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
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiItem)));
            }
        }

        #endregion

        #region CatalogItems

        public ActionResult ItemDetails(int id)
        {
            try
            {
                var item = CoreRepository.CatalogueItems.Expand("Catalogue").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CatalogueItem());
            }
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


        private void AddProductSeletionToViewBag()
        {
            try
            {
                var products = CoreRepository.Products.ToList();
                ViewBag.ProductSelection = new SelectList(products, "Id", "Name");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
            }
        }

        // GET: Catalogues/Create
        public ActionResult ItemCreate(int catalogId)
        {
            try
            {
                Contract.Requires(catalogId > 0);
                this.AddProductSeletionToViewBag();
                var apiCatalog = CoreRepository.Catalogues.Where(c => c.Id == catalogId).FirstOrDefault();
                var catalog = AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiCatalog);
                Contract.Assert(null != catalog, "No catalog for item loaded");

                var modelItem = new Models.Core.CatalogueItem();
                modelItem.Catalogue = catalog;
                modelItem.CatalogueId = catalogId;
                
                return View(modelItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CatalogueItem());
            }
        }

        // POST: Catalogues/Create
        [HttpPost]
        public ActionResult ItemCreate(Models.Core.CatalogueItem catalogueItem)
        {
            try
            {
                Contract.Requires(null != catalogueItem);
                var apiItem = AutoMapper.Mapper.Map<Api.Core.CatalogueItem>(catalogueItem);

                CoreRepository.AddToCatalogueItems(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("ItemDetails", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                this.AddProductSeletionToViewBag();
                var apiCatalog = CoreRepository.Catalogues.Where(c => c.Id == catalogueItem.CatalogueId).FirstOrDefault();
                catalogueItem.Catalogue = AutoMapper.Mapper.Map<Models.Core.Catalogue>(apiCatalog);
                return View(catalogueItem);
            }
        }

        // GET: Catalogues/Edit/5
        public ActionResult ItemEdit(int id)
        {
            try
            {
                Contract.Requires(id > 0);
                this.AddProductSeletionToViewBag();
                var apiItem = CoreRepository.CatalogueItems.Expand("Catalogue").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.CatalogueItem());
            }
        }

        // POST: Catalogues/ItemEdit/5
        [HttpPost]
        public ActionResult ItemEdit(int id, Models.Core.CatalogueItem catalogueItem)
        {
            try
            {
                Contract.Requires(id > 0);
                Contract.Requires(null != catalogueItem);
                this.AddProductSeletionToViewBag();
                var apiItem = CoreRepository.CatalogueItems.Expand("Catalogue").Where(c => c.Id == id).FirstOrDefault();
                Contract.Assert(null != apiItem);

                #region copy all edited properties

                apiItem.Name = catalogueItem.Name;
                apiItem.ValidFrom = catalogueItem.ValidFrom;
                apiItem.ValidUntil = catalogueItem.ValidUntil;
                apiItem.EndOfSale = catalogueItem.EndOfSale;
                apiItem.EndOfLife = catalogueItem.EndOfLife;
                apiItem.Description = catalogueItem.Description;
                apiItem.Parameters = catalogueItem.Parameters;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(catalogueItem);
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(catalogueItem);
            }
        }


        // GET: EntityTypes/ItemDelete/5
        public ActionResult ItemDelete(int id)
        {
            Api.Core.CatalogueItem apiItem = null;
            try
            {
                Contract.Requires(id > 0);
                apiItem = CoreRepository.CatalogueItems.Where(c => c.Id == id).FirstOrDefault();
                Contract.Assert(null != apiItem);
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Details", new { Id = apiItem.CatalogueId });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("ItemDetails", View(AutoMapper.Mapper.Map<Models.Core.CatalogueItem>(apiItem)));
            }
        }

        #endregion

    }
}
