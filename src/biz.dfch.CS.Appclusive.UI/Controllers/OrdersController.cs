using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class OrdersController : Controller
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

        // GET: Orders
        public ActionResult Index()
        {
            var items = CoreRepository.Orders.Take(PortalConfig.Pagesize).ToList();
            return View(AutoMapper.Mapper.Map<List<Models.Core.Order>>(items));
        }

        #region Order 

        // GET: Orders/Details/5
        public ActionResult Details(int id)
        {
            var item = CoreRepository.Orders.Expand("OrderItems").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.Order>(item));
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int id)
        {
            Api.Core.Order apiItem = null;
            try
            {
                apiItem = CoreRepository.Orders.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Order>(apiItem));
            }
        }

        #endregion

        #region edit OrderItems

        public ActionResult ItemDetails(int id)
        {
            var item = CoreRepository.OrderItems.Expand("Order").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.OrderItem>(item));
        }

        // GET: Orders/Delete/5
        public ActionResult ItemDelete(int id)
        {
            Api.Core.OrderItem apiItem = null;
            try
            {
                apiItem = CoreRepository.OrderItems.Expand("Order").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Details", new { id = apiItem.OrderId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorText = ex.Message;
                return View("ItemDetails", AutoMapper.Mapper.Map<Models.Core.OrderItem>(apiItem));
            }
        }

        #endregion
    }
}
