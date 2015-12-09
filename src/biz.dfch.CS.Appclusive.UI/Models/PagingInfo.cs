using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class PagingInfo
    {
        public PagingInfo(int pageNr, bool hasMore)
        {
            this.PageNr = pageNr;
            this.HasMore = hasMore;
        }
        public int PageNr { get; set; }
        public bool HasMore { get; set; }
    }
}