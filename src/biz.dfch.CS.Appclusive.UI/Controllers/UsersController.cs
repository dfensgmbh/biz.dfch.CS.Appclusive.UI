using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class UsersController : CoreControllerBase<Api.Core.User, Models.Core.User, object>
    {
        protected override DataServiceQuery<Api.Core.User> BaseQuery { get { return CoreRepository.Users; } }

        // GET: Users/Details/5
        public ActionResult Details(long id, string rId = "0", string rAction = null, string rController = null)
        {
            ViewBag.ReturnId = rId;
            ViewBag.ReturnAction = rAction;
            ViewBag.ReturnController = rController;
            try
            {
                var item = CoreRepository.Users.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.User>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.User());
            }
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View(new Models.Core.User());
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult Create(Models.Core.User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                else
                {
                    var apiItem = AutoMapper.Mapper.Map<Api.Core.User>(user);

                    CoreRepository.AddToUsers(apiItem);
                    CoreRepository.SaveChanges();

                    return RedirectToAction("Details", new { id = apiItem.Id });
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(user);
            }
        
        }

        // GET: Users/Edit/5
        public ActionResult Edit(long id)
        {
            try
            {
                var apiItem = CoreRepository.Users.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<Models.Core.User>(apiItem));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Core.User());
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        public ActionResult Edit(long id, Models.Core.User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                else
                {
                    var apiItem = CoreRepository.Users.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();

                    #region copy all edited properties

                    apiItem.ExternalId = user.ExternalId;
                    apiItem.Name = user.Name;
                    apiItem.Description = user.Description;
                    apiItem.ExternalType = user.ExternalType;
                    apiItem.Mail = user.Mail;

                    #endregion
                    CoreRepository.UpdateObject(apiItem);
                    CoreRepository.SaveChanges();
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(AutoMapper.Mapper.Map<Models.Core.User>(apiItem));
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(user);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(long id)
        {
            Api.Core.User apiItem = null;
            try
            {
                apiItem = CoreRepository.Users.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                CoreRepository.DeleteObject(apiItem);
                CoreRepository.SaveChanges();
                return RedirectToAction("Index", new { d = id });
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View("Details", AutoMapper.Mapper.Map<Models.Core.User>(apiItem));
            }
        }
    }
}
