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

namespace TimeAide.Web.Controllers
{
    public abstract class BaseApplicantRoleRightsController<T> : TimeAideWebBaseControllers where T : BaseEntity, new()
    {
        public TimeAideContext db;
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

        public BaseApplicantRoleRightsController()
        {
            db = new TimeAideContext();
            FormName = typeof(T).Name;
            var form = db.Form.FirstOrDefault(p => p.FormName == FormName && p.DataEntryStatus == 1);
            if (form == null)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, IsFormDeleted = true };
            if (SecurityHelper.IsSuperAdmin || SecurityHelper.IsAdmin)
                privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = true, AllowDelete = true, AllowEdit = true, AllowView = true, AllowChangeHistory = true };
            else
            {
                var userRole = db.UserInformationRole.FirstOrDefault(p => p.UserInformationId == SessionHelper.LoginId);
                if (userRole == null)
                    privileges = new RoleFormPrivilegeViewModel1() { Form = form, FormId = form.Id, RoleId = 0, AllowAdd = false, AllowDelete = false, AllowEdit = false, AllowView = false, AllowChangeHistory = true };
                else
                {
                    var roleFormPrivilege = db.RoleFormPrivilege.Where(p => p.RoleId == userRole.RoleId && p.Form.FormName == form.FormName).ToList();
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
        public virtual bool IsApplicantHired(int applicantInformationId)
        {
            var isHired = db.ApplicantInformation.Where(w => w.Id == applicantInformationId).Select(w => (w.UserInformationId != null)).FirstOrDefault();
            return isHired;
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
        protected ActionResult GetErrors()
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            Dictionary<String, String> errors = new Dictionary<string, string>();
            foreach (var eachState in ModelState)
            {
                if (eachState.Value != null && eachState.Value.Errors != null && eachState.Value.Errors.Count > 0)
                {
                    errors.Add(eachState.Key, eachState.Value.Errors[0].ErrorMessage);
                }
            }
            var entries = string.Join(",", errors.Select(x => "{" + string.Format("\"Key\":\"{0}\",\"Message\":\"{1}\"", x.Key, x.Value) + "}"));
            var jsonResult = Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.ContentType = "application/json";
            jsonResult.ContentEncoding = System.Text.Encoding.UTF8;   //charset=utf-8
            string json = JsonConvert.SerializeObject(jsonResult);
            return jsonResult;
        }
    }
}