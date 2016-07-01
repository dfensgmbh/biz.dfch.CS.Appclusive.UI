using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using biz.dfch.CS.Appclusive.UI.Controllers;
using biz.dfch.CS.Appclusive.UI.Helpers;

namespace biz.dfch.CS.Appclusive.UI.ActionFilter
{
    public class AuthenticationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.GetType() == typeof(LoginController))
            {
                return;
            }

            if (IsJwtHeaderPresent(filterContext))
            {
                JwtHelper.JwtHeader = filterContext.HttpContext.Request.Headers[JwtHelper.JwtHeaderKey];
            }

            if (IsLoginDataPresent(filterContext))
            {
                return;
            }

            RedirectToLoginPage(filterContext);
        }

        private void RedirectToLoginPage(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"action", "Index"},
                    {"controller", "Login"}
                });
        }

        private bool IsJwtHeaderPresent(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers.AllKeys.Any(key => key.Equals(JwtHelper.JwtHeaderKey));
        }
        
        private bool IsLoginDataPresent(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext != null
                   && filterContext.HttpContext.Session != null
                   && filterContext.HttpContext.Session["LoginData"] != null;
        }
    }
}