using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class AjaxNotificationViewModel
    {
        public string Message { get; set; }
        public ENotifyStyle Level { get; set; }
    }

    public enum ENotifyStyle
    {
        success,
        info,
        warn,
        error
        
    }
}