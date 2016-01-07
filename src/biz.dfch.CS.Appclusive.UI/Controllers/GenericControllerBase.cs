using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public abstract class GenericControllerBase<T, M, I> : ControllerBase
    {
        public GenericControllerBase() : base(typeof(I)) { }

        protected abstract DataServiceQuery<T> BaseQuery { get; }

        // GET: Endpoints
        public virtual ActionResult Index(int pageNr = 1, string searchTerm = null)
        {
            return base.Index<T, M>(BaseQuery, pageNr, searchTerm);
        }

        public ActionResult Search(string term)
        {
            return base.Search(BaseQuery, term);
        }

        public ActionResult Select(string term)
        {
            return base.Select(BaseQuery, term);
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