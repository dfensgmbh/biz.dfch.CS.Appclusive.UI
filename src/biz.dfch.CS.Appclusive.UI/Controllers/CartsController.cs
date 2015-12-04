using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CartsController : Controller
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

        // GET: Carts
        public ActionResult Index()
        {
            var items = CoreRepository.Carts.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.Cart>>(items));
        }

        #region Cart 

        // GET: Carts/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Carts.Expand("CartItems").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Cart>(item));
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        
        public ActionResult CheckoutCart()
        {
            try
            {
                Models.Core.Order order = new Models.Core.Order();
                order.Name = "DefaultOrder";
                order.Parameters = "{}";

                CoreRepository.AddToOrders(AutoMapper.Mapper.Map<Api.Core.Order>(order));
                coreRepository.SaveChanges();

    //msls.showMessageBox(
    //    message,
    //    {
    //        title: title,
    //        buttons: msls.MessageBoxButtons.yesNo
    //    }
    //).then(function (result) {
    //    if (result == msls.MessageBoxResult.yes) {
    //        myapp.activeDataWorkspace.CoreData.saveChanges().then(function() {
    //                msls.showMessageBox(
    //                    "Order has been placed",
    //                    {
    //                        title: "Order",
    //                        buttons: msls.MessageBoxButtons.ok
    //                    }
    //                );
    //            },
    //            function fail(e) {
    //                msls.showMessageBox(e.message,
    //                {
    //                    title: "Order has been placed",
    //                    buttons: msls.MessageBoxButtons.ok
    //                }).then(
    //                    function(result) {
    //                        myapp.cancelChanges();
    //                        //throw e;
    //                        myapp.showHome(msls.BoundaryOption, null);
    //                    }
    //                );
    //            });
    //    }
    //});
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region edit CartItems

        public ActionResult ItemDetails(int id)
        {
            var item = CoreRepository.CartItems.Expand("Cart").Expand("CatalogueItem").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.CartItem>(item));
        }

        #endregion
    }
}
