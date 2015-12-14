using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class OrdersController : CoreControllerBase
    {
        // GET: Orders
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                QueryOperationResponse<Api.Core.Order> items = CoreRepository.Orders
                        .AddQueryOption("$inlinecount", "allpages")
                        .AddQueryOption("$top", PortalConfig.Pagesize)
                        .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                        .Execute() as QueryOperationResponse<Api.Core.Order>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Order>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Order>());
            }
        }

        #region Order

        // GET: Orders/Details/5
        public ActionResult Details(long id, int rId = 0, string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Orders.Expand("OrderItems").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Order>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Order());
            }
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long id)
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

        public ActionResult ItemDetails(long id)
        {
            var item = CoreRepository.OrderItems.Expand("Order").Where(c => c.Id == id).FirstOrDefault();
            return View(AutoMapper.Mapper.Map<Models.Core.OrderItem>(item));
        }

        // GET: Orders/Delete/5
        public ActionResult ItemDelete(long id)
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
