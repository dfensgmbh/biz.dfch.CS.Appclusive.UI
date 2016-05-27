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

using biz.dfch.CS.Appclusive.UI.Config;
using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
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
            : this(pageNr, itemCount, PortalConfig.Pagesize)
        {
        }

        public PagingInfo(int pageNr, long itemCount, int pageSize)
        {
            Contract.Assert(pageNr>0);
            Contract.Assert(pageSize>0);
            this.PageNr = pageNr;
            this.ItemCount = itemCount;
            this.PageCount = (long)Math.Ceiling((double)this.ItemCount / pageSize);
            this.HasMore = this.PageCount > this.PageNr;
        }

        public int PageNr { get; set; }
        public bool HasMore { get; set; }

        public long PageCount { get; set; }

        public long ItemCount { get; set; }
    }

    public class PagingInfo2
    {
        public PagingInfo2(Uri nextLink)
            : this(nextLink, PortalConfig.Pagesize)
        {
        }

        public PagingInfo2(Uri nextLink, int pageSize)
        {
            Contract.Assert(pageSize > 0);
            this.NextLink = nextLink;

            if (nextLink != null)
            {
                var query = HttpUtility.ParseQueryString(nextLink.Query);
                var s = query.Get("$skip");
                var prevSkip = int.Parse(s) - pageSize;

                prevSkip = (prevSkip < 0 ? 0 : prevSkip);

                PrevLink = new Uri(Regex.Replace(nextLink.AbsoluteUri, "\\$skip=\\d+", "$skip=" + prevSkip));
            }
        }

        public Uri NextLink { get; set; }
        public Uri PrevLink { get; set; }
    }
}