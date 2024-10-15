using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.Mvc;
using System.Web.UI.WebControls;
//using System.Web.Mvc.Html;
using TimeAide.Web.Models;

namespace TimeAide.Services.Helpers
{
    //public static class CustomHtmlHelper
    //{
    //    public static string CustomDropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
    //        where T : BaseEntity
    //    {
    //        TimeAideContext db = new TimeAideContext();
    //        htmlHelper.ViewData[name] = new SelectList(db.GetAll<T>(), "Id", typeof(T).Name + "Name");
    //        string html = htmlHelper.DropDownList(name, selectList, "Please select", htmlAttributes).ToHtmlString();
    //        return html;
    //    }
    //    public static MvcHtmlString CustomDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
    //    {
    //        //List<SelectListItem> Textes = new List<SelectListItem>();
    //        //foreach (ListItem item in values)
    //        //{
    //        //    SelectListItem selItem = new SelectListItem();
    //        //    if (item.Text.Length <= 20)
    //        //        selItem.Text = item.Text;
    //        //    else
    //        //        selItem.Text = item.Text.Substring(0, 20) + "...";
    //        //    Textes.Add(selItem);
    //        //}
    //        selectList.ToList().Insert(0, new SelectListItem { Selected = true, Text = "Please Select", Value = "0" });
    //        return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlHelper,
    //                                                                 name,
    //                                                                 selectList,
    //                                                                 optionLabel,
    //                                                                 htmlAttributes);
    //    }
    //}
}
