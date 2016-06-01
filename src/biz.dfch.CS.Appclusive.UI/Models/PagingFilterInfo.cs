using System;
using System.Text.RegularExpressions;
using System.Web;
using biz.dfch.CS.Appclusive.UI.Config;

namespace biz.dfch.CS.Appclusive.UI.Models
{
    public class PagingFilterInfo
    {
        public PagingFilterInfo()
            : this(null)
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
            return new Uri(Regex.Replace(uri.AbsoluteUri, "\\" + skipString + "=\\d+", skipString + "=" + skip));
        }

        public static Uri BuildPreviousLink(Uri currentUri)
        {
            var currentSkip = GetSkipFromUri(currentUri);
            var previousSkip = currentSkip - PortalConfig.Pagesize;

            previousSkip = (previousSkip < 0 ? 0 : previousSkip);

            return ReplaceSkipInUri(currentUri, previousSkip);
        }

        public bool HasMore
        {
            get { return NextLink != null; }
        }
    }
}