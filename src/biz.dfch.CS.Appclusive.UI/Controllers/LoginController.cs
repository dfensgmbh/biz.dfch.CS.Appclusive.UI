using biz.dfch.CS.Appclusive.UI.Models;
using biz.dfch.CS.Appclusive.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using biz.dfch.CS.Appclusive.UI.Config;
using biz.dfch.CS.Appclusive.UI.Helpers;
using biz.dfch.CS.Appclusive.UI.Managers;
using Microsoft.Ajax.Utilities;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    [System.Web.Mvc.AllowAnonymous]
    public class LoginController : Controller
    {
        public LoginController()
        {
            ViewBag.Notifications = new List<AjaxNotificationViewModel>();
        }

        // GET: Login?ReturnUrl=%2fbiz.dfch.CS.Appclusive.UI%2f
        public ActionResult Index(string returnUrl=null)
        {
            LoginData data = new LoginData()
            {
                ReturnUrl = returnUrl,
                Username = Properties.Settings.Default.DefaultLoginUsername,
                Domain = Properties.Settings.Default.DefaultLoginDomain
            };
            return View(data);
        }

        // POST: Login
        [System.Web.Mvc.HttpPost]
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
            catch (AuthenticationException ex)
            {
                var error = new AjaxNotificationViewModel()
                {
                    Level = ENotifyStyle.error,
                    Message = ex.Message
                };

                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).Add(error);

                return View(data);
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
            Session.Clear();

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }

        // [GET] ~/Login/FromPortal/?accessToken={A12...}&tenantId={B34..}
        [System.Web.Mvc.HttpGet]
        public ActionResult FromPortal([FromUri]string accessToken, [FromUri]Guid tenantId)
        {
            Contract.Assert(null != accessToken);
            Contract.Assert(Guid.Empty != tenantId);

            try
            {
                if (HttpContext == null || HttpContext.Session == null)
                {
                    throw new ApplicationException("Server does not support Sesions.");
                }

                AccessTokenHelper.AccessToken = accessToken;
                TenantHelper.FixedTenantId = tenantId;

                new AuthenticatedDiagnosticsApi().InvokeEntitySetActionWithVoidResult("Endpoints", "AuthenticatedPing", null);
                
                return RedirectToAction("Index", "Home");
            }
            catch (DataServiceQueryException ex)
            {
                if (ex.Response.StatusCode == 401)
                {
                    throw new AuthenticationException("Invalid credentials", ex);
                }

                throw;
            }
        }

        /// <summary>
        /// Dummy method for develop use only
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool DoLogin(LoginData data)
        {
            var isAuthenticated = Login(data);

            if (isAuthenticated)
            {
                if (!data.Username.Contains("\\"))
                {
                    data.Username = string.Format("{0}\\{1}", data.Domain, data.Username);
                }

                var credentials = new System.Net.NetworkCredential(data.Username, data.Password, data.Domain);

                Session["LoginData"] = credentials;
                Session["PermissionDecisions"] = new PermissionDecisions(data.Username);
                
                FormsAuthentication.RedirectFromLoginPage(data.Username, false);
            }

            return isAuthenticated;
        }

        private static bool Login(LoginData data)
        {
            try
            {
                System.Net.NetworkCredential apiCreds = new System.Net.NetworkCredential(data.Username, data.Password,
                    data.Domain);

                Contract.Assert(null != apiCreds);

                var repo = new AuthenticatedDiagnosticsApi(apiCreds);
                repo.InvokeEntitySetActionWithVoidResult("Endpoints", "AuthenticatedPing", null);
                
                return true;
            }
            catch (DataServiceQueryException ex)
            {
                if (ex.Response.StatusCode == 401)
                    throw new AuthenticationException("Invalid credentials", ex);

                throw;
            }
        }
    }
}