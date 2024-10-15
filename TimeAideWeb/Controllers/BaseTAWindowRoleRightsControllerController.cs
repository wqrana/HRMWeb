
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using System.Data.Entity;
using System.Net;
using TimeAide.Services.Helpers;
using System.Diagnostics;
using Newtonsoft.Json;
using TimeAide.Data;

namespace TimeAide.Web.Controllers
{
    public abstract class BaseTAWindowRoleRightsController<T> : TimeAideWebBaseControllers where T : BaseEntity, new()
    {
        public TimeAideContext timeAideWebContext;
        public TimeAideWindowContext timeAideWindowContext;
        RoleFormPrivilegeViewModel1 privileges;
        protected string FormName
        {
            get;
            set;
        }
        internal Form Form
        {
            get;
            set;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (String.IsNullOrWhiteSpace(SessionHelper.LoginEmail))
            {
               // filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" }, { "ReturnUrl", filterContext.HttpContext.Request.Url.PathAndQuery } });

            }
            else if (timeAideWindowContext == null)
            {
                Exception exception = new Exception("Time Aide Window DB is not configured for selected client");
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, exception, this.ControllerContext);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AttendanceSchedule", "All");
                filterContext.Result = PartialView("Error", handleErrorInfo);
            }
        }
        public BaseTAWindowRoleRightsController()
        {   //Set TimeAideWeb EF
            timeAideWebContext = new TimeAideContext();
            //Set TineAideWindow EF
            timeAideWindowContext = DataHelper.GetSelectedClientTAWinEFContext();
            if (timeAideWindowContext == null)
            {
                //throw new Exception("Time Aide Window DB is not configured for selected client.");
              
            }

            FormName = typeof(T).Name;
            var form = timeAideWebContext.Form.FirstOrDefault(p => p.FormName == FormName && p.DataEntryStatus == 1);
            if (form == null)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, IsFormDeleted = true };
            if (SecurityHelper.IsSuperAdmin || SecurityHelper.IsAdmin)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = true, AllowDelete = true, AllowEdit = true, AllowView = true, AllowChangeHistory = true };
            else
            {
                var userRole = timeAideWebContext.UserInformationRole.FirstOrDefault(p => p.UserInformationId == SessionHelper.LoginId);
                if (userRole == null)
                    privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = false, AllowDelete = false, AllowEdit = false, AllowView = false, AllowChangeHistory = true };
                else
                {
                    var roleFormPrivilege = timeAideWebContext.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.FormName == form.FormName).ToList();
                    if (roleFormPrivilege != null)
                    {
                        privileges = (new RoleFormPrivilegeService()).GetView(roleFormPrivilege, form, userRole.RoleId);
                    }
                    else
                    {
                        throw new AuthorizationException();
                    }
                }
            }
            ViewBag.AllowEdit = privileges.AllowEdit;
            ViewBag.AllowAdd = privileges.AllowAdd;
            ViewBag.AllowView = privileges.AllowView;
            ViewBag.AllowDelete = privileges.AllowDelete;
            ViewBag.FormName = FormName;
            ViewBag.Title = UtilityHelper.Pluralize(FormName);
            ViewBag.Label = form.Label;
            ViewBag.LabelPlural = form.LabelPlural;
            Form = form;
        }
       
        public virtual void AllowView()
        {
            if (!privileges.AllowView)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowDelete()
        {
            if (!privileges.AllowDelete)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowEdit()
        {
            if (!privileges.AllowEdit)
            {
                throw new AuthorizationException();
            }
        }
        public virtual void AllowAdd()
        {
            if (!privileges.AllowAdd)
            {
                throw new AuthorizationException();
            }
        }
        protected string GetModelError(ViewDataDictionary viewData)
        {
            string errorMessage = "";
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    errorMessage += " " + error.ErrorMessage;
                }
            }
            return errorMessage;
        }
       
    }
}