using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class GenericControllerBase<T,M>: ControllerBase
    {       
        protected DataServiceQuery<T> BaseQuery { get; set; }

        // GET: Endpoints
        public ActionResult Index(int pageNr = 1, string searchTerm = null)
        {
            return base.Index<T, M>(BaseQuery, pageNr, searchTerm);
        }

        public ActionResult Search(string term)
        {
            return base.Search(BaseQuery, term);
        }

        //// GET: Endpoints/Details/5
        //public ActionResult Details(long id)
        //{
        //    try
        //    {
        //        var item = BaseQuery.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
        //        return View(AutoMapper.Mapper.Map<M>(item));
        //    }
        //    catch (Exception ex)
        //    {
        //        ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
        //        return View(new Models.Diagnostics.Endpoint());
        //    }
        //}
    }
}