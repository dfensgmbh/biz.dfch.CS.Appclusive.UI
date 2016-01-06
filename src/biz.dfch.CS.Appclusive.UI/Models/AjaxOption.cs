using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    /// <summary>
    /// class fits to jquery autocomplete
    /// </summary>
    public class AjaxOption
    {
        public AjaxOption(object keyParam, string nameParam)
        {
            key = keyParam;
            value=nameParam;
        }
            
        public object key { get; set; }
        public string value { get; set; }
    }
}