using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login?ReturnUrl=%2fbiz.dfch.CS.Appclusive.UI%2f
        public ActionResult Index(string returnUrl=null)
        {
            Models.LoginData data = new Models.LoginData()
            {
                ReturnUrl = returnUrl,
                Username = "Administrator",
                Domain = "mgmtscc"
            };
            return View(data);
        }

        // POST: Login
        [HttpPost]
        public ActionResult Index(Models.LoginData data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(data);
                }
                else
                {
                    if (DoLogin(data))
                    {
                        if (string.IsNullOrWhiteSpace(data.ReturnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return Redirect(data.ReturnUrl);
                        }
                    }
                    else
                    {
                        return View(data);
                    }

                }
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(data);
            }
        }
       
        public ActionResult Logout(string returnUrl = null)
        {
            FormsAuthentication.SignOut();
            Session["LoginData"] = null;
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        /// <summary>
        /// Dummy method for develop use only
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool DoLogin(Models.LoginData data)
        {
            bool isAuthenticated = true; // CheckPassword(username, password);
            if (isAuthenticated)
            {
                FormsAuthentication.RedirectFromLoginPage(data.Username, false);
                Session["LoginData"] = data;
                ////this.HttpContext. .GetOwinContext().Authentication
                ////         .SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ////Claim claim1 = new Claim(ClaimTypes.Name, username);
                ////Claim[] claims = new Claim[] { claim1 };
                ////ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                ////this.HttpContext.GetOwinContext().Authentication
                ////    .SignIn(new AuthenticationProperties() { IsPersistent = false }, claimsIdentity);
            }
            return isAuthenticated;
        }

    }
}