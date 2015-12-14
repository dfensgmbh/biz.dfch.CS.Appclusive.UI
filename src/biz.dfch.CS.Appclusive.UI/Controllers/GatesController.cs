using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class GatesController : CoreControllerBase
    {

        // GET: Gates
        public ActionResult Index(int pageNr = 1)
        {
            try
            {
                QueryOperationResponse<Api.Core.Gate> items = CoreRepository.Gates
                        .AddQueryOption("$inlinecount", "allpages")
                        .AddQueryOption("$top", PortalConfig.Pagesize)
                        .AddQueryOption("$skip", (pageNr - 1) * PortalConfig.Pagesize)
                        .Execute() as QueryOperationResponse<Api.Core.Gate>;

                ViewBag.Paging = new PagingInfo(pageNr, items.TotalCount);
                return View(AutoMapper.Mapper.Map<List<Models.Core.Gate>>(items));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new List<Models.Core.Gate>());
            }
        }

        #region Gate

        // GET: Gates/Details/5
        public ActionResult Details(long id)
        {
            try
            {
                var item = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Gate>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.EntityType());
            }
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
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(gate);
            }
        }

        // GET: Gates/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.Gate());
            }
        }

        // POST: Gates/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.Gate gate)
        {
            try
            {
                var apiItem = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();

                #region copy all edited properties

                apiItem.Name = gate.Name;
                apiItem.Description = gate.Description;
                apiItem.Parameters = gate.Parameters;
                apiItem.Status = gate.Status;
                apiItem.Type = gate.Type;

                #endregion
                CoreRepository.UpdateObject(apiItem);
                CoreRepository.SaveChanges();
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                return View(AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(gate);
            }
        }

        // GET: Gates/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.Gate apiItem = null;
            try
            {
                apiItem = CoreRepository.Gates.Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.Gate>(apiItem));
            }
        }


        #endregion

    }
}
