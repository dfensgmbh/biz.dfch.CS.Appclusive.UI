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
using Microsoft.Ajax.Utilities;

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
            Contract.Assert(pageNr > 0);
            Contract.Assert(pageSize > 0);
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

    public class PagingFilterInfo
    {
        public PagingFilterInfo() : this(null)
        {
            ;
        }

        public PagingFilterInfo(Uri nextLink)
        {
            this.NextLink = nextLink;
        }

        public Uri NextLink { get; set; }
        public Uri PreviousLink { get; set; }

        public int PreviousSkipCount
        {
            get { return PreviousLink == null ? 0 : GetSkipFromUri(PreviousLink); }
        }

        public int NextSkipCount
        {
            get { return NextLink == null ? 0 : GetSkipFromUri(NextLink); }
        }

        public static int GetSkipFromUri(Uri uri)
        {
            var query = HttpUtility.ParseQueryString(uri.Query);
            var skip = query.Get("skip") ?? query.Get("$skip");
            var skipVaule = string.IsNullOrEmpty(skip) ? 0 : int.Parse(skip);

            return skipVaule;
        }

        public static Uri ReplaceSkipInUri(Uri uri, int skip)
        {
            var query = HttpUtility.ParseQueryString(uri.Query);
            var skipString = query.Get("skip") != null ? "skip" : "$skip";
            return new Uri(Regex.Replace(uri.AbsoluteUri, "\\" + skipString  + "=\\d+", skipString +"=" + skip));
        }

        public static Uri BuildPreviousLink(Uri currentUri)
        {
            var currentSkip = GetSkipFromUri(currentUri);
            var previousSkip = currentSkip - PortalConfig.Pagesize;

            previousSkip = (previousSkip < 0 ? 0 : previousSkip);

            return ReplaceSkipInUri(currentUri, previousSkip);
        }
    }
}