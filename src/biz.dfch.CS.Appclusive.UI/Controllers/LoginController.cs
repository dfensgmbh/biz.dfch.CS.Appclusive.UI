using biz.dfch.CS.Appclusive.UI.Models;
using biz.dfch.CS.Appclusive.UI.Navigation;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web.Security;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    [AllowAnonymous]
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
            Session["LoginData"] = null;
            Session["PermissionDecisions"] = null;

            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction("Index", "Login");
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
        private bool DoLogin(LoginData data)
        {
            var isAuthenticated = Login(data);

            if (isAuthenticated)
            {
                Session["LoginData"] = new System.Net.NetworkCredential(data.Username, data.Password, data.Domain);
                Session["PermissionDecisions"] = new PermissionDecisions(data.Username, data.Domain);
                
                FormsAuthentication.RedirectFromLoginPage(data.Username, false);
            }

            return isAuthenticated;
        }

        private static bool Login(LoginData data)
        {
            Api.Core.Core coreRepository = null;

            try
            {
                System.Net.NetworkCredential apiCreds = new System.Net.NetworkCredential(data.Username, data.Password,
                    data.Domain);

                Contract.Assert(null != apiCreds);

                coreRepository = new Api.Core.Core(new Uri(Properties.Settings.Default.AppclusiveApiBaseUrl + "Core"))
                {
                    IgnoreMissingProperties = true,
                    SaveChangesDefaultOptions = SaveChangesOptions.PatchOnUpdate,
                    MergeOption = MergeOption.PreserveChanges,
                    TenantID = PermissionDecisions.Current.Tenant.Id.ToString(),
                    Credentials = apiCreds
                };

                coreRepository.Format.UseJson();

                coreRepository.Tenants.FirstOrDefault();

                return true;
            }
            catch (DataServiceQueryException ex)
            {
                if (ex.Response.StatusCode == 401)
                    throw new AuthenticationException("Invalid credentials", ex);

                throw;
            }
            finally
            {
                coreRepository = null;
            }
        }
    }
}