using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace biz.dfch.CS.Appclusive.UI.Helpers
{
    public static class AjaxHelperExtensions
    {
        //public static IHtmlString IconActionLink<T>(this AjaxHelper<T> helper, string iconClass, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        //{
        //    var builder = new TagBuilder("i");
        //    builder.MergeAttribute("class", iconClass);
        //    builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        //    var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
        //    return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.Normal)));
        //}

        public static IHtmlString IconActionLink(this AjaxHelper helper, string iconClass, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttribute("class", iconClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.Normal)));
        }

        public static IHtmlString IconActionLink(this AjaxHelper helper, string iconClass, string actionName, object routeValues, AjaxOptions ajaxOptions, string toolTip, object htmlAttributes = null)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttribute("class", iconClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            if (!string.IsNullOrWhiteSpace(toolTip))
            {
                //title="Delete" data-toggle="tooltip"
                builder.MergeAttribute("title", toolTip);
                builder.MergeAttribute("data-toggle", "tooltip");
            }
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.Normal)));
        }
        
        public static IHtmlString IconActionLink(this AjaxHelper helper, string iconClass, string actionName, RouteValueDictionary routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        {
            var builder = new TagBuilder("i");
            builder.MergeAttribute("class", iconClass);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.Normal)));
        }



    }
}