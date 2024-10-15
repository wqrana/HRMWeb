using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TimeAide.Web.Models;
using System.Web.Mvc.Html;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Services;

namespace System.Web.Mvc
{
    public static class DropDownListExtensions
    {
        // Get the select list from Enum values for dropdown
        public static System.Web.Mvc.SelectList ToSelectList<TEnum>(this TEnum obj)
            where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            return new SelectList(Enum.GetValues(typeof(TEnum))
            .OfType<Enum>()
            .Select(x => new SelectListItem
            {
                Text = Enum.GetName(typeof(TEnum), x),
                Value = (Convert.ToInt32(x))
                .ToString()
            }), "Value", "Text");
        }
        public static MvcHtmlString CustomDropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, object htmlAttributes)
            where T : BaseEntity
        {
            return CustomDropDownList<T>(htmlHelper, name, selectList, null, htmlAttributes);
        }
        public static MvcHtmlString CustomDropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, int? selectedValue, object htmlAttributes)
            where T : BaseEntity
        {
            return CustomDropDownList<T>(htmlHelper, name, selectList, selectedValue, null, htmlAttributes);
        }
        public static MvcHtmlString CustomDropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, int? selectedValue, int? parentId, object htmlAttributes)
        where T : BaseEntity
        {
            return CustomDropDownList<T>(htmlHelper, name, selectList, selectedValue, true, parentId, htmlAttributes, "- Please select -");
        }
        public static MvcHtmlString CustomDropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, int? selectedValue, bool showplus, int? parentId, object htmlAttributes, string placeHolder)
        where T : BaseEntity
        {
            TimeAideContext db = new TimeAideContext();
            string html = "";

            SetViewData<T>(htmlHelper, name, selectedValue, parentId, db);


            if (string.IsNullOrEmpty(placeHolder))
                placeHolder = "- Please select -";
            html = htmlHelper.DropDownList(name, selectList, placeHolder, htmlAttributes).ToHtmlString();
            if (showplus)
            {
                showplus = SecurityHelper.AllowAdd(typeof(T).Name);
            }
            if (!showplus || typeof(T) == typeof(Client) || typeof(T) == typeof(Company) || typeof(T) == typeof(EmailType) || typeof(T).IsSubclassOf(typeof(BaseGlobalEntity)))
            {
                return new MvcHtmlString(html);
            }
            string abc = "<button class=\"btn btn-secondary\" type=\"button\" onclick=\"AddToDropDown('" + typeof(T).Name + "','" + name + "');\">+</a></button>";
            html = "<div class=\"input-group\">" + html + "<div class=\"input-group-append\">" + abc + "</div></div>";
            return new MvcHtmlString(html);
        }
        public static MvcHtmlString CustomDropDownList<T>(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, int? selectedValue, bool showplus, int? parentId, object htmlAttributes)
        where T : BaseEntity
        {
            return CustomDropDownList<T>(htmlHelper, name, selectList, selectedValue, showplus, parentId, htmlAttributes, "- Please select -");
        }
        public static MvcHtmlString CustomDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            selectList.ToList().Insert(0, new SelectListItem { Selected = true, Text = "Please Select", Value = "0" });
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, htmlAttributes);
        }

        private static void SetViewData<T>(HtmlHelper htmlHelper, string name, int? selectedValue, int? parentId, TimeAideContext db) where T : BaseEntity
        {
            if (typeof(T) != typeof(City) && typeof(T) != typeof(State) && typeof(T) != typeof(Company) && typeof(T) != typeof(GLAccount))
            {
                string nameField = typeof(T).Name + "Name";
                if (typeof(T) == typeof(WCClassCode))
                    nameField = "ClassName";
                else if (typeof(T) == typeof(DependentStatus))
                    nameField = "StatusName";
                else if (typeof(T) == typeof(CompanyWithholding))
                    nameField = "WithHoldingName";
                if (typeof(T) == typeof(Role))
                {
                    htmlHelper.ViewData[name] = new SelectList(RoleService.GetRole(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderBy(o => o.RoleName), "Id", nameField, selectedValue);
                }
                else if (typeof(T) == typeof(SubDepartment))
                {
                    htmlHelper.ViewData[name] = new SelectList(SubDepartmentService.SubDepartments(parentId).OrderBy(o => o.SubDepartmentName), "Id", nameField, selectedValue);
                }
                else if (typeof(T) == typeof(CompanyCompensation))
                {
                    htmlHelper.ViewData[name] = new SelectList(db.GetAllByCompany<T>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList(), "Id", "CompensationName", selectedValue);
                }
                else if (typeof(T).IsSubclassOf(typeof(BaseCompanyObjects)))
                {
                    htmlHelper.ViewData[name] = new SelectList(db.GetAllByCompany<T>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).ToList(), "Id", nameField, selectedValue);
                }
                else if (typeof(T) == typeof(Module))
                {
                    htmlHelper.ViewData[name] = new SelectList(db.GetAll<Module>().OrderBy(m => m.ModuleName).OrderBy(o => o.ModuleName), "Id", nameField, selectedValue);
                }
                else if (typeof(T).IsSubclassOf(typeof(BaseGlobalEntity)))
                {
                    htmlHelper.ViewData[name] = new SelectList(db.GetAll<T>(), "Id", nameField, selectedValue);
                }

                else if (typeof(T) == typeof(Employment))
                {
                    nameField = "EmploymentRange";
                    htmlHelper.ViewData[name] = new SelectList(db.GetAllByCompany<T>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", nameField, selectedValue);
                }
                else if (typeof(T) == typeof(Credential) && SecurityHelper.IsUser)
                {
                    htmlHelper.ViewData[name] = new SelectList(db.GetAll<Credential>(SessionHelper.SelectedClientId).Where(c => c.SelfServiceUpload), "Id", nameField, selectedValue);
                }
                else if (typeof(T) == typeof(Document))
                {
                    if (SecurityHelper.IsUser)
                        htmlHelper.ViewData[name] = new SelectList(DocumentService.GetDocuments(true).Where(c => c.SelfServiceUpload), "Id", nameField, selectedValue);
                    else
                        htmlHelper.ViewData[name] = new SelectList(DocumentService.GetDocuments(true), "Id", nameField, selectedValue);
                }
                else if (typeof(T) == typeof(CustomField))
                {
                    htmlHelper.ViewData[name] = new SelectList(CustomFieldService.GetCustomFields(true), "Id", nameField, selectedValue);
                }
                else
                    htmlHelper.ViewData[name] = new SelectList(db.GetAll<T>(SessionHelper.SelectedClientId), "Id", nameField, selectedValue);
            }

            else
            {
                if (typeof(T) == typeof(Company))
                {
                    List<int> companyIds = new List<int>();
                    if (!string.IsNullOrEmpty(SessionHelper.SupervisorCompany))
                        companyIds = SessionHelper.SupervisorCompany.Split(',').Select(int.Parse).ToList();
                    var companies = db.Company.Where(c => c.DataEntryStatus == 1 && c.ClientId == SessionHelper.SelectedClientId && (SecurityHelper.IsSuperAdmin || companyIds.Contains(c.Id)));
                    htmlHelper.ViewData[name] = new SelectList(companies, "Id", typeof(T).Name + "Name", selectedValue);
                }
                else if (parentId.HasValue)
                {
                    if (typeof(T) == typeof(City))
                        htmlHelper.ViewData[name] = new SelectList(db.City.Where(c => c.DataEntryStatus == 1 && c.StateId == parentId).OrderBy(o => o.CityName), "Id", typeof(T).Name + "Name", selectedValue);
                    else if (typeof(T) == typeof(State))
                        htmlHelper.ViewData[name] = new SelectList(db.State.Where(c => c.DataEntryStatus == 1 && c.CountryId == parentId).OrderBy(o => o.StateName), "Id", typeof(T).Name + "Name", selectedValue);
                    else if (typeof(T) == typeof(GLAccount))
                    {
                        if (parentId == 1)
                        {
                            htmlHelper.ViewData[name] = new SelectList(db.GetAllByCompany<GLAccount>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                .Where(c => c.GLAccountTypeId == 1 || c.GLAccountTypeId == 5).OrderBy(o => o.GLAccountName), "Id", typeof(T).Name + "Name", selectedValue);
                        }
                        else if (parentId == 2)
                        {
                            htmlHelper.ViewData[name] = new SelectList(db.GetAllByCompany<GLAccount>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                .Where(c => c.GLAccountTypeId == 2 || c.GLAccountTypeId == 5).OrderBy(o => o.GLAccountName), "Id", typeof(T).Name + "Name", selectedValue);
                        }
                        else if (parentId == 0)
                        {
                            htmlHelper.ViewData[name] = new SelectList(db.GetAllByCompany<GLAccount>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderBy(o => o.GLAccountName), "Id", typeof(T).Name + "Name", selectedValue);
                        }
                    }
                }
                else
                    htmlHelper.ViewData[name] = new SelectList(db.GetAll<T>(SessionHelper.SelectedClientId).Take(0), "Id", typeof(T).Name + "Name", selectedValue);
            }
        }
    }
}