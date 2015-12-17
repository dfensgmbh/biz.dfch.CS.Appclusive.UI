/**
 * Copyright 2015 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "The intelligent Automation Framework and Middleware";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region Test

        // GET: Aces/Test
        public ActionResult Test()
        {
            return View(new Models.Core.Ace());
        }

        // POST: Aces/Test
        [HttpPost]
        public ActionResult Test(Models.Core.Ace ace)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ace);
                }
                else
                {
                    ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(new AjaxNotificationViewModel(ENotifyStyle.success, "Successfully saved"));
                    return View(ace);
                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(ace);
            }
        }

        public ActionResult TestSearch(string term)
        {
            // Get Tags from database
            string[] tags = { "ASP.NET", "WebForms", 
                    "MVC", "jQuery", "ActionResult", 
                    "MangoDB", "Java", "Windows" };
            return this.Json(tags.Where(t => t.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TestKvSearch(string term)
        {
            List<toption> options = new List<toption>();
            options.Add(new toption() { value = "MangoDB", key = 1 });
            options.Add(new toption() { value = "ActionResult", key = 2 });
            options.Add(new toption() { value = "WebForms", key = 3 });
            options.Add(new toption() { value = "ASP.NET", key = 4 });
            options.Add(new toption() { value = "jQuery", key = 5 });
            options.Add(new toption() { value = "MVC", key = 6 });
            options.Add(new toption() { value = "Windows", key = 7 });
            options.Add(new toption() { value = "Java", key = 8 });
            return this.Json(options.Where(t => t.value.StartsWith(term)), JsonRequestBehavior.AllowGet);
        }

        public class toption
        {
            public long key { get; set; }
            public string value { get; set; }
        }
        #endregion
    }
}