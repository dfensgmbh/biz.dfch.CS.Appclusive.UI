using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using biz.dfch.CS.Appclusive.UI.Models;
using System.Data.Services.Client;
using Api_Cmp = biz.dfch.CS.Appclusive.Core.OdataServices.Cmp;
using M= biz.dfch.CS.Appclusive.UI.Models.Cmp.CimiTarget;

namespace biz.dfch.CS.Appclusive.UI.Controllers
{
    public class CimiTargetsController : CmpControllerBase<Api_Cmp.CimiTarget, Models.Cmp.CimiTarget>
    {
        public CimiTargetsController()
        {
            base.BaseQuery = CmpRepository.CimiTargets;
        }

        // GET: CimiTargets/Details/5
        public ActionResult Details(long id)
        {
            try
            {
                var item = BaseQuery.Expand("CreatedBy").Expand("ModifiedBy").Where(c => c.Id == id).FirstOrDefault();
                return View(AutoMapper.Mapper.Map<M>(item));
            }
            catch (Exception ex)
            {
                ((List<AjaxNotificationViewModel>)ViewBag.Notifications).AddRange(ExceptionHelper.GetAjaxNotifications(ex));
                return View(new Models.Diagnostics.Endpoint());
            }
        }
    }
}
