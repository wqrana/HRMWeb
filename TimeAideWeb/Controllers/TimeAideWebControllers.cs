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
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Web.Mvc.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace TimeAide.Web.Controllers
{
    public class AuthorizationException : ApplicationException
    {
        public string ErrorMessage
        {
            get { return "You are not authorize to access the requested information. Please contact system admin"; }
        }
    }
    public abstract class TimeAideWebBaseControllers : Controller
    {
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var requestURL=filterContext.HttpContext.Request.Url;
            if (Request.IsAuthenticated)
            {
                var controllerName = filterContext.Controller.GetType().Name;
                var actionName = filterContext.ActionDescriptor.ActionName;

                bool logEvent = false;
                if (filterContext.HttpContext.Request.HttpMethod == "POST")
                {
                    logEvent = LogSaveEvent(actionName);
                    actionName += ("(Save)");
                }
                else
                {
                    logEvent = LogOpenEvent(actionName);
                }
                if (logEvent)
                    UtilityHelper.UserSessionLogDetail(controllerName, actionName);
            }
            else
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "controller", "Account" }, { "action", "Login" }, { "ReturnUrl", filterContext.HttpContext.Request.Url.PathAndQuery } });

        }


        
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            base.OnAuthentication(filterContext);

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = userManager.FindByName(HttpContext.User.Identity.Name);
            var userid =  filterContext.HttpContext.User.Identity.GetUserId();
           
            var request = filterContext.HttpContext.Request;


        }
        

        private static bool LogSaveEvent(string actionName)
        {
            var logSetting = UserSessionLogEventService.CurrentCompanyUserSessionLogEvent;
            return (logSetting.LogAddSave && actionName.ToLower().Contains("create")) ||
                   (logSetting.LogEditSave && (actionName.ToLower().Contains("save") || actionName.ToLower().Contains("edit"))) ||
                   (logSetting.LogDeleteSave && (actionName.ToLower().Contains("delete") || actionName.ToLower().Contains("delete")));
        }
        private static bool LogOpenEvent(string actionName)
        {
            var logSetting = UserSessionLogEventService.CurrentCompanyUserSessionLogEvent;
            return (logSetting.LogAddOpen && actionName.ToLower().Contains("create")) ||
                   (logSetting.LogEditOpen && (actionName.ToLower().Contains("save") || actionName.ToLower().Contains("edit"))) ||
                   (logSetting.LogDeleteOpen && (actionName.ToLower().Contains("delete") || actionName.ToLower().Contains("delete"))) ||
                   (logSetting.LogListing && actionName.ToLower().Contains("index")) ||
                   (logSetting.LogView && actionName.ToLower().Contains("details"));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            //Log the error!!
            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, filterContext.Exception, this.ControllerContext);
            //Redirect to action
            //filterContext.Result = RedirectToAction("Error", "InternalError");

            // OR return specific view
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/_InternalError.cshtml"
            };
        }
    }
    public abstract class TimeAideWebSecurotyBaseControllers : TimeAideWebBaseControllers
    {
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
        public TimeAideContext db = new TimeAideContext();
        protected RoleFormPrivilegeViewModel1 privileges;
        public TimeAideWebSecurotyBaseControllers(string formName)
        {
            FormName = formName;
            privileges = RoleFormPrivilegeService.GetFormPrivileges(formName);
            ViewBag.AllowEdit = privileges.AllowEdit;
            ViewBag.AllowAdd = privileges.AllowAdd;
            ViewBag.AllowView = privileges.AllowView;
            ViewBag.AllowDelete = privileges.AllowDelete;
            ViewBag.AllowChangeHistory = privileges.AllowChangeHistory;
            ViewBag.FormName = FormName;
            ViewBag.Title = UtilityHelper.Pluralize(FormName);
            if (privileges.IsFormDeleted)
            {
                ViewBag.Label = formName;
                ViewBag.LabelPlural = formName;
            }
            else
            {
                ViewBag.Label = privileges.Form.Label;
                ViewBag.LabelPlural = privileges.Form.LabelPlural;
            }
            Form = privileges.Form;
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
        public virtual void AllowView()
        {
            if (!privileges.AllowView)
            {
                throw new AuthorizationException();
            }
        }
        public void AllowChangeHistory()
        {
            if (!privileges.AllowChangeHistory)
            {
                throw new AuthorizationException();
            }
        }
    }
    public abstract class TimeAideWebControllers<T> : TimeAideWebSecurotyBaseControllers where T : BaseEntity, new()
    {
        public TimeAideWebControllers() : base(typeof(T).Name)
        {
            if (typeof(T).IsSubclassOf(typeof(BaseGlobalEntity)))
            {
                //ViewBag.AllowEdit = false;
                ViewBag.AllowAdd = false;
                ViewBag.AllowDelete = false;
            }
            ViewBag.IsBaseCompanyObject = typeof(T).IsSubclassOf(typeof(BaseCompanyObjects));
            ViewBag.CanBeAssignedToCurrentCompany = false;
            ViewBag.IsAllCompanies = false;
        }
        public virtual List<T> OnIndex(List<T> model)
        {
            return model;
        }
        public virtual ActionResult Index()
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "Index");
                AllowView();
                if (typeof(T).IsSubclassOf(typeof(BaseCompanyObjects)))
                    return RedirectToAction("IndexByCompany");
                List<T> entitySet;
                //if (typeof(T) == typeof(TransactionConfiguration))
                //    entitySet = db.TransactionConfiguration.Where(c=>c.ClientId == SessionHelper.SelectedClientId).ToList();
                //else
                entitySet = db.GetAll<T>(SessionHelper.SelectedClientId);

                entitySet = OnIndex(entitySet);
                return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).ToList());
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(T).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult ChangeHistory(int refrenceId)
        {
            try
            {
                AllowChangeHistory();
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId).ToList();
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = FormName;
                    ViewBag.RefrenceId = refrenceId;
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                return PartialView("~/Views/AuditLog/ChangeHistory.cshtml", entitySet);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(T).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult DownloadChangeHistoryExcel(int refrenceId)
        {

            try
            {
                AllowChangeHistory();
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId).ToList();
                if (entitySet.Count > 0)
                {
                    //string tablename = entitySet.FirstOrDefault().AuditLog.TableName;
                    //int referenceId = entitySet.FirstOrDefault().AuditLog.ReferenceId;
                    //var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + tablename + " where " + tablename + "Id = " + referenceId.ToString() + "").FirstOrDefault();
                    //if (refrenceObject != null)
                    //{
                    //    ViewBag.ReferenceObject = refrenceObject;
                    //    ViewBag.TableName = tablename;
                    //    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                    //}
                }
                if (FormName == "EmployeeDocument")
                {
                    //var workflow = db.WorkflowTriggerRequest.FirstOrDefault(d => d.SelfServiceEmployeeDocument.EmployeeDocumentId == refrenceId && 
                    //                                                             d.WorkflowTrigger.Workflow.WorkflowLevel.Count==d.WorkflowTriggerRequestDetail.Count && 
                    //                                                             (d.WorkflowTrigger.Workflow.WorkflowLevel.Count==0 || d.WorkflowTriggerRequestDetail.First().WorkflowActionTypeId==2));
                    //if (workflow.WorkflowTriggerRequestDetail.Count > 0)
                    //{
                    //    var abc = workflow.WorkflowTriggerRequestDetail.OrderByDescending(d => d.Id).FirstOrDefault();
                    //}
                }
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = FormName;
                    ViewBag.RefrenceId = refrenceId;
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                //return PartialView("~/Views/AuditLog/ChangeHistory.cshtml", entitySet);

                var gv = new GridView();
                gv.DataSource = entitySet;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();


                //using (XLWorkbook wb = new XLWorkbook())
                //{
                //    var ws = wb.Worksheets.Add(, "History");

                //    SessionHelper.CurretnHttpContext.Response.Clear();
                //    SessionHelper.CurretnHttpContext.Response.Buffer = true;
                //    SessionHelper.CurretnHttpContext.Response.Charset = "";
                //    SessionHelper.CurretnHttpContext.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    SessionHelper.CurretnHttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + FormName+"("+refrenceId.ToString()+")" + ".xlsx");
                //    using (MemoryStream MyMemoryStream = new MemoryStream())
                //    {
                //        wb.SaveAs(MyMemoryStream);
                //        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                //        HttpContext.Current.Response.Flush();
                //        HttpContext.Current.Response.End();
                //    }
                //}

                return Json("Index");
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(T).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual List<T> OnIndexByUser(List<T> model, int userId)
        {
            return model;
        }
        public virtual void OnAllowView(T entity)
        {
        }
        public ActionResult IndexByUser(int? id)
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByUser");
                AllowView();
                ViewBag.UserInformationId = id;
                var model = db.GetAllByUser<T>(id, SessionHelper.SelectedClientId).OrderByDescending(e => e.CreatedDate).ToList();
                model = OnIndexByUser(model, id ?? 0);
                return PartialView("Index", model);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(T).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult IndexByCompany(int? id)
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "IndexByCompany");
                AllowView();
                if ((id ?? 0) == 0)
                {
                    id = SessionHelper.SelectedCompanyId;
                }
                var model = db.GetAllByCompany<T>(id, SessionHelper.SelectedClientId).OrderByDescending(e => e.CreatedDate).ToList();
                model = OnIndex(model);
                return PartialView("Index", model);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(T).Name, "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual T OnMostRecentRecord(List<T> entityList, int id)
        {
            return entityList.FirstOrDefault();
        }
        public virtual ActionResult MostRecentRecord(int? id)
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "MostRecentRecord");
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                var entityList = db.GetAllByUser<T>(id, SessionHelper.SelectedClientId).OrderByDescending(u => u.CreatedDate).ToList();
                T model = OnMostRecentRecord(entityList, id ?? 0);
                ViewBag.UserInformationId = id;
                ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, "MostRecentRecord", null);
                if (result.View != null)
                {
                    ViewBag.Label = ViewBag.Label + " - Most Recent Record";
                    return PartialView(model);
                }

                if (model == null)
                {
                    model = new T();
                }
                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView("Details", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Action: MostRecentRecord");
                Debug.WriteLine(ex.InnerException);
                return View("~/Views/Shared/Unauthorized.cshtml");
            }
        }
        public ActionResult AddByUser(int? id)
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "AddByUser");
                AllowAdd();
                T model = new T();
                ViewBag.Label = ViewBag.Label + " - Add";
                (model as BaseUserObjects).UserInformationId = id;
                ViewBag.UserInformationId = id;
                OnCreate();
                OnCreate(model);
                return PartialView("Create", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult CreateEditPop()
        {
            try
            {
                AllowAdd();
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual void OnDetails(T entity)
        {
        }
        public virtual ActionResult Details(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                T model = db.Find<T>(id.Value, SessionHelper.SelectedClientId);
                if (model == null)
                {
                    return PartialView();
                }
                if (model is BaseUserObjects)
                    ViewBag.UserInformationId = (model as BaseUserObjects).UserInformationId;

                OnDetails(model);

                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult CreatePopup()
        {

            try
            {
                AllowAdd();
                ViewBag.Label = ViewBag.Label + " - Add";
                return PartialView("Create");
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult CreateAdhocMasterData()
        {
            try
            {
                AllowAdd();
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual void OnCreate()
        {
        }
        public virtual void OnCreate(T model)
        {
        }
        public virtual ActionResult Create()
        {
            try
            {
                //UtilityHelper.UserSessionLogDetail(FormName, "Create");
                AllowAdd();
                ViewBag.Label = ViewBag.Label + " - Add";
                ViewBag.IsBaseCompanyObject = typeof(T).IsSubclassOf(typeof(BaseCompanyObjects));
                ViewBag.IsAllCompanies = true;
                ViewBag.CanBeAssignedToCurrentCompany = true;
                OnCreate();
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return PartialView("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        //Override in child class for special handling on edit
        public virtual T OnEdit(T entity)
        {
            return entity;
        }
        public virtual T OnEdit(T entity, int id)
        {
            return entity;
        }
        public virtual ActionResult Edit(int? id)
        {
            try
            {
                AllowEdit();
                if (id == null)
                {
                    return PartialView();
                }
                T entity = db.Find<T>(id.Value, SessionHelper.SelectedClientId);
                if (id == 0)
                    entity = OnEdit(entity, id ?? 0);
                else
                    entity = OnEdit(entity);
                if (entity == null)
                {
                    return PartialView();
                }
                //Override in child class for special handling on edit

                ViewBag.Label = ViewBag.Label + " - Edit";
                ViewBag.IsBaseCompanyObject = false;
                ViewBag.IsAllCompanies = false;
                ViewBag.CanBeAssignedToCurrentCompany = false;
                if (typeof(T).IsSubclassOf(typeof(BaseCompanyObjects)))
                {
                    ViewBag.IsBaseCompanyObject = true;
                    if (!(entity as BaseCompanyObjects).CompanyId.HasValue)
                    {
                        ViewBag.IsAllCompanies = true;
                    }
                    var companies = (entity as BaseCompanyObjects).GetRefferredCompanies();
                    if (companies != null)
                    {
                        if (companies.Count == 0 || (companies.Count == 1 && companies.FirstOrDefault() == SessionHelper.SelectedCompanyId))
                        {
                            ViewBag.CanBeAssignedToCurrentCompany = true;
                        }
                    }

                }
                return PartialView(entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual void OnClose(T entity)
        {
        }
        public virtual ActionResult Close(int? id)
        {
            try
            {
                AllowEdit();
                if (id == null)
                {
                    return PartialView();
                }
                T entity = db.Find<T>(id.Value, SessionHelper.SelectedClientId);
                if (entity == null)
                {
                    return PartialView();
                }
                OnClose(entity);
                ViewBag.Label = ViewBag.Label + " - Close";
                return PartialView(entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult Delete(int? id)
        {
            try
            {
                AllowDelete();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                dynamic entity = null;
                if (typeof(Client) == typeof(T))
                {
                    entity = db.Client.Find(id.Value);
                }
                else
                {
                    entity = db.Find<T>(id.Value, SessionHelper.SelectedClientId);
                }
                if (entity == null)
                {
                    return HttpNotFound();
                }
                return PartialView((T)entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }

        public virtual bool CheckBeforeDelete(int id)
        {
            return true;
        }

        [HttpPost]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                AllowDelete();
                dynamic entity = null;
                if (typeof(Client) == typeof(T))
                {
                    entity = db.Client.Find(id);
                }
                else
                {
                    entity = db.Find<T>(id, SessionHelper.SelectedClientId);
                }

                if (!CheckBeforeDelete(id))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var jsonResult = Json(new { success = false, errors = "Record cannot be deleted, it has related  records in database." }, JsonRequestBehavior.AllowGet);
                    string json = JsonConvert.SerializeObject(jsonResult);
                    return Json(json);
                }
                //T city = db.Find<T>(id);

                entity.ModifiedBy = SessionHelper.LoginId;
                entity.ModifiedDate = DateTime.Now;
                entity.DataEntryStatus = 0;
                OnDelete(entity);
                db.SaveChanges();
                if (entity is BaseCompanyObjects)
                    return RedirectToAction("IndexByCompany");
                else if (entity is BaseUserObjects)
                    return RedirectToAction("IndexByUser", new { id = (entity as BaseUserObjects).UserInformationId });
                else if (entity is City)
                    return RedirectToAction("IndexByState");
                else if (entity is State)
                    return RedirectToAction("IndexByCountry");
                else
                    return RedirectToAction("Index");
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual T OnDelete(T entity)
        {
            return entity;
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