using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Linq.Expressions;

namespace TimeAide.Web.Extensions
{
    public static class DisplayDateForExtensions
    {
        public static MvcHtmlString DisplayFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            //checked in branch
            return new MvcHtmlString("");
        }
    }
}