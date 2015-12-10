using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Diagnostics.Contracts;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class ProductsController : CoreControllerBase
    {
        public ProductsController()
            : base()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }
        // GET: Products
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                List<Api.Core.Product> items;
                if (pageNr > 1)
                {
                    items = CoreRepository.Products.Skip((pageNr - 1) * PortalConfig.Pagesize).Take(PortalConfig.Pagesize + 1).ToList();
                }
                else
                {
                    items = CoreRepository.Products.Take(PortalConfig.Pagesize + 1).ToList();
                }
                ViewBag.Paging = new PagingInfo(pageNr, items.Count > PortalConfig.Pagesize);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Product>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Product>());
            }
        }

        #region Product

        // GET: Products/Details/5
        public ActionResult Details(int id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Products.Expand("CatalogueItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Product>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Product());
            }
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View(new Models.Core.Product());
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(Models.Core.Product product)
        {
            try
            {
                var apiItem = AutoMapper.Mapper.Map<Api.Core.Product>(product);

                CoreRepository.AddToProducts(apiItem);
                CoreRepository.SaveChanges();

                return RedirectToAction("Details", new { id = apiItem.Id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var apiItem = CoreRepository.Products.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Product>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Product());
            }
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Models.Core.Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                else
                {
                    var apiItem = CoreRepository.Products.Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.Name = product.Name;
                    apiItem.Description = product.Description;
                    apiItem.Parameters = product.Parameters;
                    apiItem.Version = product.Version;
                    apiItem.EndOfLife = product.EndOfLife;
                    apiItem.EndOfSale = product.EndOfSale;
                    apiItem.ValidFrom = product.ValidFrom;
                    apiItem.ValidUntil = product.ValidUntil;
                    apiItem.Type = product.Type;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(AutoMapper.Mapper.Map<Models.Core.Product>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(product);
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.Product apiItem = null;
            try
            {
                apiItem = CoreRepository.Products.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", View(AutoMapper.Mapper.Map<Models.Core.Product>(apiItem)));
            }
        }
        #endregion
    }
}
