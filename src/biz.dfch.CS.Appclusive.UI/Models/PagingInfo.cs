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

        public PagingInfo(int pageNr, long itemCount)
        {
            this.PageNr = pageNr;
            this.ItemCount = itemCount;
            this.PageCount = (long)Math.Ceiling((double)this.ItemCount / PortalConfig.Pagesize);
            this.HasMore = this.PageCount > this.PageNr;
        }

        public int PageNr { get; set; }
        public bool HasMore { get; set; }

        public long PageCount { get; set; }

        public long ItemCount { get; set; }
    }
}