using System.Web.Mvc;
using System.Web.Routing;
using biz.dfch.CS.Appclusive.UI.Controllers;

namespace biz.dfch.CS.Appclusive.UI.ActionFilter
{
    public class AuthenticationActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.GetType() == typeof (LoginController))
                return;

            if (filterContext.HttpContext == null ||
                filterContext.HttpContext.Session == null ||
                filterContext.HttpContext.Session["LoginData"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"action", "Index"},
                    {"controller", "Login"}
                });
            }
        }
    }
}