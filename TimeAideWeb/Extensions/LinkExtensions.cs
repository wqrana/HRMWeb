using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using TimeAide.Services.Helpers;

namespace System.Web.Mvc
{
    public static class LinkExtensions
    {
        public static MvcHtmlString ActionLinkView(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string formName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool showActionLinkAsDisabled)
        {
            if (SecurityHelper.AllowEdit(formName))
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return MvcHtmlString.Empty;
        }
        public static MvcHtmlString ActionLinkEdit(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, string formName)
        {
            if (SecurityHelper.AllowEdit(formName))
            {
                return htmlHelper.ActionLink(linkText, actionName, routeValues);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString ActionLinkAdd(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string formName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool showActionLinkAsDisabled)
        {
            if (SecurityHelper.AllowEdit(formName))
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return MvcHtmlString.Empty;
        }

        public static MvcHtmlString ActionLinkDelete(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string formName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool showActionLinkAsDisabled)
        {
            if (SecurityHelper.AllowEdit(formName))
            {
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            return MvcHtmlString.Empty;
        }

    }
}