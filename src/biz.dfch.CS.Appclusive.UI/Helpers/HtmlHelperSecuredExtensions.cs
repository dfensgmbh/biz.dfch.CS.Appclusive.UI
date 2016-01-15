using biz.dfch.CS.Appclusive.UI.App_LocalResources;
using biz.dfch.CS.Appclusive.UI.Navigation;
using System;
using System.Web.Mvc;

namespace biz.dfch.CS.Appclusive.UI.Helpers
{
    public static class HtmlHelperSecuredExtensions
    {
        #region Secured buttons

        public static MvcHtmlString SecuredButton<TModel>(this HtmlHelper<TModel> htmlHelper, string permissionCRUD, string urlAction, string aclass = "", string displayText = "", string iclass = "")
        {
            string ret = string.Empty;           
            string link = @"<a class=""btn {0}"" href=""{1}""><i class=""fa {2} ""></i> {3}</a>";
            Type type = GetItemType(typeof(TModel));

            switch (permissionCRUD.ToLower())
            {
                case "create":
                    if (PermissionDecisions.Current.CanCreate(type))
                    {
                        // <a class="btn btn-default" href="@Url.Action("Create")"><i class="fa fa-plus"></i> @GeneralResources.CreateNew</a>
                        if (string.IsNullOrEmpty(aclass)) aclass = "btn-default";
                        if (string.IsNullOrEmpty(iclass)) iclass = "fa-plus";
                        if (string.IsNullOrEmpty(displayText)) displayText = GeneralResources.CreateNew;
                        ret = string.Format(link, aclass, urlAction, iclass, displayText);
                    }
                    break;

                case "read":
                    if (PermissionDecisions.Current.CanRead(type))
                    {
                        // <a class="btn btn-primary btn-sm" href="@Url.Action("Details", new { id = item.Id })"><i class="fa fa-cog"></i> @GeneralResources.DetailsLink</a>
                        if (string.IsNullOrEmpty(aclass)) aclass = "btn-primary btn-sm";
                        if (string.IsNullOrEmpty(iclass)) iclass = "fa-cog";
                        if (string.IsNullOrEmpty(displayText)) displayText = GeneralResources.DetailsLink;
                        ret = string.Format(link, aclass, urlAction, iclass, displayText);

                    }
                    break;

                case "update":
                    if (PermissionDecisions.Current.CanUpdate(type))
                    {
                        // <a class="btn btn-default btn-sm" href="@Url.Action("Edit", new { id = item.Id })"><i class="fa fa-pencil "></i> @GeneralResources.EditLink</a>
                        if (string.IsNullOrEmpty(aclass)) aclass = "btn-default btn-sm";
                        if (string.IsNullOrEmpty(iclass)) iclass = "fa-pencil";
                        if (string.IsNullOrEmpty(displayText)) displayText = GeneralResources.EditLink;
                        ret = string.Format(link, aclass, urlAction, iclass, displayText);
                    }
                    break;

                case "delete":
                    if (PermissionDecisions.Current.CanDelete(type))
                    {
                        // <a class="btn btn-default btn-sm" href="@Url.Action("Delete", new { id = item.Id })" onclick="return confirm('@GeneralResources.ConfirmDelete')" title="Delete" data-toggle="tooltip"><i class="fa fa-trash-o"></i></a>
                        link = @"<a class=""btn {0}"" href=""{1}"" onclick=""return confirm('{4}')"" title=""{3}"" data-toggle=""tooltip""><i class=""fa {2} ""></i></a>";
                        if (string.IsNullOrEmpty(aclass)) aclass = "btn-danger btn-sm";
                        if (string.IsNullOrEmpty(iclass)) iclass = "fa-trash-o";
                        if (string.IsNullOrEmpty(displayText)) displayText = GeneralResources.ConfirmDelete;
                        ret = string.Format(link, aclass, urlAction, iclass, displayText, GeneralResources.Delete);

                    }
                    break;

                default:
                    break;
            }
            return new MvcHtmlString(ret);
        }

        public static MvcHtmlString SecuredButtonSeePassword<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            Type type = GetItemType(typeof(TModel));
            if (PermissionDecisions.Current.CanDecrypt(type))
            {
                // <a class="btn btn-primary" onclick="togglePW()" id="pwBtn">@GeneralResources.SeePassword</a>
                string link = @"<a class=""btn btn-primary"" onclick=""togglePW()"" id=""pwBtn"">{0}</a>";
                string s = string.Format(link, GeneralResources.SeePassword);
                return new MvcHtmlString(s);
            }
            return MvcHtmlString.Create(string.Empty);
        }

        #endregion

        #region SecuredListItems

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

        public static MvcHtmlString SecuredListItem2<TModel>(this HtmlHelper<TModel> htmlHelper, Type type, string iconclass, MvcHtmlString urlAction)
        {
            if (PermissionDecisions.Current.CanRead(type))
            {
                // <li class="list-group-item"><i class="fa fa-book"></i> @Html.ActionLink(@GeneralResources.Catalogue, "Index", "Catalogues")</li>
                string link = @"<li class=""list-group-item""><i class=""{0}""></i> {1}</li>";
                return new MvcHtmlString(string.Format(link, iconclass, urlAction));
            }
            return MvcHtmlString.Create(string.Empty);
        }

        #endregion

        #region private helpers

        /// <summary>
        /// Returns T from IEnumerable<T>, if t is of type IEnumerable<>. (GetItemType(IEnumerable<Customer>))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Type GetItemType(Type t)
        {
            if (t.GetGenericArguments().Length > 0)
            {
                return t.GetGenericArguments()[0];
            }
            return t;
        }

        #endregion
    }
}