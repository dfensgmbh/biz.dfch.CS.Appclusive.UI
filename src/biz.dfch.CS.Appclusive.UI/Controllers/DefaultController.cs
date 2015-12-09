using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class DefaultController : Controller
    {
        //// GET: Default
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // 
        // GET: Default

        public string Index()
        {
            return "This is my <b>default</b> action...";
        }

        // 
        // GET: Default/Welcome/ 

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        // http://localhost:xxxx/Default/TestQueryString?name=Edgar&numtimes=42
        public string TestQueryString(string name, int numTimes = 1)
        {
            return HttpUtility.HtmlEncode("Hello " + name + ", NumTimes is: " + numTimes);
        }

        // http://localhost:xxxx/Default/TestQueryString/42?name=Edgar
        public string TestParameters(string name, int ID = 1)
        {
            return HttpUtility.HtmlEncode("Hello " + name + ", ID: " + ID);
        }
    }
}