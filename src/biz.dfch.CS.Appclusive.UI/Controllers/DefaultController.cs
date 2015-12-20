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