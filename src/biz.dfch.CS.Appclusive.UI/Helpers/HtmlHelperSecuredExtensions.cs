using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Helpers
{
    public static class HtmlHelperSecuredExtensions
    {
        public static MvcHtmlString SecuredButton<TModel>(this HtmlHelper<TModel> htmlHelper, string actionName, string urlAction)
        {
            switch (actionName.ToLower())
            {
                case "edit":

                    if (PermissionDecisions.Current.CanUpdate(typeof(TModel)))
                    {
                        // <a class="btn btn-default btn-sm" href="@Url.Action("Edit", new { id = item.Id })"><i class="fa fa-pencil "></i> @GeneralResources.EditLink</a>
                        string link = @"<a class=""btn btn-default btn-sm"" href=""{0}""><i class=""fa fa-pencil ""></i> {1}</a>";
                        string s = string.Format(link, urlAction, GeneralResources.EditLink);
                        return new MvcHtmlString(s);
                    }
                    break;

                case "details":
                    if (PermissionDecisions.Current.CanRead(typeof(TModel)))
                    {
                        // <a class="btn btn-primary btn-sm" href="@Url.Action("Details", new { id = item.Id })"><i class="fa fa-cog"></i> @GeneralResources.DetailsLink</a>
                        string link = @"<a class=""btn btn-primary btn-sm"" href=""{0}""><i class=""fa fa-cog ""></i> {1}</a>";
                        string s = string.Format(link, urlAction, GeneralResources.DetailsLink);
                        return new MvcHtmlString(s);
                    }
                    break;

                case "delete":
                    if (PermissionDecisions.Current.CanDelete(typeof(TModel)))
                    {
                        // <a class="btn btn-default btn-sm" href="@Url.Action("Delete", new { id = item.Id })" onclick="return confirm('@GeneralResources.ConfirmDelete')" title="Delete" data-toggle="tooltip"><i class="fa fa-trash-o"></i></a>
                        string link = @"<a class=""btn btn-default btn-sm"" href=""{0}"" onclick=""return confirm('{1}')"" title=""{2}"" data-toggle=""tooltip""><i class=""fa fa-trash-o""></i></a>";

                        string s = string.Format(link, urlAction, GeneralResources.ConfirmDelete, "Delete"); // TODO cwi: GeneralResources.DeleteLink
                        return new MvcHtmlString(s);
                    }
                    break;

                case "approve":
                    if (PermissionDecisions.Current.CanRead(typeof(TModel)))
                    {
                        // <a class="btn btn-success btn-sm" href="@Url.Action("Approve", new { id = item.Id })"><i class="fa fa-cog"></i> @GeneralResources.Approve</a>
                        string link = @"<a class=""btn btn-success btn-sm"" href=""{0}""><i class=""fa fa-cog ""></i> {1}</a>";
                        string s = string.Format(link, urlAction, GeneralResources.Approve);
                        return new MvcHtmlString(s);
                    }
                    break;

                case "decline":
                    if (PermissionDecisions.Current.CanRead(typeof(TModel)))
                    {
                        // <a class="btn btn-warning btn-sm" href="@Url.Action("Decline", new { id = item.Id })"><i class="fa fa-cog"></i> @GeneralResources.Decline</a>
                        string link = @"<a class=""btn btn-warning btn-sm"" href=""{0}""><i class=""fa fa-cog ""></i> {1}</a>";
                        string s = string.Format(link, urlAction, GeneralResources.Decline);
                        return new MvcHtmlString(s);
                    }
                    break;

                default:
                    break;
            }
            return MvcHtmlString.Create(string.Empty);
        }

        public static MvcHtmlString SecuredListItem<TModel>(this HtmlHelper<TModel> htmlHelper, Type type, MvcHtmlString urlAction)
        {
            if (PermissionDecisions.Current.CanRead(type))
            {
                // <li>@Html.ActionLink(@GeneralResources.Catalogue, "Index", "Catalogues")</li>
                string link = @"<li>{0}</li>";
                return new MvcHtmlString(string.Format(link, urlAction));
            }
            return MvcHtmlString.Create(string.Empty);
        }

        // TODO cwi: Merge with SecuredListItem 
        public static MvcHtmlString SecuredListItem2<TModel>(this HtmlHelper<TModel> htmlHelper, Type type, string iconclass, MvcHtmlString urlAction)
        {
            if (PermissionDecisions.Current.CanRead(type))
            {
                // <li class="list-group-item"><i class="fa fa-book"></i> @Html.ActionLink(@GeneralResources.Catalogue, "Index", "Catalogues")</li>
                string link = @"<li class=""list-group-item""><i class=""{0}""></i>{1}</li>";
                return new MvcHtmlString(string.Format(link, iconclass, urlAction));
            }
            return MvcHtmlString.Create(string.Empty);
        }
    }
}