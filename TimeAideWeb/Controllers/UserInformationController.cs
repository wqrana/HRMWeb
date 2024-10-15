using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using TimeAide.Models.ViewModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using TimeAide.Web.Extensions;
using TimeAide.Data;
using TimeAide.Services;
using DocumentFormat.OpenXml.Office2010.Excel;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Microsoft.Office.Interop.Excel;

namespace TimeAide.Web.Controllers
{
    public class UserInformationController : TimeAideWebBaseControllers
    {
        private TimeAideContext db = new TimeAideContext();
        string FormName
        {
            get;
            set;
        }
        RoleFormPrivilegeViewModel1 privileges;
        public UserInformationController()
        {
            FormName = typeof(UserInformation).Name;
            var form = db.Form.FirstOrDefault(p => p.FormName == FormName);
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
                    privileges = (new RoleFormPrivilegeService()).GetView(roleFormPrivilege, form, userRole.RoleId);
                }
            }
            ViewBag.AllowEdit = privileges.AllowEdit;
            ViewBag.AllowAdd = privileges.AllowAdd;
            ViewBag.AllowView = privileges.AllowView;
            ViewBag.AllowDelete = privileges.AllowDelete;
            ViewBag.AllowChangeHistory = privileges.AllowChangeHistory;
            ViewBag.FormName = FormName;
            ViewBag.Title = UtilityHelper.Pluralize(FormName);
            ViewBag.Label = form.Label;
            ViewBag.LabelPlural = form.LabelPlural;
        }

        [HttpGet]
        public ActionResult GlobelEmployeeView()
        {
            var redirectTo = privileges.AllowView ? "" : "CompanyPortal";

         
            var supervisorId = SessionHelper.LoginId;


           

            if (string.IsNullOrEmpty(redirectTo))
            {
                ViewBag.Label = "All Employees";
                 ViewBag.CompanyId = new SelectList( CompanyService.GetCompany(SessionHelper.SelectedClientId), "Id", "CompanyName", null).OrderBy(o => o.Text);
                //ViewBag.DepartmentId = CompanyService.GetCompany(SessionHelper.SelectedClientId).OrderBy(o => o.CompanyName);
                return View("_GlobalEmployeeListView");
            }
            else
            {
                return Json($"Success,{redirectTo}", JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public JsonResult GetGlobelEmployeeList(EmployeeUserInformationView model)
        {

            List<EmployeeUserInformationView> empuserview = new List<EmployeeUserInformationView>();
            try
            {

                var employeeStatusId = model.EmployeeStatusId;
                

                var supervisorId = SessionHelper.LoginId;


                empuserview = db.SP_EmployeeUserInformationByFilter<EmployeeUserInformationView>( supervisorId, SessionHelper.SelectedClientId, employeeStatusId,model.CompanyId);


                var obj = Json(  new {data= empuserview }, JsonRequestBehavior.AllowGet);

                obj.MaxJsonLength = int.MaxValue;

                return obj;
            }


            catch (Exception ex) {

                return Json(new { data = empuserview }, JsonRequestBehavior.AllowGet);
            }
            
            

        }



        // GET: UserInformation
        public ActionResult Index(int? id)
        {
            if (!privileges.AllowView)
            {
                Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
            var isApplyActiveEmployeeFilter = false;
            var sessionFilters = SessionHelper.UserFilterInSession;
            ViewBag.CompanyID = new SelectList(db.Company.Where(w => w.DataEntryStatus == 1
                                              && w.ClientId == SessionHelper.SelectedClientId
                                              && w.Id == SessionHelper.SelectedCompanyId), "Id", "CompanyName", SessionHelper.SelectedCompanyId);

            var configuration = ApplicationConfigurationService.GetApplicationConfiguration("EnableActiveEmployeeFilter");
            if (configuration != null )
                isApplyActiveEmployeeFilter = configuration.ValueAsBoolean;

            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName", sessionFilters.DepartmentId).OrderBy(o=>o.Text);
            ViewBag.SubDepartmentId = new SelectList(SubDepartmentService.SubDepartments(sessionFilters.DepartmentId), "Id", "SubDepartmentName", sessionFilters.SubDepartmentId).OrderBy(o => o.Text);
            ViewBag.EmployeeTypeId = new SelectList(db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeName", sessionFilters.EmployeeTypeId).OrderBy(o => o.Text); 
            ViewBag.EmploymentTypeId = new SelectList(db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmploymentTypeName", sessionFilters.EmploymentTypeId).OrderBy(o => o.Text); 
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName", sessionFilters.PositionId).OrderBy(o => o.Text);
            var employeeStatusId = sessionFilters.EmployeeStatusId;
            if (isApplyActiveEmployeeFilter == true && sessionFilters.IsFilterApllied == false)
                employeeStatusId = 1;

            ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus.Where(w => w.DataEntryStatus == 1), "Id", "EmployeeStatusName", employeeStatusId).OrderBy(o => o.Text);
            ViewBag.SupervisorId = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.CompanyId == SessionHelper.SelectedCompanyId && w.Id != SessionHelper.LoginId), "Id", "ShortFullName", sessionFilters.SupervisorId).OrderBy(o => o.Text);
            ViewBag.IsFilterApplied = sessionFilters.IsFilterApllied;

            SessionHelper.SelectedUserInformationId = 0;
            // var userList = getUserList();
            //List View

            //if (id.HasValue)
            //{
            //    return View("IndexByGridView", userList);
            //}

            //return View(userList);
            return View();
        }
        public ActionResult IndexByViewType(UserFilterViewModel userFilter)
        {
            string returnViewName = "IndexByListView";
            //var userList = getUserList();
            //var userList = getViewBasedUserList(true, userFilter);

            //userList = ApplyUserFilterList(userList, userFilter);
            if (userFilter.ViewTypeId == 1)
            {
                var userList = getViewBasedUserSessionFilterList(true, userFilter);
                returnViewName = "IndexByGridView";
                userList = ApplyUserGridViewPaging(userList, userFilter);
                return PartialView(returnViewName, userList);
            }
            else
            {
                return PartialView(returnViewName);
            }


        }



        [HttpGet]
        public JsonResult GetEmployeeGridList(UserFilterViewModel userFilter)
        {


            var userList = getViewBasedUserSessionFilterList(true, userFilter);

            try
            {
               

                userList = ApplyUserFilterList(userList, userFilter);

                var obj = Json(new { data = userList }, JsonRequestBehavior.AllowGet);

                obj.MaxJsonLength = int.MaxValue;

                return obj;
            }


            catch (Exception ex)
            {

                return Json(new { data = userList }, JsonRequestBehavior.AllowGet);
            }



        }


        [HttpGet]
        public JsonResult FilterAppliedCheck()
        {


            var filterStatus = SessionHelper.UserFilterInSession.IsFilterApllied;


            try
            {
                
              

                var obj = Json(filterStatus, JsonRequestBehavior.AllowGet);

               

                return obj;
            }


            catch (Exception ex)
            {

                return Json(filterStatus, JsonRequestBehavior.AllowGet);
            }



        }



        


        public ActionResult IndexBulkUserRegistration()
        {
            ViewBag.EmployeeGroupId = new SelectList(db.GetAllByCompany<EmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeGroupName");
            return View();
        }

        public ActionResult IndexEmailBlast()
        {
            var emailBlast = db.GetAllByCompany<EmailBlast>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);   
            ViewBag.EmployeeGroupId = new SelectList(db.GetAllByCompany<EmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeGroupName");
            return View(emailBlast);
        }
        public ActionResult UserRegistrationReport()
        {
            var entitySetRegisterationEmail = db.AuditLogDetail.Where(d => d.AuditLog.TableName.Contains(""))
                                  .Select(d => d.AuditLog).ToList();

            var registrationEmail = db.AuditLogDetail
                             .Where(p => p.AuditLog.TableName == "Account Registration" && p.ColumnName == "Registration Email" && p.ClientId == SessionHelper.SelectedClientId && p.CompanyId == SessionHelper.SelectedCompanyId)
                             .GroupBy(x => x.AuditLog.ReferenceId)
                             .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault()).ToList();

            var manualUserRegistration = db.AuditLogDetail
                             .Where(p => p.AuditLog.TableName == "Account Registration" && p.ColumnName == "Manual User Registration" && p.ClientId == SessionHelper.SelectedClientId && p.CompanyId == SessionHelper.SelectedCompanyId)
                             .GroupBy(x => x.AuditLog.ReferenceId)
                             .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault()).Select(p => p.AuditLog.ReferenceId).ToList();
            
            var completedIds = registrationEmail.Where(p => p.AuditLog.ActionType == "Registration Completed"|| p.AuditLog.ActionType == "User SignedIn").Select(p => p.AuditLog.ReferenceId);
            var pendingIds = registrationEmail.Where(p => !completedIds.Contains(p.AuditLog.ReferenceId)).Select(p => p.AuditLog.ReferenceId);
            // var pendingIds = registrationEmail.Where(p => p.AuditLog.ActionType != "Registration Completed").Select(p => p.AuditLog.ReferenceId);

            var othersRegistration = db.UserInformation.Include("UserEmployeeGroup").Where(e => !completedIds.Contains(e.Id) && !pendingIds.Contains(e.Id) && !manualUserRegistration.Contains(e.Id) && e.UserContactInformations.Count > 0 & !String.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().LoginEmail) && e.CompanyId == SessionHelper.SelectedCompanyId).ToList();
            var registrationCompleted = db.UserInformation.Include("UserEmployeeGroup").Where(e => e.UserContactInformations.Count > 0 && !String.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().LoginEmail) && completedIds.Contains(e.Id)).ToList();
            var pendings = db.UserInformation.Include("UserEmployeeGroup").Where(e => e.UserContactInformations.Count > 0 && !String.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().LoginEmail) && pendingIds.Contains(e.Id)).ToList();
            var registrations = db.UserInformation.Include("UserEmployeeGroup").Where(e => e.UserContactInformations.Count > 0 && !String.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().LoginEmail) &&  manualUserRegistration.Contains(e.Id)).ToList();
            var unRegistrations = db.UserInformation.Include("UserEmployeeGroup").Where(e => (e.UserContactInformations.Count== 0 || String.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().LoginEmail))
                                                                                          && (((e.Employment.Count==0 || e.EmploymentHistory.Count == 0) && e.CompanyId == SessionHelper.SelectedCompanyId) || (e.EmploymentHistory.Count>0 && e.EmploymentHistory.OrderByDescending(h => h.Id).FirstOrDefault().CompanyId == SessionHelper.SelectedCompanyId))
                                                          ).ToList();

            ViewBag.Pending = pendings;
            ViewBag.RegistrationCompleted = registrationCompleted;
            ViewBag.Registrations = registrations.Union(othersRegistration).ToList();
            ViewBag.UnregisteredWithoutEmail = unRegistrations.Where(e => !pendingIds.Contains(e.Id)).Where(e=>e.UserContactInformations.Count== 0 || (string.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().WorkEmail) && string.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().PersonalEmail) && string.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().OtherEmail))).ToList();
            ViewBag.UnregisteredWithEmail = unRegistrations.Where(e => !pendingIds.Contains(e.Id)).Where(e => e.UserContactInformations.Count > 0 &&  (!string.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().WorkEmail) || !string.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().PersonalEmail) || !string.IsNullOrEmpty(e.UserContactInformations.FirstOrDefault().OtherEmail))).ToList();
            return View(new List<UserInformation>());
        }
        [HttpPost]
        public ActionResult SendBulkRegistrationEmail(List<EmployeePrivilegeViewModel> dataArray)
        {
            try
            {
                List<UserInformationViewModel> sentItems = new List<UserInformationViewModel>();
                List<UserInformationViewModel> notSentItems = new List<UserInformationViewModel>();
                if (dataArray != null)
                {
                    //var notSent = dataArray.Where(e => String.IsNullOrEmpty(e.LoginEmail) || e.LoginEmail == "- Please select -").ToList();
                    foreach (var each in dataArray)
                    {

                        var userContactInformation = db.UserContactInformation.Include(u => u.UserInformation).Include(u => u.UserInformation.UserInformationRole).FirstOrDefault(c => c.UserInformationId == each.UserInformationId);
                        if (userContactInformation.UserInformation != null)
                        {
                            if (String.IsNullOrEmpty(each.LoginEmail) || each.LoginEmail == "- Please select -")
                            {
                                notSentItems.Add(GetUserInformationViewModel(each, userContactInformation.UserInformation));
                                continue;
                            }
                            if (!userContactInformation.UserInformation.UserInformationRole.Any())
                            {
                                foreach (var eachRole in userContactInformation.UserInformation.UserInformationRole.ToArray())
                                    db.Entry(eachRole).State = EntityState.Deleted;
                                userContactInformation.UserInformation.UserInformationRole.Clear();
                                userContactInformation.UserInformation.UserInformationRole.Add(new UserInformationRole { UserInformationId = userContactInformation.UserInformationId, RoleId = each.RoleId });
                            }

                            UpdateUserEmployeeGroups(each, true, db);
                            //if (String.IsNullOrEmpty(each.LoginEmail))
                            //{
                            //    each.LoginEmail = userContactInformation.WorkEmail ?? userContactInformation.PersonalEmail;
                            //    each.LoginEmail = each.LoginEmail ?? userContactInformation.OtherEmail;
                            //}
                            //if (!String.IsNullOrEmpty(each.LoginEmail))
                            //{
                            userContactInformation.LoginEmail = each.LoginEmail;
                            ResendRegistrationEmail(each, userContactInformation, db, false);

                            sentItems.Add(GetUserInformationViewModel(each, userContactInformation.UserInformation));
                            //}
                        }
                    }
                    //return Json(new { Sent = sentItems, NotSent = notSentItems });

                }
                ViewBag.Sent = sentItems;
                ViewBag.NotSent = notSentItems;
                ViewDataDictionary viewDataDictionary = new ViewDataDictionary();
                viewDataDictionary.Add("Sent", sentItems);
                viewDataDictionary.Add("NotSent", notSentItems);
                return PartialView("IndexBulkUserRegistrationReport", viewDataDictionary);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                return Json("Error");
            }
        }
        private UserInformationViewModel GetUserInformationViewModel(EmployeePrivilegeViewModel row, UserInformation user)
        {

            var activeEmployment = TimeAide.Services.EmploymentService.GetActiveEmployment(row.UserInformationId);
            EmploymentHistory activeEmploymentHistory = null;
            if (activeEmployment != null)
            {
                activeEmploymentHistory = TimeAide.Services.EmploymentHistoryService.GetActiveEmploymentHistory(row.UserInformationId, activeEmployment.Id);
            }
            var employee = new UserInformationViewModel()
            {
                Id = row.UserInformationId,
                FirstLastName = user.FirstLastName,
                FirstName = user.FirstName,
                SecondLastName = user.SecondLastName,
                ShortFullName = user.ShortFullName,
                EmployeeId = user.EmployeeId,

                UserRole = TimeAide.Web.Extensions.UserManagmentService.GetUserRole(row.UserInformationId),
                EmployeeGroups = TimeAide.Web.Extensions.UserManagmentService.GetUserEmployeeGroups(row.UserInformationId)
            };
            if (user != null && user.UserContactInformations != null && user.UserContactInformations.Count > 0)
            {
                employee.LoginEmail = user.UserContactInformations.FirstOrDefault().LoginEmail;
            }
            if (activeEmploymentHistory != null)
            {
                if (activeEmploymentHistory.Department != null)
                    employee.DepartmentName = activeEmploymentHistory.Department.DepartmentName;
                if (activeEmploymentHistory.SubDepartment != null)
                    employee.EmployeeTypeName = activeEmploymentHistory.SubDepartment.SubDepartmentName;
                if (activeEmploymentHistory.Position != null)
                    employee.PositionName = activeEmploymentHistory.Position.PositionName;
            }
            return employee;
        }
        private List<UserInformationViewModel> ApplyUserFilterList(List<UserInformationViewModel> userList, UserFilterViewModel userFilter)
        {
            //if (userFilter.EmployeeId != null)
            //{
            //    userList = userList.Where(w => w.EmployeeId == userFilter.EmployeeId).ToList();
            //}
            //if(userFilter.EmployeeName != null)
            //{
            //    userList = userList.Where(w => w.ShortFullName.ToLower().Contains(userFilter.EmployeeName.ToLower())).ToList();
            //}
            //if(userFilter.PositionId != null)
            //{
            //    userList = userList.Where(w => w.PositionId == userFilter.PositionId).ToList();
            //}
            //if (userFilter.EmployeeStatusId != null)
            //{
            //    userList = userList.Where(w => w.EmployeeStatusId == userFilter.EmployeeStatusId).ToList();
            //}
            if (userFilter.ViewTypeId == 1 && userFilter.SearchText != null)
            {
                var searchTxt = userFilter.SearchText.ToLower();
                userList = userList.Where(w => w.ShortFullName.ToLower().Contains(searchTxt.ToLower())
                                           || w.EmployeeStatusName.ToLower().Contains(searchTxt.ToLower())
                                           ).ToList();

            }

            return userList;
        }
        private List<UserInformationViewModel> ApplyUserGridViewPaging(List<UserInformationViewModel> userList, UserFilterViewModel userFilter)
        {
            int totalRecords, pageRecordFrom, pageRecordTo, totalPages = 0;
            int pageSize = userFilter.PageSize ?? 0;
            int pageNo = userFilter.PageNo ?? 0;
            int skipRecords = (pageNo - 1) * pageSize;
            pageRecordFrom = skipRecords + 1;
            totalRecords = userList.Count();
            double tempTotalRec = totalRecords * 1.0;
            totalPages = (int)Math.Ceiling((tempTotalRec / pageSize));
            userList = userList.Skip(skipRecords).Take(pageSize).ToList<UserInformationViewModel>();
            pageRecordTo = skipRecords + userList.Count();
            pageRecordFrom = pageRecordTo > 0 ? pageRecordFrom : 0;
            ViewBag.GridPagingObject = new PagingViewModel
            {
                TotolRecords = totalRecords,
                TotalPages = totalPages,
                StartRecord = pageRecordFrom,
                EndRecord = pageRecordTo,
                CurrentPage = pageNo
            };

            return userList;
        }
        private List<UserInformationViewModel> getViewBasedUserSessionFilterList(bool? isLoadingRequired, UserFilterViewModel userFilter)
        {
            List<UserInformationViewModel> newList;
            var setSessionFilters = new UserFilterInSession();
            if (SessionHelper.SelectedClientId == 0)
            {
                SessionHelper.SelectedClientId = SessionHelper.ClientId;
            }

            if (SessionHelper.SelectedCompanyId == 0)
            {
                SessionHelper.SelectedCompanyId = SessionHelper.CompanyId;
                var company = (new TimeAideContext()).Find<Company>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                if (company != null)
                    SessionHelper.SelectedCompanyId_Old = company.Old_Id ?? 0;
            }
            ViewBag.CompanyID = new SelectList(db.Company.Where(w => w.DataEntryStatus == 1
                                                && w.ClientId == SessionHelper.SelectedClientId
                                                && w.Id == SessionHelper.SelectedCompanyId), "Id", "CompanyName", SessionHelper.SelectedCompanyId);

            try
            {
                var employeeId = 0;
                var employeeName = "";
                var departmentId = 0;
                var subDepartmentId = 0;
                var positionId = 0;
                var employmentTypeId = 0;
                var employeeTypeId = 0;
                var employeeStatusId = 0;
                var supervisorId = SessionHelper.LoginId;
                if (userFilter != null)
                {

                    employeeId = userFilter.EmployeeId ?? 0;
                    employeeName = userFilter.EmployeeName == null ? employeeName : userFilter.EmployeeName;

                    setSessionFilters.DepartmentId = userFilter.DepartmentId;
                    departmentId = userFilter.DepartmentId ?? 0;

                    setSessionFilters.SubDepartmentId = userFilter.SubDepartmentId;
                    subDepartmentId = userFilter.SubDepartmentId ?? 0;

                    setSessionFilters.PositionId = userFilter.PositionId;
                    positionId = userFilter.PositionId ?? 0;

                    setSessionFilters.EmploymentTypeId = userFilter.EmploymentTypeId;
                    employmentTypeId = userFilter.EmploymentTypeId ?? 0;

                    setSessionFilters.EmployeeTypeId = userFilter.EmployeeTypeId;
                    employeeTypeId = userFilter.EmployeeTypeId ?? 0;

                    setSessionFilters.EmployeeStatusId = userFilter.EmployeeStatusId;
                    employeeStatusId = userFilter.EmployeeStatusId ?? 0;

                    setSessionFilters.SupervisorId = userFilter.SupervisorId;
                    supervisorId = userFilter.SupervisorId ?? SessionHelper.LoginId;
                }
                //Calling Db Procedure SP_UserInformationByFilter
                var userInformationList = db.SP_UserInformationByFilter<UserInformationViewModel>(employeeId, employeeName, departmentId, subDepartmentId, positionId,
                                        employmentTypeId, employeeTypeId, employeeStatusId, supervisorId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);

                SessionHelper.UserFilterInSession = setSessionFilters;

                newList = userInformationList;

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return newList;

        }
        private List<UserInformationViewModel> getViewBasedUserList(bool? isLoadingRequired, UserFilterViewModel userFilter)
        {
            List<UserInformationViewModel> newList;
            // var setSessionFilters = new UserFilterInSession();
            if (SessionHelper.SelectedClientId == 0)
            {
                SessionHelper.SelectedClientId = SessionHelper.ClientId;
            }

            if (SessionHelper.SelectedCompanyId == 0)
            {
                SessionHelper.SelectedCompanyId = SessionHelper.CompanyId;
                var company = (new TimeAideContext()).Find<Company>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                if (company != null)
                    SessionHelper.SelectedCompanyId_Old = company.Old_Id ?? 0;
            }
            ViewBag.CompanyID = new SelectList(db.Company.Where(w => w.DataEntryStatus == 1
                                                && w.ClientId == SessionHelper.SelectedClientId
                                                && w.Id == SessionHelper.SelectedCompanyId), "Id", "CompanyName", SessionHelper.SelectedCompanyId);



            try
            {
                var employeeId = 0;
                var employeeName = "";
                var positionId = 0;
                var employeeStatusId = 0;
                var supervisorId = SessionHelper.LoginId;
                if (userFilter != null)
                {

                    employeeId = userFilter.EmployeeId ?? 0;
                    employeeName = userFilter.EmployeeName == null ? employeeName : userFilter.EmployeeName;

                    positionId = userFilter.PositionId ?? 0;

                }
                //Calling Db Procedure
                var userInformationList = db.SP_UserInformation<UserInformationViewModel>(employeeId, employeeName, positionId,
                                         employeeStatusId, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);

                //if ((isLoadingRequired ?? false) == true)
                //{
                //    foreach (UserInformation user in userInformationList)
                //    {
                //        user.Company = db.Company.Find(user.CompanyId);
                //        user.Department = db.Department.Find(user.DepartmentId);
                //        user.EmploymentStatu = db.EmploymentStatus.Find(user.EmploymentStatusId);
                //        user.SubDepartment = db.SubDepartment.Find(user.SubDepartmentId);
                //        user.EmployeeStatus = db.EmployeeStatus.Find(user.EmployeeStatusId);
                //        user.Position = db.Position.Find(user.PositionId);
                //        user.EmployeeType = db.EmployeeType.Find(user.EmployeeTypeID);

                //    }
                //}
                newList = userInformationList;

            }
            catch (Exception ex)
            {
                throw ex;
            }


            return newList;

        }
        
        private void SeflServiceUser(IQueryable<UserInformation> userInformation)
        {
            userInformation = userInformation.Where(u => u.Id == SessionHelper.LoginId);
        }

        // GET: UserInformation/Details/5
        public ActionResult Details(string id)
        {
            var userId = id == null ? 0 : int.Parse(Encryption.DecryptURLParm(id));
            if (userId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!privileges.AllowView)
            {
                Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Details");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
            // UserInformation userInformation = db.UserInformation.Find(id);
            //UserInformation userInformation = db.UserInformation.Where(w => w.Id == id)
            //                                    .Include(u => u.Ethnicity)
            //                                    .Include(u => u.Disability)
            //                                    .Include(u => u.EmployeeVeteranStatus)
            //                                    .Include(u => u.EmployeeStatus)

            //                                    .FirstOrDefault();
            try
            {
                //Db Procedure Calling
                var userInformation = db.SP_UserInformationById<UserInformation>(userId);
                if (userInformation == null)
                {
                    // return HttpNotFound();
                    throw new Exception("User Not Found:" + id);
                }

                //Load related entities
                userInformation.Company = db.Company.Find(userInformation.CompanyId);
                userInformation.Department = db.Department.Find(userInformation.DepartmentId ?? 0);
                userInformation.EmploymentStatu = db.EmploymentStatus.Find(userInformation.EmploymentStatusId ?? 0);
                userInformation.SubDepartment = db.SubDepartment.Find(userInformation.SubDepartmentId ?? 0);
                userInformation.EmployeeStatus = db.EmployeeStatus.Find(userInformation.EmployeeStatusId);
                userInformation.Position = db.Position.Find(userInformation.PositionId ?? 0);
                userInformation.EmployeeType = db.EmployeeType.Find(userInformation.EmployeeTypeID ?? 0);
                userInformation.Ethnicity = db.Ethnicity.Find(userInformation.EthnicityId ?? 0);
                userInformation.Disability = db.Disability.Find(userInformation.DisabilityId ?? 0);
                userInformation.EmployeeVeteranStatus = db.EmployeeVeteranStatus.Where(w => w.UserInformationId == userId).ToList();
                //end loading entities;

                //userInformation.SelectedVeteranStatus
                var picturePath = userInformation.PictureFilePath;
                //setting the default picture
                userInformation.PictureFilePath = string.IsNullOrEmpty(picturePath) ? "/images/no-profile-image.jpg" : picturePath;
                userInformation.SSNEnd = userInformation.SSN;
                var resumeName = string.IsNullOrEmpty(userInformation.ResumeFilePath) ? "" : userInformation.ResumeFilePath;
                var startingIndex = resumeName.LastIndexOf("\\") + 1;
                if (startingIndex > 0)
                {
                    userInformation.ResumeFilePath = resumeName.Substring(startingIndex, resumeName.Length - startingIndex);
                }
                else
                {
                    userInformation.ResumeFilePath = "";
                }
                ViewBag.GenderId = new SelectList(db.Gender.Where(u => u.DataEntryStatus == 1), "Id", "GenderName");
                ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus.Where(u => u.DataEntryStatus == 1), "Id", "MaritalStatusName");
                //Contact Info dropdown
                //ViewBag.ContactMediumId = new SelectList(db.ContactMedium, "Id", "ContactMediumName");
                //ViewBag.ContactTypeId = new SelectList(db.ContactType, "Id", "ContactTypeName");
                // Emergency relationship
                ViewBag.RelationshipId = new SelectList(db.Relationship.Where(w => w.DataEntryStatus == 1), "Id", "RelationshipName");
                //
                ViewBag.DisabilityId = new SelectList(db.Disability.Where(w => w.DataEntryStatus == 1), "Id", "DisabilityName");
                ViewBag.EthnicityId = new SelectList(db.Ethnicity.Where(w => w.DataEntryStatus == 1), "Id", "EthnicityName");
                ViewBag.VeteranStatusId = new SelectList(db.VeteranStatus.Where(w => w.DataEntryStatus == 1), "Id", "VeteranStatusName");
                return View(userInformation);
            }
            catch (Exception ex)
            {

                //  Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex.Message, this.ControllerContext);
                //  Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                throw ex;
            }
        }

        public ActionResult EmployeeProfile(string id)
        {
            var userId = id == null ? 0 : int.Parse(Encryption.DecryptURLParm(id));
            if (userId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                //db.Configuration.LazyLoadingEnabled = false;

                var userInformation = db.SP_UserInformationById<UserInformation>(userId);
                if (userInformation == null)
                {
                    throw new Exception("User Not Found:" + id.ToString());
                }
                userInformation.UserContactInformations = db.UserContactInformation.Where(u => u.UserInformationId == userId && u.DataEntryStatus == 1).ToList();
                userInformation.Gender = db.Gender.Find(userInformation.GenderId ?? 0);
                userInformation.EmergencyContact = db.EmergencyContact.Where(u => u.UserInformationId == userId && u.DataEntryStatus == 1).ToList();
                userInformation.EmployeeDependent = db.EmployeeDependent.Include(u => u.Relationship).Where(u => u.UserInformationId == userId && u.DataEntryStatus == 1).ToList();
                userInformation.EmployeeEducation = db.EmployeeEducation.Include(u => u.Degree).Where(u => u.UserInformationId == userId && u.DataEntryStatus == 1).OrderByDescending(u => u.DateCompleted).ToList();
                userInformation.Ethnicity = db.Ethnicity.Find(userInformation.EthnicityId ?? 0);
                userInformation.Disability = db.Disability.Find(userInformation.DisabilityId ?? 0);
                userInformation.MaritalStatus = db.MaritalStatus.Find(userInformation.MaritalStatusId ?? 0);
                userInformation.Employment = db.Employment.Where(u => u.UserInformationId == userId && u.DataEntryStatus == 1).OrderByDescending(e => e.EffectiveHireDate).ToList();

                userInformation.EmployeeCredential = db.EmployeeCredential.Include(u => u.Credential).Include(u => u.NotificationLog).Where(c => c.UserInformationId == userId && c.DataEntryStatus == 1).ToList();
                userInformation.EmployeeDocument = db.EmployeeDocument.Include(u => u.NotificationLog).Where(c => c.UserInformationId == userId && c.DataEntryStatus == 1).ToList();
                userInformation.EmployeeTraining = db.EmployeeTraining.Where(c => c.UserInformationId == userId && c.DataEntryStatus == 1).ToList();


                ViewBag.EmployeeUserSupervisors = String.Join(", ", db.EmployeeSupervisor.Where(w => w.DataEntryStatus == 1 && w.EmployeeUserId == userId).Select(c => c.SupervisorUser.ShortFullName.ToString()).Distinct());
                var activeEmployment = TimeAide.Services.EmploymentService.GetActiveEmployment(userId);
                ViewBag.ActiveEmployment = activeEmployment;
                if (activeEmployment != null)
                    ViewBag.ActiveEmploymentHistory = TimeAide.Services.EmploymentHistoryService.GetActiveEmploymentHistory(userId, activeEmployment.Id);
                else
                    ViewBag.ActiveEmploymentHistory = null;

                ViewBag.ActivePayInformationHistory = TimeAide.Services.PayInformationHistoryService.GetActivePayInformationHistory(userId);


                userInformation.Company = db.Company.Find(userInformation.CompanyId);
                userInformation.Department = db.Department.Find(userInformation.DepartmentId ?? 0);
                userInformation.EmploymentStatu = db.EmploymentStatus.Find(userInformation.EmploymentStatusId ?? 0);
                userInformation.SubDepartment = db.SubDepartment.Find(userInformation.SubDepartmentId ?? 0);
                userInformation.EmployeeStatus = db.EmployeeStatus.Find(userInformation.EmployeeStatusId);
                userInformation.Position = db.Position.Find(userInformation.PositionId ?? 0);
                userInformation.EmployeeType = db.EmployeeType.Find(userInformation.EmployeeTypeID ?? 0);

                userInformation.EmployeeVeteranStatus = db.EmployeeVeteranStatus.Where(w => w.UserInformationId == userId).ToList();
                var picturePath = userInformation.PictureFilePath;
                userInformation.PictureFilePath = string.IsNullOrEmpty(picturePath) ? "/images/no-profile-image.jpg" : picturePath;
                userInformation.SSNEnd = userInformation.SSNEndComputed;
                var resumeName = string.IsNullOrEmpty(userInformation.ResumeFilePath) ? "" : userInformation.ResumeFilePath;
                var startingIndex = resumeName.LastIndexOf("/") + 1;
                if (startingIndex > 0)
                {
                    userInformation.ResumeFilePath = resumeName.Substring(startingIndex, resumeName.Length - startingIndex);
                }
                else
                {
                    userInformation.ResumeFilePath = "";
                }

                ViewBag.CanAddressWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(userId, 6);
                ViewBag.CanNumberWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(userId, 3);
                ViewBag.CanEmergencyContactWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(userId, 4);
                ViewBag.CanEmployeeDependantWorkflowIntiated = TimeAide.Services.WorkflowService.CanWorkflowIntiated(userId, 5);

                ViewBag.EmployeeDocumentPrivilege = RoleFormPrivilegeService.GetFormPrivileges("ChangeRequestEmployeeDocument");
                ViewBag.EmployeeCredentialPrivilege = RoleFormPrivilegeService.GetFormPrivileges("ChangeRequestEmployeeCredential");
                ViewBag.AddressPrivilege = RoleFormPrivilegeService.GetFormPrivileges("ChangeRequestAddress");
                ViewBag.EmergencyContactPrivilege = RoleFormPrivilegeService.GetFormPrivileges("ChangeRequestEmergencyContact");
                ViewBag.EmployeeDependentPrivilege = RoleFormPrivilegeService.GetFormPrivileges("ChangeRequestEmployeeDependent");
                ViewBag.EmailsAndNumberPrivilege = RoleFormPrivilegeService.GetFormPrivileges("ChangeRequestEmailsAndNumber");


                //userInformation.ChangeRequestAddress = TimeAide.Services.WorkflowService.GetChangeRequestAddress(id.Value);
                //userInformation.ChangeRequestEmailNumbers = TimeAide.Services.WorkflowService.GetChangeRequestEmailNumbers(id.Value);
                //userInformation.ChangeRequestEmergencyContact = TimeAide.Services.WorkflowService.GetChangeRequestEmergencyContact(id.Value);
                //userInformation.ChangeRequestEmployeeDependent = TimeAide.Services.WorkflowService.GetChangeRequestEmployeeDependent(id.Value);
                //userInformation.SelfServiceEmployeeDocument = TimeAide.Services.WorkflowService.GetSelfServiceEmployeeDocument(id.Value);
                //userInformation.SelfServiceEmployeeCredential = TimeAide.Services.WorkflowService.GetSelfServiceEmployeeCredential(id.Value);
                //userInformation.BirthDate = null;
                //db.Configuration.LazyLoadingEnabled = false;
                return View(userInformation);
            }
            catch (Exception ex)
            {
                db.Configuration.LazyLoadingEnabled = false;
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult AjaxUploadUserCV()
        {
            dynamic retResult = null;
            string status = "Error";
            string message = "No file is selected";
            string cvName = "";
            string serverFilePath = "";
            string relativeFilePath = "";
            string userID = Request.Form["UserID"];

            // serverFilePath = Path.Combine(Server.MapPath("~/Content/doc/resumes"), profilePictureName);
            //retResult = new { status = "Error", message = "No file is selected" };

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase cvFile = Request.Files[0];
                    var user = db.UserInformation.Find(int.Parse(userID));
                    if (user != null)
                    {
                        var fileExt = Path.GetExtension(cvFile.FileName);
                        cvName = "Emp-" + userID + "-CV" + fileExt;


                        FilePathHelper filePathHelper = new FilePathHelper();

                        serverFilePath = filePathHelper.GetPath("resumes", cvName);
                        cvFile.SaveAs(serverFilePath);
                        relativeFilePath = filePathHelper.RelativePath;
                        user.ResumeFilePath = relativeFilePath;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        status = "Success";
                        message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid User record data!";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            retResult = new { status = status, message = message, cvName = cvName };
            return Json(retResult);
        }
        [HttpPost]
        public JsonResult AjaxCheckUserCV(int userID)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";
            string relativeFilePath = "";
            string serverFilePath = "";
            FileInfo downloadCVFile;
            // serverFilePath = Path.Combine(Server.MapPath("~/Content/doc/resumes"), profilePictureName);
            //retResult = new { status = "Error", message = "No file is selected" };

            if (userID > 0)
            {
                try
                {

                    var user = db.UserInformation.Find(userID);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(user.ResumeFilePath))
                        {
                            relativeFilePath = user.ResumeFilePath;
                            var tempPath = "~" + relativeFilePath;
                            serverFilePath = Server.MapPath(tempPath);
                            downloadCVFile = new FileInfo(serverFilePath);
                            if (!downloadCVFile.Exists)
                            {
                                status = "Error";
                                message = "Employee CV is not yet Uploaded!";
                            }
                        }
                        else
                        {
                            status = "Error";
                            message = "Employee CV is not yet Uploaded!";
                        }
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Employee record data!";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            else
            {
                status = "Error";
                message = "Invalid Employee record data!";
            }
            retResult = new { status = status, message = message };
            return Json(retResult);
        }
        public ActionResult DownloadUserCV(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadCVFile = null;
            byte[] fileBytes;
            var user = db.UserInformation.Find(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ResumeFilePath))
                {
                    relativeFilePath = user.ResumeFilePath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadCVFile = new FileInfo(serverFilePath);

                    if (downloadCVFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadCVFile.Name);
                    }
                }

            }

            return null;

        }

        [HttpPost]
        public JsonResult AjaxUploadUserPicture()
        {
            dynamic retResult = null;
            string serverFilePath = "";
            string relativeFilePath = "";
            string userID = Request.Form["UserID"];
            string action = Request.Form["Action"];
            var profilePictureName = userID + '.' + "jpg";


            FilePathHelper filePathHelper = new FilePathHelper();
            serverFilePath = filePathHelper.GetPath("pictures", profilePictureName);
            retResult = new { status = "Error", message = "Invalid Operation", picturePath = "" };
            FileInfo checkFile = null;

            if (action == "U")
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        HttpPostedFileBase pictureFile = Request.Files[0];
                        var user = db.UserInformation.Find(int.Parse(userID));
                        if (user != null)
                        {
                            pictureFile.SaveAs(serverFilePath);
                            relativeFilePath = filePathHelper.RelativePath;
                            user.PictureFilePath = relativeFilePath;
                            db.SaveChanges();
                            retResult = new { status = "Success", message = "Profile picture is successfully Uploaded!", picturePath = relativeFilePath };
                        }
                        else
                        {
                            retResult = new { status = "Error", message = "Invalid record data", picturePath = "" };
                        }

                    }
                    catch (Exception ex)
                    {
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        retResult = new { status = "Error", message = ex.Message, picturePath = "" };
                    }
                }
            }
            else if (action == "D")
            {

                var user = db.UserInformation.Find(int.Parse(userID));
                if (user != null)
                {
                    checkFile = new FileInfo(serverFilePath);
                    var isExists = checkFile.Exists;
                    if (isExists) checkFile.Delete();

                    user.PictureFilePath = null;
                    relativeFilePath = "/images/no-profile-image.jpg";
                    db.SaveChanges();
                    retResult = new { status = "Success", message = "Profile picture is successfully deleted!", picturePath = relativeFilePath };
                }
                else
                {
                    retResult = new { status = "Error", message = "Invalid record data", picturePath = "" };
                }

            }

            return Json(retResult);
        }
        [HttpPost]
        public JsonResult AjaxCreateEditUser([Bind(Include = "EmployeeId,FirstName,MiddleInitial,FirstLastName,SecondLastName,SSN")] UserInformation model)
        {
            //dynamic retResult = new {id=0, status = "", message = "" };
            dynamic retResult = null;
            string empIdUniqueLevelStr = "";
            try
            {
                var isUniqueEmpIdAtClientLevel = ApplicationConfigurationService.GetConfigurationStatus("UseUniqueEmployeeIdClientLevel");
                empIdUniqueLevelStr = isUniqueEmpIdAtClientLevel ? "Client" : "Company";
                //var user = db.UserInformation.Find(model.Id); old based on internal id
                UserInformation user = null;
                if (isUniqueEmpIdAtClientLevel)
                {
                    //user = db.UserInformation.Where(w => w.DataEntryStatus == 1 &&
                    //                                w.EmployeeId == model.EmployeeId &&
                    //                                w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId).FirstOrDefault();

                    user= db.GetAll<UserInformation>(SessionHelper.SelectedClientId).Where(w=> w.EmployeeId == model.EmployeeId).FirstOrDefault();
                }
                else
                {
                    user = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.EmployeeId == model.EmployeeId).FirstOrDefault();

                }
                
                if (user == null)
                {
                    if (!string.IsNullOrEmpty(model.SSN))
                    {
                        string checkSSN = Encryption.Encrypt(model.SSN);
                        // var existingCount = db.UserInformation.Where(w => (w.SSNEncrypted == checkSSN)).Count();
                        //SSN should by unique within specific company(Task 208)
                        //var existingCount = db.UserInformation.Where(w => ((w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId)
                        //                                                    && w.SSNEncrypted == checkSSN)).Count();


                        var existingCount = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId).Where(w => w.SSNEncrypted == checkSSN).Count();
                        
                        if (existingCount > 0)
                        {
                            retResult = new { id = 0, status = "Error", message = $"SSN is already in use." };
                            return Json(retResult);
                        }
                    }

                    user = new UserInformation();

                    user.EmployeeId = model.EmployeeId;
                    user.FirstName = model.FirstName;
                    user.MiddleInitial = model.MiddleInitial;
                    user.FirstLastName = model.FirstLastName;
                    user.SecondLastName = model.SecondLastName;
                    user.ShortFullName = model.FullName;
                    user.ClientId = SessionHelper.SelectedClientId;
                    user.CompanyId = SessionHelper.SelectedCompanyId;
                    //user.SSNEnd             = string.IsNullOrEmpty(model.SSN)?null: model.SSN.Substring(model.SSN.Length-4);

                    user.SSNEncrypted = model.SSNEncrypted;
                    user.SSNEnd = model.SSNEndComputed;
                    user.EmployeeStatusId = 2;
                    user.EmployeeStatusDate = DateTime.Now;
                    db.UserInformation.Add(user);
                    db.SaveChanges();
                    //retResult.id = user.Id;
                    //retResult.status = "Success";
                    //retResult.message = "Employee is added successfully!";

                    retResult = new { id = Encryption.EncryptURLParm(user.Id.ToString()), status = "Success", message = "Employee is added successfully!" };
                }
                else
                {
                    // retResult.status = "Error";
                    // retResult.message = "Employee is already exists";
                    retResult = new { id = 0, status = "Error", message = $"Employee ID is already in use at {empIdUniqueLevelStr} Level." };
                }


            }
            catch (DbEntityValidationException ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //retResult.status = "Error";
                //retResult.message = ex.Message;
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }

            return Json(retResult);
        }

        // GET: UserInformation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Company, "Id", "CompanyName", userInformation.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName", userInformation.DepartmentId);
            ViewBag.EmploymentStatusId = new SelectList(db.EmploymentStatus, "Id", "EmploymentStatusName", userInformation.EmploymentStatusId);
            ViewBag.DefaultJobCodeId = new SelectList(db.JobCode, "Id", "JobCodeName", userInformation.DefaultJobCodeId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "Client", userInformation.ClientId);
            ViewBag.SubDepartmentId = new SelectList(SubDepartmentService.SubDepartments(userInformation.DepartmentId), "Id", "SubDepartmentName", userInformation.SubDepartmentId);
            ViewBag.EmployeeTypeID = new SelectList(db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeDescription", userInformation.EmployeeTypeID);
            //ViewBag.SupervisoryLevelId = new SelectList(db.SupervisoryLevel, "Id", "SupervisoryLevelDescription", userInformation.SupervisoryLevelId);
            ViewBag.GenderId = new SelectList(db.Gender, "Id", "GenderName", userInformation.GenderId);
            ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus, "Id", "MaritalStatusName", userInformation.MaritalStatusId);
            return View(userInformation);
        }

        // POST: UserInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdNumber,SystemID,FirstName,MiddleInitial,FirstLastName,SecondLastName,ShortFullName,ClientId,CompanyId,DepartmentId,SubDepartmentId,EmployeeTypeID,EmploymentStatusId,DefaultJobCodeId,SupervisoryLevelId,EmployeeNote,Gender,BirthDate,MaritalStatus,SSNEnd,SSNEncrypted,imgPhoto,PasswordHash,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] UserInformation userInformation)
        {
            if (ModelState.IsValid)
            {
                userInformation.ModifiedBy = SessionHelper.LoginId;
                userInformation.ModifiedDate = DateTime.Now;
                db.Entry(userInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(db.Company, "Id", "CompanyName", userInformation.CompanyId);
            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName", userInformation.DepartmentId);
            ViewBag.EmploymentStatusId = new SelectList(db.EmploymentStatus, "Id", "EmploymentStatusName", userInformation.EmploymentStatusId);
            ViewBag.DefaultJobCodeId = new SelectList(db.JobCode, "Id", "JobCodeName", userInformation.DefaultJobCodeId);
            ViewBag.ClientId = new SelectList(db.Client, "ClientId", "Client", userInformation.ClientId);
            ViewBag.SubDepartmentId = new SelectList(SubDepartmentService.SubDepartments(userInformation.DepartmentId), "Id", "SubDepartmentName", userInformation.SubDepartmentId);
            ViewBag.EmployeeTypeID = new SelectList(db.EmployeeType, "Id", "EmployeeTypeDescription", userInformation.EmployeeTypeID);
            //ViewBag.SupervisoryLevelId = new SelectList(db.SupervisoryLevel, "Id", "SupervisoryLevelDescription", userInformation.SupervisoryLevelId);
            ViewBag.GenderId = new SelectList(db.Gender, "Id", "GenderName", userInformation.GenderId);
            ViewBag.MaritalStatusId = new SelectList(db.MaritalStatus, "Id", "MaritalStatusName", userInformation.MaritalStatusId);
            return View(userInformation);
        }
        [HttpPost]
        public JsonResult AjaxUpdateGenInfo([Bind(Include = "Id,EmployeeId,FirstName,MiddleInitial,FirstLastName,SecondLastName,ShortFullName,GenderId,BirthDate,BirthPlace,MaritalStatusId,SSNEnd,EthnicityId,DisabilityId,SelectedVeteranStatus,SSN")] UserInformation model)
        {
            //userInformation.SelectedVeteranStatus
            string errorMessage = "";
            //var selectedVeteranList = (model.SelectedVeteranStatus ?? "").Split(',');
            List<string> selectedVeteranList = (model.SelectedVeteranStatus ?? "").Split(',').ToList();
            List<EmployeeVeteranStatus> veteranAddList = new List<EmployeeVeteranStatus>();
            List<EmployeeVeteranStatus> veteranRemoveList = new List<EmployeeVeteranStatus>();
            var isUniqueEmpIdAtClientLevel = ApplicationConfigurationService.GetConfigurationStatus("UseUniqueEmployeeIdClientLevel");
            var empIdUniqueLevelStr = isUniqueEmpIdAtClientLevel ? "Client" : "Company";
            try
            {
                var user = db.UserInformation.Find(model.Id);
                if (user == null) throw new Exception("Record not found in system");
                //var isEmployeeIdExist = db.UserInformation.Where(w => w.DataEntryStatus == 1 &&
                //                                                 w.Id != user.Id && w.ClientId == user.ClientId && w.CompanyId == user.CompanyId
                //                                                 && w.EmployeeId == model.EmployeeId).Count();
                var isEmployeeIdExist = 0;
                if (isUniqueEmpIdAtClientLevel)
                {

                    isEmployeeIdExist = db.GetAll<UserInformation>(SessionHelper.SelectedClientId)
                                           .Where(w => w.Id != user.Id && w.EmployeeId == model.EmployeeId && w.EmployeeStatusId!=4) // skip company transfer at client level
                                           .Count();
                }
                else
                {
                    isEmployeeIdExist = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                            .Where(w => w.Id != user.Id && w.EmployeeId == model.EmployeeId)
                                            .Count();

                }
                if (isEmployeeIdExist > 0) throw new Exception($"Employee Id is already in use at {empIdUniqueLevelStr} level.");

                user.ModifiedBy = SessionHelper.LoginId;
                user.ModifiedDate = DateTime.Now;
                user.EmployeeId = model.EmployeeId;
                user.FirstName = model.FirstName;
                user.MiddleInitial = model.MiddleInitial;
                user.FirstLastName = model.FirstLastName;
                user.SecondLastName = model.SecondLastName;
                user.ShortFullName = model.ShortFullName;
                user.BirthDate = model.BirthDate;
                user.BirthPlace = model.BirthPlace;
                user.GenderId = model.GenderId;
                user.MaritalStatusId = model.MaritalStatusId;
                // user.SSNEnd = model.SSNEnd;
                user.SSN = model.SSN;
                //if (user.SSNEnd != null)
                //Validate SSN number
                if (user.SSN != null)
                {   //validateUpdatedSSN, below return object
                    //{ isValid = validateStatus, newEncryptedSSN= validatedEncryptedSSN, errorMessage = errorMessage }
                    dynamic validationResult = validateUpdatedSSN(user.Id, user.SSN, user.SSNEnd);
                    if (validationResult.isValid)
                    {
                        user.SSNEncrypted = validationResult.newEncryptedSSN;
                        user.SSNEnd = user.SSNEndComputed;
                    }
                    else
                    {
                        var errorResult = new { status = "Error", message = validationResult.errorMessage };
                        return Json(errorResult);
                    }
                }
                user.EthnicityId = model.EthnicityId;
                user.DisabilityId = model.DisabilityId;
                foreach (var veteranItem in user.EmployeeVeteranStatus)
                {
                    // var RecCnt = selectedVeteranList.Where(w => w == veteranItem.Id.ToString()).Count();
                    var RecCnt = selectedVeteranList.Where(w => w == veteranItem.VeteranStatusId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        veteranRemoveList.Add(veteranItem);
                    }

                }
                foreach (var selectedveteranId in selectedVeteranList)
                {
                    if (selectedveteranId == "") continue;
                    int veteranId = int.Parse(selectedveteranId);
                    var recExists = user.EmployeeVeteranStatus.Where(w => w.VeteranStatusId == veteranId).Count();
                    if (recExists == 0)
                    {
                        veteranAddList.Add(new EmployeeVeteranStatus() { UserInformationId = user.Id, VeteranStatusId = veteranId, CreatedBy = 1, DataEntryStatus = 1, CreatedDate = DateTime.Now });

                    }
                }

                db.EmployeeVeteranStatus.RemoveRange(veteranRemoveList);
                db.EmployeeVeteranStatus.AddRange(veteranAddList);

                db.SaveChanges();
                var okResult = new { status = "Success", message = "Changes are saved successfully" };
                return Json(okResult);
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                errorMessage = ex.Message;
            }
            var retResult = new { status = "Error", message = errorMessage };
            return Json(retResult);
        }
        private dynamic validateUpdatedSSN(int userID, string SSN, string newSSNEnd)
        {
            bool validateStatus = true;
            string validateUpdatedSSN = "";
            string validatedEncryptedSSN = "";
            string errorMess = "";
           
            //if (newSSNEnd.Length == 4)
            //{
            if (!string.IsNullOrEmpty(SSN))
            {

                // validateUpdatedSSN = SSN.Substring(0, 7) + newSSNEnd;
                validateUpdatedSSN = SSN;
                if (validateUpdatedSSN.Length == 9)
                {
                    validatedEncryptedSSN = Encryption.Encrypt(validateUpdatedSSN);
                    //var existingCount = db.UserInformation.Where(w => (w.SSNEncrypted == validatedEncryptedSSN) && w.Id != userID).Count();
                    //SSN should by unique within specific company(Task 208)
                    //var existingCount = db.UserInformation.Where(w => (w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId)
                    //                                                       && (w.SSNEncrypted == validatedEncryptedSSN && w.Id != userID)).Count();

                    var existingCount = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.SSNEncrypted == validatedEncryptedSSN && w.Id != userID).Count();
                   
                    if (existingCount > 0)
                    {
                        validateStatus = false;
                        errorMess = $"SSN is already in use.";
                    }

                }
                else
                {
                    validateStatus = false;
                    errorMess = "Invalid New SSN Format.";
                }
            }
            else
            {
                validateStatus = false;
                errorMess = "SSN doesn't exist, Can't be updated!";
            }
            //}
            //else
            //{
            //    validateStatus = false;
            //    errorMess = "Invalid SSN End Format!";
            //}
            return new { isValid = validateStatus, newEncryptedSSN = validatedEncryptedSSN, errorMessage = errorMess };
        }
        //Company Stat Widget
        public ActionResult GetDashboardGenderStatWidget(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);
            var model = new DashboardGenderStatWidgetViewModel();
            var userRecords = getViewBasedUserList(false, null);
            //var companyUserList = db.GetAll<Employment>(SessionHelper.SelectedClientId).
            //                         Where(u => u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&
            //                         (u.OriginalHireDate >= userRecordPeriodRange.fromDate && u.OriginalHireDate <= userRecordPeriodRange.toDate)
            //                         && u.TerminationDate == null && u.UserInformation.EmployeeStatusId == 1).Select(s => s.UserInformation);

            var genderUserList = db.GetAll<Employment>(SessionHelper.SelectedClientId).
                                     Where(u => u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&
                                     (u.OriginalHireDate >= userRecordPeriodRange.fromDate && u.OriginalHireDate <= userRecordPeriodRange.toDate)
                                     && u.TerminationDate == null && u.UserInformation.EmployeeStatusId == 1)
                                    .Join(userRecords,
                                          empmnt => empmnt.UserInformationId,
                                           ur => ur.Id,
                                           (empmnt, ur) => ur).Distinct();

            model.TotalEmployee = genderUserList.Count();
            model.GenderStatistics = new List<GenderStatViewModel>();
            var genderList = db.GetAll<Gender>();
            foreach (var gender in genderList)
            {

                var genderStat = new GenderStatViewModel()
                {
                    Id = gender.Id,
                    Name = gender.GenderName,
                    TotalCount = model.TotalEmployee,
                    GenderCount = genderUserList.Where(w => w.GenderId == gender.Id).Count()
                };
                model.GenderStatistics.Add(genderStat);

            }


            return PartialView("DashboardGenderStat", model);
        }
        //End Company Stat
        public ActionResult GetDashboardEmployeeRecordWidget(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);
            var model = new DashboardUserWidgetViewModel();
            //var userRecordsInPeriod = getUserList().Where(w => w.EmployeeStatusDate >= userRecordPeriodRange.fromDate
            //                                               && w.EmployeeStatusDate <= userRecordPeriodRange.toDate); 
            // var userRecordsInPeriod = getViewBasedUserList(false, null).Where(w => w.EmployeeStatusDate >= userRecordPeriodRange.fromDate
            // && w.EmployeeStatusDate <= userRecordPeriodRange.toDate);
            var userRecords = getViewBasedUserList(false, null);
            //Total
            model.ActiveUserCount = userRecords.Where(w => w.EmployeeStatusId == 1).Count();
            model.InactiveUserCount = userRecords.Where(w => w.EmployeeStatusId == 2).Count();
            model.ClosedRecordCount = userRecords.Where(w => w.EmployeeStatusId == 3).Count();
            //End
            //New Records
            model.NewHireCount = db.GetAll<Employment>(SessionHelper.SelectedClientId).
                                    Where(u => u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&
                                    (u.OriginalHireDate >= userRecordPeriodRange.fromDate && u.OriginalHireDate <= userRecordPeriodRange.toDate)
                                    && u.TerminationDate == null && u.UserInformation.EmployeeStatusId == 1)
                                   .Join(userRecords,
                                         empmnt => empmnt.UserInformationId,
                                          ur => ur.Id,
                                          (empmnt, ur) => new
                                          {
                                              Id = ur.Id
                                          }
                                         ).Distinct().Count();

            model.NewInactiveCount = userRecords.Where(w => w.EmployeeStatusId == 2 && w.EmployeeStatusDate >= userRecordPeriodRange.fromDate
                                                                                     && w.EmployeeStatusDate <= userRecordPeriodRange.toDate).Count();

            model.NewClosedCount = db.GetAll<Employment>(SessionHelper.SelectedClientId).
                                    Where(u => u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&
                                    (u.TerminationDate >= userRecordPeriodRange.fromDate && u.TerminationDate <= userRecordPeriodRange.toDate)
                                    && u.UserInformation.EmployeeStatusId == 3)
                                   .Join(userRecords,
                                         empmnt => empmnt.UserInformationId,
                                          ur => ur.Id,
                                          (empmnt, ur) => new
                                          {
                                              Id = ur.Id
                                          }
                                         ).Distinct().Count();

            return PartialView("DashboardEmployeeRecord", model);
        }
        public ActionResult GetDashboardEmployeeList(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);
            var userRecords = getViewBasedUserList(false, null);
            // var userRecordsInPeriod = getViewBasedUserList(true, null).Where(w => w.EmployeeStatusDate >= userRecordPeriodRange.fromDate);
            IEnumerable<UserInformationViewModel> userFilteredRecords = null;
            //Status based filtering the list
            switch (parmModel.UserRecordStatusTypeId)
            {
                case 0: // All Active 
                    userFilteredRecords = userRecords.Where(w => w.EmployeeStatusId == 1);
                    break;
                case 2: // All Inactive
                    userFilteredRecords = userRecords.Where(w => w.EmployeeStatusId == 2);
                    break;
                case 3: // All Closed
                    userFilteredRecords = userRecords.Where(w => w.EmployeeStatusId == 3);
                    break;
                case 4: // new Hired
                    userFilteredRecords = db.GetAll<Employment>(SessionHelper.SelectedClientId).
                                    Where(u => u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&
                                    (u.OriginalHireDate >= userRecordPeriodRange.fromDate && u.OriginalHireDate <= userRecordPeriodRange.toDate)
                                    && u.TerminationDate == null && u.UserInformation.EmployeeStatusId == 1)
                                   .Join(userRecords,
                                         empmnt => empmnt.UserInformationId,
                                          ur => ur.Id,
                                          (empmnt, ur) => ur);
                    break;
                case 5: //new Inactive
                    //
                    userFilteredRecords = userRecords.Where(w => w.EmployeeStatusId == 2 && w.EmployeeStatusDate >= userRecordPeriodRange.fromDate
                                                                                       && w.EmployeeStatusDate <= userRecordPeriodRange.toDate);
                    break;
                case 6: //new Closed
                    userFilteredRecords = db.GetAll<Employment>(SessionHelper.SelectedClientId).
                                    Where(u => u.UserInformation.CompanyId == SessionHelper.SelectedCompanyId &&
                                    (u.TerminationDate >= userRecordPeriodRange.fromDate && u.TerminationDate <= userRecordPeriodRange.toDate)
                                    && u.UserInformation.EmployeeStatusId == 3)
                                   .Join(userRecords,
                                         empmnt => empmnt.UserInformationId,
                                          ur => ur.Id,
                                          (empmnt, ur) => ur);
                    //
                    break;

            }
            return PartialView("DashboardEmployeeList", userFilteredRecords.OrderBy(o => o.EmployeeId));
        }

        public ActionResult GetDashboardCurrentNotificationRecordWidget(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);
            var model = new DashboardNotificationWidgetViewModel();
            //var userList = getUserList().Select(s => s.Id);
            var userList = getViewBasedUserList(false, null).Select(s => s.Id);
            var userIds = String.Join(",", userList);

            var userIdsParameter = new SqlParameter("@userIds", userIds);
            var fromDateParameter = new SqlParameter("@fromDate", userRecordPeriodRange.fromDate);
            var toDateParameter = new SqlParameter("@toDate", userRecordPeriodRange.toDate);
            //Db Procedure Calling
            try
            {
                var notificationData = db.Database
                    .SqlQuery<DashboardNotificationListViewModel>("sp_userDashboardNotificationLog @userIds,@fromDate,@toDate", userIdsParameter, fromDateParameter, toDateParameter)
                    .ToList();

                //End Calling
                if (notificationData != null)
                {
                    model.NotifyingCount = notificationData.Where(w => w.RecordStatus == "Notifying").Count();
                    model.ExpiredCount = notificationData.Where(w => w.RecordStatus == "Expired").Count();
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
            }


            return PartialView("DashboardCurrenNotificationRecord", model);
        }
        public ActionResult GetDashboardMyNotificationRecordWidget(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);
            var model = new DashboardNotificationWidgetViewModel();
            //var userList = getUserList().Select(s => s.Id);
            //var userList = getViewBasedUserList(false).Select(s => s.Id);
            var userId = parmModel.UserId ?? SessionHelper.LoginId;
            var userIds = userId.ToString();

            var userIdsParameter = new SqlParameter("@userIds", userIds);
            var fromDateParameter = new SqlParameter("@fromDate", userRecordPeriodRange.fromDate);
            var toDateParameter = new SqlParameter("@toDate", userRecordPeriodRange.toDate);
            //Db Procedure Calling
            try
            {
                var notificationData = db.Database
                    .SqlQuery<DashboardNotificationListViewModel>("sp_userDashboardNotificationLog @userIds,@fromDate,@toDate", userIdsParameter, fromDateParameter, toDateParameter)
                    .ToList();

                //End Calling
                if (notificationData != null)
                {
                    model.NotifyingCount = notificationData.Where(w => w.RecordStatus == "Notifying").Count();
                    model.ExpiredCount = notificationData.Where(w => w.RecordStatus == "Expired").Count();
                }
            }
            catch (Exception ex)
            {
            }


            return PartialView("DashboardMyNotificationRecord", model);
        }
        public ActionResult GetDashboardCurrentNotificationList(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);

            //var userList = getUserList().Select(s => s.Id);
            var userList = getViewBasedUserList(false, null).Select(s => s.Id);
            var userIds = String.Join(",", userList);
            IList<DashboardNotificationListViewModel> currNotificationList = new List<DashboardNotificationListViewModel>();
            var userIdsParameter = new SqlParameter("@userIds", userIds);
            var fromDateParameter = new SqlParameter("@fromDate", userRecordPeriodRange.fromDate);
            var toDateParameter = new SqlParameter("@toDate", userRecordPeriodRange.toDate);
            //Db Procedure Calling
            var notificationData = db.Database
                .SqlQuery<DashboardNotificationListViewModel>("sp_userDashboardNotificationLog @userIds,@fromDate,@toDate", userIdsParameter, fromDateParameter, toDateParameter)
                .ToList().OrderBy(o => o.ExpirationDate);
            //End Calling
            if (notificationData != null)
            {
                if (parmModel.UserRecordStatusTypeId == null)
                {
                    currNotificationList = notificationData.ToList();
                }
                else if (parmModel.UserRecordStatusTypeId == 0)
                {
                    currNotificationList = notificationData.Where(w => w.RecordStatus == "Notifying").ToList();
                }
                else
                {
                    currNotificationList = notificationData.Where(w => w.RecordStatus == "Expired").ToList();
                }
            }

            return PartialView("DashboardCurrenNotificationList", currNotificationList);
        }
        public ActionResult GetDashboardMyNotificationList(DashboardViewModel parmModel)
        {
            dynamic userRecordPeriodRange = getUserRecordPeriodRange(parmModel.UserRecordPeriodTypeId);

            //var userList = getUserList().Select(s => s.Id);
            //var userList = getViewBasedUserList(false).Select(s => s.Id);
            var userId = parmModel.UserId ?? SessionHelper.LoginId;
            var userIds = userId.ToString();
            IList<DashboardNotificationListViewModel> currNotificationList = new List<DashboardNotificationListViewModel>();
            var userIdsParameter = new SqlParameter("@userIds", userIds);
            var fromDateParameter = new SqlParameter("@fromDate", userRecordPeriodRange.fromDate);
            var toDateParameter = new SqlParameter("@toDate", userRecordPeriodRange.toDate);
            //Db Procedure Calling
            var notificationData = db.Database
                .SqlQuery<DashboardNotificationListViewModel>("sp_userDashboardNotificationLog @userIds,@fromDate,@toDate", userIdsParameter, fromDateParameter, toDateParameter)
                .ToList().OrderBy(o => o.ExpirationDate);
            //End Calling
            if (notificationData != null)
            {
                if (parmModel.UserRecordStatusTypeId == null)
                {
                    currNotificationList = notificationData.ToList();
                }
                else if (parmModel.UserRecordStatusTypeId == 0)
                {
                    currNotificationList = notificationData.Where(w => w.RecordStatus == "Notifying").ToList();
                }
                else
                {
                    currNotificationList = notificationData.Where(w => w.RecordStatus == "Expired").ToList();
                }
            }

            return PartialView("DashboardMyNotificationList", currNotificationList);
        }
        public ActionResult GetDashboardForecastNotificationRecordWidget()
        {

            var userList = getViewBasedUserList(false, null).Select(s => s.Id);
            var userIds = String.Join(",", userList);

            var userIdsParameter = new SqlParameter("@userIds", userIds);

            //Db Procedure Calling
            var notificationData = db.Database
                .SqlQuery<DashboardNotificationForecastViewModel>("sp_UserDashboardNotificationForecast @userIds", userIdsParameter)
                .ToList();
            //End Calling

            return PartialView("DashboardForecastNotificationRecord", notificationData);
        }
        public ActionResult GetDashboardForecastNotificationList(DashboardViewModel parmModel)
        {
            var forecastMonthStr = parmModel.ForecastMonthId;
            var forecastMoth = int.Parse(forecastMonthStr.Substring(0, 2));
            var forecastYear = int.Parse(forecastMonthStr.Substring(2, 4));
            var forcastMonthStart = new DateTime(forecastYear, forecastMoth, 1);
            var forcastMonthEnd = forcastMonthStart.AddMonths(1).AddDays(-1);
            ViewBag.MonthTitle = forcastMonthStart.ToString("MMMM-yyyy");
            var userList = getViewBasedUserList(false, null).Select(s => s.Id);
            var userIds = String.Join(",", userList);

            //  IList<DashboardNotificationListViewModel> currNotificationList = new List<DashboardNotificationListViewModel>();
            var userIdsParameter = new SqlParameter("@userIds", userIds);
            var fromDateParameter = new SqlParameter("@fromDate", forcastMonthStart);
            var toDateParameter = new SqlParameter("@toDate", forcastMonthEnd);

            //Db Procedure Calling
            var notificationData = db.Database
                .SqlQuery<DashboardNotificationListViewModel>("sp_UserDashboardNotificationForecastDetail @userIds,@fromDate,@toDate", userIdsParameter, fromDateParameter, toDateParameter)
                .ToList().OrderBy(o => o.ExpirationDate);
            //End Calling


            return PartialView("DashboardForecastNotificationList", notificationData);
        }

        public ActionResult GetReportByEmployeeList(int? superviorId)
        {

            //var selectionByEmployee = getViewBasedUserList(false, null);

            // 
            IEnumerable<UserInformationViewModel> userInformationList = null;
            try
            {
                var employeeId = 0;
                var employeeName = "";
                var positionId = 0;
                var employeeStatusId = 0;
                var selectedSuperviorId = superviorId == null ? SessionHelper.LoginId : superviorId.Value;
                //Calling Db Procedure
                userInformationList = db.SP_UserInformation<UserInformationViewModel>(employeeId, employeeName, positionId,
                                        employeeStatusId, selectedSuperviorId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView("ReportByEmployeeList", userInformationList.OrderBy(w => w.EmployeeId));
        }

        private dynamic getUserRecordPeriodRange(int periodType)
        {
            DateTime periodFromDate, periodToDate;
            var periodTempToDate = DateTime.Today;
            periodToDate = periodTempToDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            periodFromDate = periodTempToDate;
            switch (periodType)
            {
                case -1: // All
                    periodFromDate = periodTempToDate.AddYears(-200);
                    break;
                case 0: // Today
                    periodFromDate = periodTempToDate;
                    break;
                case 1: // This week
                    periodFromDate = periodTempToDate.
                    AddDays(((int)(periodTempToDate.DayOfWeek) * -1) + 1);
                    break;
                case 2: // Two weeks
                    periodFromDate = periodTempToDate.
                    AddDays(-14);
                    break;
                case 3: // One Month
                    periodFromDate = periodTempToDate.
                    AddMonths(-1);
                    break;
                case 4: // Quarter (3 Months)
                    periodFromDate = periodTempToDate.
                    AddMonths(-3);
                    break;
                case 6: // Semester (6 Months)
                    periodFromDate = periodTempToDate.
                    AddMonths(-6);
                    break;
                case 12: // One Year (12 Months)
                    periodFromDate = periodTempToDate.
                    AddMonths(-12);
                    break;
            }
            return new { fromDate = periodFromDate, toDate = periodToDate };
        }
        // GET: UserInformation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInformation userInformation = db.UserInformation.Find(id);
            if (userInformation == null)
            {
                return HttpNotFound();
            }
            return View(userInformation);
        }

        // POST: UserInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserInformation userInformation = db.UserInformation.Find(id);
            //db.UserInformation.Remove(userInformation);
            userInformation.DataEntryStatus = 0;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: UserContactInformation/Edit/5
        public ActionResult EditContactInformation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserContactInformation userContactInformation = db.UserContactInformation.Where(u => u.UserInformationId == id.Value).FirstOrDefault();
            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                //return HttpNotFound();
            }
            //ViewBag.HomeCityId = new SelectList(db.City, "Id", "CityName", userContactInformation.HomeCityId);
            //ViewBag.MailingCityId = new SelectList(db.City, "Id", "CityName", userContactInformation.MailingCityId);
            //ViewBag.MailingStateId = new SelectList(db.State, "Id", "StateName", userContactInformation.MailingStateId);
            //ViewBag.HomeStateId = new SelectList(db.State, "Id", "StateName", userContactInformation.HomeStateId);
            //ViewBag.UserID = new SelectList(db.UserInformation, "Id", "IdNumber", userContactInformation.UserID);
            return View(userContactInformation);
        }

        // POST: UserContactInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContactInformation([Bind(Include = "UserContactInformationId,UserInformationId,HomeAddress1,HomeAddress2,HomeCityId,HomeStateId,HomeZipCode,MailingAddress1,MailingAddress2,MailingCityId,MailingStateId,MailingZipCode,HomeNumber,CelNumber,FaxNumber,OtherNumber,WorkEmail,PersonalEmail,OtherEmail,EmergencyContact,EmergencyRelationship,EmergencyNumber1,EmergencyNumber2,WorkNumber,WorkExtension,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] UserContactInformation userContactInformation)
        {
            if (ModelState.IsValid)
            {
                if (userContactInformation.Id == 0)
                {
                    db.UserContactInformation.Add(userContactInformation);
                }
                else
                {
                    db.Entry(userContactInformation).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.HomeCityId = new SelectList(db.City, "Id", "CityName", userContactInformation.HomeCityId);
            //ViewBag.MailingCityId = new SelectList(db.City, "Id", "CityName", userContactInformation.MailingCityId);
            //ViewBag.MailingStateId = new SelectList(db.State, "Id", "StateName", userContactInformation.MailingStateId);
            //ViewBag.HomeStateId = new SelectList(db.State, "Id", "StateName", userContactInformation.HomeStateId);
            //ViewBag.UserID = new SelectList(db.UserInformation, "Id", "IdNumber", userContactInformation.UserID);
            return View(userContactInformation);
        }

        public ActionResult AddSupervisor(int userId)
        {
            var model = new EmployeeSupervisorViewModel();
            model.SelectedUserId = userId;
            UserInformation userInformation = db.UserInformation.FirstOrDefault(u => u.Id == userId);

            var supervisors = db.EmployeeSupervisor.Where(u => u.SupervisorUserId == userId && u.DataEntryStatus == 1).ToList();
            List<SelectListItem> supervisorsItems = new List<SelectListItem>();
            foreach (var team in supervisors)
            {
                supervisorsItems.Add(new SelectListItem { Value = team.EmployeeUserId.ToString(), Text = team.EmployeeUser.FullName });
            }
            model.SupervisedUsers = new MultiSelectList(supervisorsItems.OrderBy(i => i.Text), "Value", "Text");
            model.SupervisedUserId = new List<string>();

            var users = db.UserInformation.Where(u => u.ClientId == userInformation.ClientId && u.Id != userId).ToList();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var team in users)
            {
                var addedU = supervisors.FirstOrDefault(s => s.EmployeeUserId == team.Id);
                if (addedU == null)
                    items.Add(new SelectListItem { Value = team.Id.ToString(), Text = team.FullName });
            }
            model.Users = new MultiSelectList(items.OrderBy(i => i.Text), "Value", "Text");
            model.AllUserId = new List<string>();

            return PartialView(model);
        }

        // POST: EmployeeSupervisor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddSupervisor([Bind(Include = "SelectedUserId,SupervisedUsers,Users,SupervisedUserId,UserId")] EmployeeSupervisorViewModel employeeSupervisor)
        {
            if (ModelState.IsValid)
            {
                var supervisor = db.EmployeeSupervisor.Where(e => e.SupervisorUserId == employeeSupervisor.SelectedUserId);
                supervisor.ToList().ForEach(c => c.DataEntryStatus = 0);

                if (employeeSupervisor.SupervisedUserId != null)
                {
                    foreach (var each in employeeSupervisor.SupervisedUserId)
                    {
                        int userId = Convert.ToInt32(each);

                        var employee = supervisor.FirstOrDefault(s => s.EmployeeUserId == userId);
                        if (employee == null)
                        {
                            db.EmployeeSupervisor.Add(new EmployeeSupervisor()
                            {
                                EmployeeUserId = userId,
                                SupervisorUserId = employeeSupervisor.SelectedUserId
                            });

                        }
                        else
                        {
                            employee.DataEntryStatus = 1;
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("EmployeePrivilegeView", new { id = employeeSupervisor.SelectedUserId });
            }

            //ViewBag.EmployeeUserId = new SelectList(db.UserInformation, "Id", "FullName", employeeSupervisor.EmployeeUserId);
            //ViewBag.SupervisorUserId = new SelectList(db.UserInformation, "Id", "FullName", employeeSupervisor.SupervisorUserId);
            return GetErrors();
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
            return Json(new { success = false, errors = "[" + string.Join(",", entries) + "]" }, JsonRequestBehavior.AllowGet);
        }
        // GET: UserContactInformation/Create
        public ActionResult EmployeeSystemAccess(int? id, string viewType)
        {
            if (!SecurityHelper.IsAvailable("Profile"))
            {
                Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }

            UserContactInformation userContactInformation = null;
            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);
            var userInformation = db.UserInformation.Find(id ?? 0);
            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                userContactInformation.UserInformationId = id ?? 0;
                ViewBag.Label = ViewBag.Label + " - Add";
            }
            else
            {
                ViewBag.Label = ViewBag.Label + " - Edit";
            }

            EmployeePrivilegeViewModel model = new EmployeePrivilegeViewModel();
            model.ViewType = viewType;

            model.Id = userContactInformation.Id;
            model.UserInformationId = userContactInformation.UserInformationId;
            ViewBag.UserInformationId = id;
            var emailList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(userContactInformation.WorkEmail))
                emailList.Add(new SelectListItem() { Text = userContactInformation.WorkEmail, Value = userContactInformation.WorkEmail });
            if (!string.IsNullOrEmpty(userContactInformation.PersonalEmail))
                emailList.Add(new SelectListItem() { Text = userContactInformation.PersonalEmail, Value = userContactInformation.PersonalEmail });
            if (!string.IsNullOrEmpty(userContactInformation.OtherEmail))
                emailList.Add(new SelectListItem() { Text = userContactInformation.OtherEmail, Value = userContactInformation.OtherEmail });

            if (viewType == "Access Level")
            {
                model.LoginEmail = userContactInformation.LoginEmail;
                if (userInformation != null)
                {
                    if (userInformation.UserInformationRole.Count > 0)
                    {
                        model.RoleId = userInformation.UserInformationRole.FirstOrDefault().RoleId;
                        model.RoleTypeId = userInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId ?? 0;
                        model.Role = userInformation.UserInformationRole.FirstOrDefault().Role;
                    }
                }
                ViewBag.RoleId = new SelectList(db.Role.Take(0), "Id", "RoleName");
            }
            if (viewType == "LockAccount")
            {
                model.LockAccount = UserManagmentService.IsLockout(userContactInformation.LoginEmail);
            }
            if (viewType == "Account")
            {
                bool isActiveEmployee = true;
                if (userContactInformation.UserInformation.EmployeeStatus == null)
                {
                    isActiveEmployee = false;
                }
                else if (userContactInformation.UserInformation.EmployeeStatus.Id == (int)EmployeeStatusNames.Closed)
                {
                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("ClosedEmployeesAccessAllow");
                    if (configuration == null || !configuration.ValueAsBoolean)
                        isActiveEmployee = false;

                }
                else if (userContactInformation.UserInformation.EmployeeStatus.Id == (int)EmployeeStatusNames.Inactive)
                {
                    var configuration = ApplicationConfigurationService.GetApplicationConfiguration("InactiveEmployeesAccessAllow");
                    if (configuration == null || !configuration.ValueAsBoolean)
                        isActiveEmployee = false;
                }
                ViewBag.IsActiveEmployee = isActiveEmployee;               
                ViewBag.LoginEmail = userContactInformation.LoginEmail;
                ViewBag.LoginEmailAddressTypeId = new SelectList(emailList, "Value", "Text", userContactInformation.LoginEmail);
            }
            if(viewType == "Notification")
            {
                ViewBag.NotificationEmailAddressTypeId = new SelectList(emailList, "Value", "Text", userContactInformation.NotificationEmail);
                //NotificationEmailAddressTypeId
            }
            if (viewType == "Access Restrictions")
            {
                model.SelectedCompanyId = String.Join(",", db.SupervisorCompany.Where(w => w.DataEntryStatus == 1 && w.Company.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.Company.Id.ToString()));
                model.SelectedDepartmentId = String.Join(",", db.SupervisorDepartment.Where(w => w.DataEntryStatus == 1 && w.Department.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.Department.Id.ToString()));
                model.SelectedSubDepartmentId = String.Join(",", db.SupervisorSubDepartment.Where(w => w.DataEntryStatus == 1 && w.SubDepartment.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.SubDepartment.Id.ToString()));
                model.SelectedEmployeeTypeId = String.Join(",", db.SupervisorEmployeeType.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).Select(c => c.EmployeeType.Id.ToString()));
                ViewBag.CompanyId = GetAllCompnies();
                ViewBag.DepartmentId = new SelectList(new List<Department>(), "Id", "DepartmentName");
                ViewBag.SubDepartmentId = new SelectList(new List<SubDepartment>(), "Id", "SubDepartmentName");
                ViewBag.EmployeeTypeId = new SelectList(new List<EmployeeType>(), "Id", "EmployeeTypeName");
                model.HasAllCompany = userInformation.HasAllCompany;
                if (userInformation.HasAllCompany.HasValue && userInformation.HasAllCompany.Value)
                    model.SelectedCompanyId = "0";
            }

            if (viewType == "Member Of")
            {
                model.SelectedEmployeeGroupId = String.Join(",", db.UserEmployeeGroup.Where(w => w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId && w.CompanyId == SessionHelper.SelectedCompanyId && w.UserInformationId == id).Select(c => c.EmployeeGroupId.ToString()));
                ViewBag.EmployeeGroupId = new SelectList(db.GetAllByCompany<EmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeGroupName");
            }

            return PartialView(model);
        }

        private SelectList GetAllCompnies()
        {
            List<Company> companies = db.Company.Where(w => w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId).OrderBy(o => o.CompanyName).ToList();
            companies.Insert(0, new Company() { CompanyName = "All", Id = 0 });
            SelectList companyList = new SelectList(companies, "Id", "CompanyName");
            return companyList;
        }

        [HttpPost]
        public JsonResult EmployeeActivationAccessLogin(EmployeePrivilegeViewModel model)
        {
            string status = "Success";
            string message = "User Access/Login is updated successfully!";

            using (var context = new TimeAideContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {

                    var contact = context.UserContactInformation.FirstOrDefault(c => c.Id == model.Id);
                    var userInformation = context.UserInformation.Find(model.UserInformationId);
                    try
                    {
                        string accountActivityType = "";
                        if (model.ViewType == "Account" || model.ViewType == "AccountRegister")
                        {
                            var exsistingLoginEmail = db.UserContactInformation.Where(c => c.LoginEmail == model.LoginEmail && c.Id != contact.Id);
                            if (exsistingLoginEmail.Count() > 0)
                            {
                                throw new Exception("Login Email is already in use.");
                            }

                            accountActivityType = GetAcountActivityType(model, contact);
                            if (accountActivityType == "EmailIsChanged")
                            {
                                if (!UserManagmentService.DeleteUser(contact.LoginEmail))
                                {
                                    throw new Exception("There was an error while completing the Access/Login.");
                                }
                                contact.UserInformation.AspNetUserId = null;
                            }

                            contact.LoginEmail = model.LoginEmail;
                        }

                        if (userInformation != null)
                        {
                            if (!userInformation.UserInformationRole.Any(u => u.RoleId == model.RoleId))
                            {
                                foreach (var each in userInformation.UserInformationRole.ToArray())
                                    context.Entry(each).State = EntityState.Deleted;
                                userInformation.UserInformationRole.Clear();
                                userInformation.UserInformationRole.Add(new UserInformationRole { UserInformationId = userInformation.Id, RoleId = model.RoleId });
                            }
                        }

                        context.SaveChanges();

                        if (model.ViewType == "Account")
                        {
                            ResendRegistrationEmail(model, contact, context, false);
                        }
                        else if (model.ViewType == "AccountRegister")
                        {
                            ResendRegistrationEmail(model, contact, context, true);
                        }
                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        status = "Error";
                        message = ex.Message;
                    }


                }
            }

            return Json(new { status = status, message = message });
        }

        [HttpPost]
        public ActionResult EmployeeSystemAccess(EmployeePrivilegeViewModel model)
        {
            if (!SecurityHelper.IsAvailable("Profile"))
            {
                Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
            var isSeperateEmailNotification = false;
            var configuration = ApplicationConfigurationService.GetApplicationConfiguration("UseSeperateEmailNotification");
            if (configuration != null)
                isSeperateEmailNotification = configuration.ValueAsBoolean;

            if (ModelState.IsValid)
            {
                using (var context = new TimeAideContext())
                {
                    using (var dbContextTransaction = context.Database.BeginTransaction())
                    {

                        var contact = context.UserContactInformation.FirstOrDefault(c => c.Id == model.Id);
                        var userInformation = context.UserInformation.Find(model.UserInformationId);
                        try
                        {
                            string accountActivityType = "";
                            if (model.ViewType == "Account" || model.ViewType == "AccountRegister")
                            {
                                var exsistingLoginEmail = db.UserContactInformation.Where(c => c.LoginEmail == model.LoginEmail && c.Id != contact.Id);
                                if (exsistingLoginEmail.Count() > 0)
                                {
                                    ModelState.AddModelError("LoginEmail", "Login Email is already in use.");
                                    return GetErrors();
                                }
                                //if (string.IsNullOrEmpty(contact.UserInformation.AspNetUserId) && model.LockAccount)
                                //{
                                //    if (!UserManagmentService.SetLockout(contact.LoginEmail))
                                //    {
                                //        throw new Exception("There was an error while completing the request.");
                                //    }
                                //}
                                accountActivityType = GetAcountActivityType(model, contact);
                                if (accountActivityType == "EmailIsChanged")
                                {
                                    if (!UserManagmentService.DeleteUser(contact.LoginEmail))
                                    {
                                        throw new Exception("There was an error while completing the request.");
                                    }
                                    contact.UserInformation.AspNetUserId = null;
                                }

                                contact.LoginEmail = model.LoginEmail;
                                if (isSeperateEmailNotification == false)
                                {
                                    contact.NotificationEmail = model.LoginEmail;
                                }
                                else if(isSeperateEmailNotification == true && string.IsNullOrEmpty(contact.NotificationEmail) == true)
                                {
                                    contact.NotificationEmail = model.LoginEmail;
                                }
                            }
                            if (model.ViewType == "LockAccount" && !string.IsNullOrEmpty(contact.UserInformation.AspNetUserId))
                            {
                                var isLockAccount = UserManagmentService.IsLockout(contact.LoginEmail);
                                if (model.LockAccount && !isLockAccount)
                                {
                                    if (!UserManagmentService.SetLockout(contact.LoginEmail))
                                    {
                                        throw new Exception("There was an error while completing the request.");
                                    }
                                }
                                else if (!model.LockAccount && isLockAccount)
                                {
                                    if (!UserManagmentService.SetUnLockout(contact.LoginEmail))
                                    {
                                        throw new Exception("There was an error while completing the request.");
                                    }
                                }
                            }
                            if (model.ViewType == "Access Level")
                            {
                                if (userInformation != null)
                                {
                                    if (!userInformation.UserInformationRole.Any(u => u.RoleId == model.RoleId))
                                    {
                                        foreach (var each in userInformation.UserInformationRole.ToArray())
                                            context.Entry(each).State = EntityState.Deleted;
                                        userInformation.UserInformationRole.Clear();
                                        userInformation.UserInformationRole.Add(new UserInformationRole { UserInformationId = userInformation.Id, RoleId = model.RoleId });
                                    }
                                }
                            }

                            if (model.ViewType == "Access Restrictions")
                            {
                                UpdateSupervisorCompany(model, contact, context);
                                UpdateSupervisorEmployeeType(model, contact, context);
                                UpdateSupervisorDepartment(model, contact, context);
                                UpdateSupervisorSubDepartment(model, contact, context);
                            }

                            if (model.ViewType == "Member Of")
                            {
                                UpdateUserEmployeeGroups(model, false, context);
                            }


                            if (model.ViewType == "Account" || model.ViewType == "AccountRegister")
                            {
                                TimeAide.Services.UserInformationService.AddUserRegistrationLog(model.ViewType, context, userInformation, accountActivityType);
                            }
                            if (model.ViewType == "Notification")
                            {
                                contact.NotificationEmail = model.NotificationEmail;
                            }
                            //context.AuditLogDetail.Add(logDetail);
                            context.SaveChanges();


                            if (model.ViewType == "Account")
                            {
                                ResendRegistrationEmail(model, contact, context, false);
                            }
                            else if (model.ViewType == "AccountRegister")
                            {
                                ResendRegistrationEmail(model, contact, context, true);
                            }
                            
                            dbContextTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        }
                        return RedirectToAction("EmployeeSystemAccessView", new { id = userInformation.Id });

                    }
                }
            }
            return GetErrors();
        }

        [HttpPost]
        public ActionResult DeleteLoginEmail(EmployeePrivilegeViewModel model)
        {
            if (!SecurityHelper.IsAvailable("Profile"))
            {
                Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
            var isSeperateEmailNotification = false;
            var configuration = ApplicationConfigurationService.GetApplicationConfiguration("UseSeperateEmailNotification");
            if (configuration != null)
                isSeperateEmailNotification = configuration.ValueAsBoolean;
            
            if (ModelState.IsValid)
            {
                var contact = db.UserContactInformation.FirstOrDefault(c => c.Id == model.Id);
                UserManagmentService.DeleteUser(contact.LoginEmail);
                contact.LoginEmail = null;
                if(isSeperateEmailNotification == false)
                {
                    contact.NotificationEmail = null;
                }
                db.SaveChanges();
                return RedirectToAction("EmployeeSystemAccessView", new { id = contact.UserInformationId });
            }
            return GetErrors();
        }
        private static string GetAcountActivityType(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation)
        {
            if (!string.IsNullOrEmpty(userContactInformation.LoginEmail) && !string.IsNullOrEmpty(model.LoginEmail) && userContactInformation.LoginEmail != model.LoginEmail)
                return "EmailIsChanged";
            else if (!string.IsNullOrEmpty(model.LoginEmail) && string.IsNullOrEmpty(userContactInformation.LoginEmail))
                return "NewRegistration";
            else if (string.IsNullOrEmpty(model.LoginEmail) && !string.IsNullOrEmpty(userContactInformation.LoginEmail))
                return "Removed";
            else if (!string.IsNullOrEmpty(model.LoginEmail) && userContactInformation.LoginEmail == model.LoginEmail)
                return "Reset";
            return "";
        }
        public virtual ActionResult EmployeeSystemAccessChangeHistory(int refrenceId)
        {
            try
            {
                AllowView();
                string type = "";
                //if (columnName == "EmployeeUserId")
                //    type = "Supervisor";
                //else
                //    type = "Supervised Employee";
                var refrenceUserContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == refrenceId);
                var refrenceUserContactInformationId = 0;
                if (refrenceUserContactInformation != null)
                    refrenceUserContactInformationId = refrenceUserContactInformation.Id;
                string[] tables = { "SupervisorCompany", "SupervisorDepartment", "SupervisorEmployeeType", "SupervisorSubDepartment", "UserEmployeeGroup", "UserInformationRole" };
                var entitySet1 = db.AuditLogDetail.Where(d => d.ColumnName == "UserInformationId" && d.NewValue == refrenceId.ToString() && tables.Contains(d.AuditLog.TableName))
                                  .Select(d => d.AuditLog).ToList();
                var entitySetLoginEmail = db.AuditLogDetail.Where(d => d.ColumnName == "LoginEmail" && d.AuditLog.ReferenceId == refrenceUserContactInformationId && d.AuditLog.TableName == "UserContactInformation")
                                  .Select(d => d.AuditLog).ToList();
                var entitySetChangePasswordByAdminReason = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == "ChangePasswordByAdminReason")
                                  .Select(d => d.AuditLog).ToList();

                var entitySetRegisterationEmail = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName.Contains("Account Registration"))
                                  .Select(d => d.AuditLog).ToList();

                var entitySet = entitySetRegisterationEmail.Union(entitySetChangePasswordByAdminReason.Union(entitySetLoginEmail.Union(entitySet1))).SelectMany(a => a.AuditLogDetail).Where(d => d.ColumnName != "UserInformationId").ToList();
                foreach (var each in entitySet.OrderBy(e => e.CreatedDate))
                {

                    if (each.AuditLog.TableName == "UserContactInformation")
                    {
                        if (!string.IsNullOrEmpty(each.NewValue) && !string.IsNullOrEmpty(each.OldValue))
                            each.ColumnName = each.ColumnName + " was modified old: " + each.OldValue + ", new :" + each.NewValue;
                        else if (!string.IsNullOrEmpty(each.OldValue))
                            each.ColumnName = each.ColumnName + " was Added: " + each.OldValue;
                        else if (!string.IsNullOrEmpty(each.NewValue))
                            each.ColumnName = each.ColumnName + " was Added: " + each.NewValue;
                    }
                    else if (each.AuditLog.TableName == "ChangePasswordByAdminReason")
                    {
                        each.AuditLog.TableName = "Change Password";
                        each.ColumnName = each.ColumnName + ": " + each.NewValue;
                    }
                    int entityId;
                    if (int.TryParse(each.NewValue, out entityId))
                    {
                        //db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                        if (each.AuditLog.TableName == "SupervisorCompany")
                        {
                            var user = db.Company.FirstOrDefault(u => u.Id == entityId);
                            each.ColumnName = user.CompanyName;
                        }
                        else if (each.AuditLog.TableName == "SupervisorDepartment")
                        {
                            var user = db.Department.FirstOrDefault(u => u.Id == entityId);
                            each.ColumnName = user.DepartmentName;
                        }
                        else if (each.AuditLog.TableName == "SupervisorEmployeeType")
                        {
                            var user = db.EmployeeType.FirstOrDefault(u => u.Id == entityId);
                            each.ColumnName = user.EmployeeTypeName;
                        }
                        else if (each.AuditLog.TableName == "SupervisorSubDepartment")
                        {
                            var user = db.SubDepartment.FirstOrDefault(u => u.Id == entityId);
                            each.ColumnName = user.SubDepartmentName;
                        }
                        else if (each.AuditLog.TableName == "UserEmployeeGroup")
                        {
                            var user = db.EmployeeGroup.FirstOrDefault(u => u.Id == entityId);
                            each.ColumnName = user.EmployeeGroupName;
                        }
                        else if (each.AuditLog.TableName == "UserInformationRole")
                        {
                            var user = db.Role.FirstOrDefault(u => u.Id == entityId);
                            each.ColumnName = user.RoleName;
                        }

                        //"", "", "", "", "" };
                    }
                    each.NewValue = each.AuditLog.TableName;
                    //if (columnName == "EmployeeUserId")
                    //    each.NewValue = "Supervisor";
                    //else
                    //    each.NewValue = "Supervised Employee";
                }
                var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceId);
                ViewBag.Title = type + " Change History for " + userInformation.FullName;
                return PartialView(entitySet.OrderBy(u => u.CreatedDate));
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        private void UpdateSupervisorSubDepartment(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation, TimeAideContext context)
        {
            var selectedSubDepartmentIds = (model.SelectedSubDepartmentId ?? "").Split(',').ToList();
            List<SupervisorSubDepartment> supervisorCompanyAddList = new List<SupervisorSubDepartment>();
            List<SupervisorSubDepartment> supervisorCompanyRemoveList = new List<SupervisorSubDepartment>();
            var existingSupervisorCompanyList = context.SupervisorSubDepartment.Where(w => w.UserInformationId == model.UserInformationId).ToList();

            foreach (var eachExisting in existingSupervisorCompanyList)
            {
                var RecCnt = selectedSubDepartmentIds.Where(w => w == eachExisting.SubDepartmentId.ToString()).Count();
                if (RecCnt == 0)
                {
                    supervisorCompanyRemoveList.Add(eachExisting);
                }

            }
            foreach (var eachSelected in selectedSubDepartmentIds)
            {
                if (eachSelected == "") continue;
                int subDepartmentId = int.Parse(eachSelected);
                var recExists = existingSupervisorCompanyList.Where(w => w.SubDepartmentId == subDepartmentId).Count();
                if (recExists == 0)
                {
                    supervisorCompanyAddList.Add(new SupervisorSubDepartment() { UserInformationId = model.UserInformationId, SubDepartmentId = subDepartmentId });

                }
            }

            context.SupervisorSubDepartment.RemoveRange(supervisorCompanyRemoveList);
            context.SupervisorSubDepartment.AddRange(supervisorCompanyAddList);
        }

        private void UpdateSupervisorDepartment(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation, TimeAideContext context)
        {
            var selectedDepartmentIds = (model.SelectedDepartmentId ?? "").Split(',').ToList();
            List<SupervisorDepartment> supervisorDepartmentAddList = new List<SupervisorDepartment>();
            List<SupervisorDepartment> supervisorDepartmentRemoveList = new List<SupervisorDepartment>();
            var existingSupervisorCompanyList = context.SupervisorDepartment.Where(w => w.UserInformationId == model.UserInformationId).ToList();

            foreach (var eachExisting in existingSupervisorCompanyList)
            {
                var RecCnt = selectedDepartmentIds.Where(w => w == eachExisting.DepartmentId.ToString()).Count();
                if (RecCnt == 0)
                {
                    supervisorDepartmentRemoveList.Add(eachExisting);
                }

            }
            foreach (var eachSelected in selectedDepartmentIds)
            {
                if (eachSelected == "") continue;
                int departmentId = int.Parse(eachSelected);
                var recExists = existingSupervisorCompanyList.Where(w => w.DepartmentId == departmentId).Count();
                if (recExists == 0)
                {
                    supervisorDepartmentAddList.Add(new SupervisorDepartment() { UserInformationId = model.UserInformationId, DepartmentId = departmentId });

                }
            }

            context.SupervisorDepartment.RemoveRange(supervisorDepartmentRemoveList);
            context.SupervisorDepartment.AddRange(supervisorDepartmentAddList);
        }

        private void UpdateSupervisorEmployeeType(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation, TimeAideContext context)
        {
            var selectedEmployeeTypeIds = (model.SelectedEmployeeTypeId ?? "").Split(',').ToList();
            List<SupervisorEmployeeType> supervisorEmployeeTypeAddList = new List<SupervisorEmployeeType>();
            List<SupervisorEmployeeType> supervisorEmployeeTypeRemoveList = new List<SupervisorEmployeeType>();
            var existingSupervisorEmployeeTypeList = context.SupervisorEmployeeType.Where(w => w.UserInformationId == model.UserInformationId).ToList();

            foreach (var eachExisting in existingSupervisorEmployeeTypeList)
            {
                var RecCnt = selectedEmployeeTypeIds.Where(w => w == eachExisting.EmployeeTypeId.ToString()).Count();
                if (RecCnt == 0)
                {
                    supervisorEmployeeTypeRemoveList.Add(eachExisting);
                }

            }
            foreach (var eachSelected in selectedEmployeeTypeIds)
            {
                if (eachSelected == "") continue;
                int employeeTypeId = int.Parse(eachSelected);
                var recExists = existingSupervisorEmployeeTypeList.Where(w => w.EmployeeTypeId == employeeTypeId).Count();
                if (recExists == 0)
                {
                    supervisorEmployeeTypeAddList.Add(new SupervisorEmployeeType() { UserInformationId = model.UserInformationId, EmployeeTypeId = employeeTypeId });

                }
            }

            context.SupervisorEmployeeType.RemoveRange(supervisorEmployeeTypeRemoveList);
            context.SupervisorEmployeeType.AddRange(supervisorEmployeeTypeAddList);
        }

        private void UpdateSupervisorCompany(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation, TimeAideContext context)
        {
            var selectedSupervisorCompanyIds = (model.SelectedCompanyId ?? "").Split(',').ToList();
            var userInformation = context.UserInformation.FirstOrDefault(w => w.Id == model.UserInformationId);
            if (model.SelectedCompanyId == "0")
            {
                userInformation.HasAllCompany = true;
                selectedSupervisorCompanyIds = context.Company.Where(w => w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId).Select(o => o.Id.ToString()).ToList();
            }
            else
                userInformation.HasAllCompany = false;
            List<SupervisorCompany> supervisorCompanyAddList = new List<SupervisorCompany>();
            List<SupervisorCompany> supervisorCompanyRemoveList = new List<SupervisorCompany>();
            var existingSupervisorCompanyList = context.SupervisorCompany.Where(w => w.UserInformationId == model.UserInformationId).ToList();

            foreach (var eachExisting in existingSupervisorCompanyList)
            {
                var RecCnt = selectedSupervisorCompanyIds.Where(w => w == eachExisting.CompanyId.ToString()).Count();
                if (RecCnt == 0)
                {
                    supervisorCompanyRemoveList.Add(eachExisting);
                }

            }
            foreach (var eachSelected in selectedSupervisorCompanyIds)
            {
                if (eachSelected == "") continue;
                int companyId = int.Parse(eachSelected);
                var recExists = existingSupervisorCompanyList.Where(w => w.CompanyId == companyId).Count();
                if (recExists == 0)
                {
                    supervisorCompanyAddList.Add(new SupervisorCompany() { UserInformationId = model.UserInformationId, CompanyId = companyId });

                }
            }

            context.SupervisorCompany.RemoveRange(supervisorCompanyRemoveList);
            context.SupervisorCompany.AddRange(supervisorCompanyAddList);
        }

        private void UpdateUserEmployeeGroups(EmployeePrivilegeViewModel model, bool addOnly, TimeAideContext context)
        {
            var selectedEmployeeGroupIds = (model.SelectedEmployeeGroupId ?? "").Split(',').ToList();
            List<UserEmployeeGroup> employeeGroupAddList = new List<UserEmployeeGroup>();
            List<UserEmployeeGroup> employeeGroupRemoveList = new List<UserEmployeeGroup>();
            var existingEmployeeGroupList = context.UserEmployeeGroup.Where(w => w.UserInformationId == model.UserInformationId).ToList();

            foreach (var eachExisting in existingEmployeeGroupList)
            {
                var RecCnt = selectedEmployeeGroupIds.Where(w => w == eachExisting.EmployeeGroupId.ToString()).Count();
                if (RecCnt == 0)
                {
                    employeeGroupRemoveList.Add(eachExisting);
                }

            }
            foreach (var eachSelected in selectedEmployeeGroupIds)
            {
                if (eachSelected == "") continue;
                int employeeGroupId = int.Parse(eachSelected);
                var recExists = existingEmployeeGroupList.Where(w => w.EmployeeGroupId == employeeGroupId).Count();
                if (recExists == 0)
                {
                    employeeGroupAddList.Add(new UserEmployeeGroup() { UserInformationId = model.UserInformationId, EmployeeGroupId = employeeGroupId });

                }
            }
            if (!addOnly)
                context.UserEmployeeGroup.RemoveRange(employeeGroupRemoveList);
            context.UserEmployeeGroup.AddRange(employeeGroupAddList);
        }

        private void ResendRegistrationEmail(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation, TimeAideContext context, bool completeRegistration)
        {
            if (!String.IsNullOrEmpty(userContactInformation.LoginEmail))
            {
                var UserInformation = context.UserInformation.FirstOrDefault(a => a.Id == userContactInformation.UserInformationId);
                UserInformationActivation userInformationActivation = GetActivationCode(userContactInformation, context);

                string pass = UserManagmentService.GenerateRandomPassword(null);
                ApplicationDbContext _identityContext = new ApplicationDbContext();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_identityContext));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
                if (!UserManagmentService.DeleteUser(model.LoginEmail))
                {
                    throw new Exception("There was an error while completing the request.");
                }
                ApplicationUser user = UserManagmentService.CreateUser(model.LoginEmail, pass, userManager);
                UserInformation.AspNetUserId = user.Id;
                context.SaveChanges();
                if (!String.IsNullOrEmpty(model.LoginEmail))
                {
                    Dictionary<string, string> toEmails = new Dictionary<string, string>();
                    toEmails.Add(model.LoginEmail, userContactInformation.UserInformation.FirstLastName);
                    user.ChangePassword = completeRegistration;
                    UserManagmentService.UpdateUser(user, userManager);
                    string emailType = completeRegistration ? "Manual Registration" : "Registration";
                    UtilityHelper.SendRegistrationEmail(toEmails, userInformationActivation.ActivationCode.ToString(), context, userContactInformation.UserInformationId, pass, emailType);
                }
            }
        }

        private static UserInformationActivation GetActivationCode(UserContactInformation userContactInformation, TimeAideContext context)
        {
            var userInformationActivation = new UserInformationActivation()
            {
                ActivationCode = Guid.NewGuid(),
                UserInformationId = userContactInformation.UserInformationId,
                CreatedBy = userContactInformation.UserInformationId,
                ClientId = SessionHelper.SelectedClientId
            };
            context.UserInformationActivation.Add(userInformationActivation);
            return userInformationActivation;
        }
        //private void UserRegistrationEmail(EmployeePrivilegeViewModel model, UserContactInformation userContactInformation, TimeAideContext context)
        //{
        //    if (!String.IsNullOrEmpty(userContactInformation.LoginEmail))
        //    {
        //        if (String.IsNullOrEmpty(userContactInformation.UserInformation.AspNetUserId))
        //        {
        //            UserInformationActivation userInformationActivation = new UserInformationActivation()
        //            {
        //                ActivationCode = Guid.NewGuid(),
        //                UserInformationId = userContactInformation.UserInformationId,
        //                CreatedBy = userContactInformation.UserInformationId,
        //                DataEntryStatus = 2
        //            };
        //            context.UserInformationActivation.Add(userInformationActivation);
        //            string pass = UserManagmentService.GenerateRandomPassword(null);
        //            UserManagmentService.CreateUser(model.LoginEmail, pass);
        //            context.SaveChanges();
        //            if (!String.IsNullOrEmpty(model.LoginEmail))
        //            {
        //                Dictionary<string, string> toEmails = new Dictionary<string, string>();
        //                toEmails.Add(model.LoginEmail, userContactInformation.UserInformation.FirstLastName);
        //                UtilityHelper.SendRegistrationEmail(toEmails, null, context, userContactInformation.UserInformationId, pass);
        //            }
        //        }
        //    }
        //}

        public ActionResult EmployeeSystemAccessView(int? id)
        {
            if (!SecurityHelper.IsAvailable("Profile"))
            {
                Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
            ViewBag.IsSeperateEmailNotification = false;
            var configuration = ApplicationConfigurationService.GetApplicationConfiguration("UseSeperateEmailNotification");
            if (configuration != null)
                ViewBag.IsSeperateEmailNotification = configuration.ValueAsBoolean;

            UserContactInformation userContactInformation = null;
            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);
            var userInformation = db.UserInformation.Find(id ?? 0);
            EmployeePrivilegeViewModel model = new EmployeePrivilegeViewModel();
            model.UserInformationId = id ?? 0;
            if (userInformation != null)
            {
                if (userInformation.UserInformationRole.Count > 0)
                {
                    model.RoleId = userInformation.UserInformationRole.FirstOrDefault().RoleId;
                    model.RoleTypeId = userInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId ?? 0;
                    model.Role = userInformation.UserInformationRole.FirstOrDefault().Role;
                }
                model.EmployeeStatusId = userInformation.EmployeeStatusId;
            }
            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                userContactInformation.UserInformationId = id ?? 0;
                ViewBag.Label = ViewBag.Label + " - Add";
            }
            else
            {

                model.Id = userContactInformation.Id;
                model.UserInformationId = userContactInformation.UserInformationId;
                model.LoginEmail = userContactInformation.LoginEmail;
                model.NotificationEmail = userContactInformation.NotificationEmail;
                ViewBag.Label = ViewBag.Label + " - Edit";


                model.UserInformationActivation = userContactInformation.UserInformation.UserInformationActivation.OrderByDescending(a => a.Id).FirstOrDefault();
                //if (userInformation.UserInformationRole.Count > 0)
                //{
                //    model.RoleId = userContactInformation.UserInformation.UserInformationRole.FirstOrDefault().RoleId;
                //    model.RoleTypeId = userContactInformation.UserInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId ?? 0;
                //    model.Role = userContactInformation.UserInformation.UserInformationRole.FirstOrDefault().Role;
                //}


                var emailList = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(userContactInformation.WorkEmail))
                    emailList.Add(new SelectListItem() { Text = userContactInformation.WorkEmail, Value = userContactInformation.WorkEmail });
                if (!string.IsNullOrEmpty(userContactInformation.PersonalEmail))
                    emailList.Add(new SelectListItem() { Text = userContactInformation.PersonalEmail, Value = userContactInformation.PersonalEmail });
                if (!string.IsNullOrEmpty(userContactInformation.OtherEmail))
                    emailList.Add(new SelectListItem() { Text = userContactInformation.OtherEmail, Value = userContactInformation.OtherEmail });
                
                ViewBag.LoginEmailAddressTypeId = new SelectList(emailList, "Value", "Text", userContactInformation.LoginEmail);
                ViewBag.NotificationEmailAddressTypeId = new SelectList(emailList, "Value", "Text", userContactInformation.NotificationEmail);
                
                model.SelectedCompanyId = String.Join(", ", db.SupervisorCompany.Where(w => w.DataEntryStatus == 1 && w.Company.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.Company.CompanyName.ToString()));
                var data = db.UserEmployeeGroup.ToList();
                model.SelectedEmployeeGroupId = String.Join(", ", db.UserEmployeeGroup.Where(w => w.DataEntryStatus == 1 && w.EmployeeGroup.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.EmployeeGroup.EmployeeGroupName.ToString()));
                model.SelectedEmployeeTypeId = String.Join(", ", db.SupervisorEmployeeType.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).Select(c => c.EmployeeType.EmployeeTypeName.ToString()));
                model.SelectedSubDepartmentId = String.Join(", ", db.SupervisorSubDepartment.Where(w => w.DataEntryStatus == 1 && w.SubDepartment.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.SubDepartment.SubDepartmentName.ToString()));
                model.SelectedDepartmentId = String.Join(", ", db.SupervisorDepartment.Where(w => w.DataEntryStatus == 1 && w.Department.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.Department.DepartmentName.ToString()));
                //ViewBag.ClientId = new SelectList(db.Client.Where(w => w.DataEntryStatus == 1), "Id", "ClientName");
                ViewBag.CompanyId = new SelectList(db.Company.Where(w => w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId), "Id", "CompanyName");
                ViewBag.EmployeeTypeId = new SelectList(db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeName");
                ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName");
                ViewBag.SubDepartmentId = new SelectList(SubDepartmentService.SubDepartments(null), "Id", "SubDepartmentName");

                model.HasAllCompany = userInformation.HasAllCompany;
                //UserInformation userInformation = db.UserInformation.FirstOrDefault(u => u.Id == id);

                var supervisors = db.EmployeeSupervisor.Where(u => u.SupervisorUserId == id && u.DataEntryStatus == 1).Select(e => e.EmployeeUser).ToList();
                model.SupervisedEmployee = supervisors;
                model.SupervisedUserId = new List<string>();
                model.LockAccount = UserManagmentService.IsLockout(userContactInformation.LoginEmail);


            }

            return PartialView(model);
        }
        public ActionResult EmployeeActivationAccessView(int? id)
        {
            UserContactInformation userContactInformation = null;
            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);
            var userInformation = db.UserInformation.Find(id ?? 0);
            EmployeePrivilegeViewModel model = new EmployeePrivilegeViewModel();
            model.UserInformationId = id ?? 0;
            if (userInformation != null)
            {
                if (userInformation.UserInformationRole.Count > 0)
                {
                    model.RoleId = userInformation.UserInformationRole.FirstOrDefault().RoleId;
                    model.RoleTypeId = userInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId ?? 0;
                    model.Role = userInformation.UserInformationRole.FirstOrDefault().Role;
                }
            }
            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                userContactInformation.UserInformationId = id ?? 0;

            }
            else
            {
                model.Id = userContactInformation.Id;
                model.UserInformationId = userContactInformation.UserInformationId;
                model.LoginEmail = userContactInformation.LoginEmail;
                model.UserInformationActivation = userContactInformation.UserInformation.UserInformationActivation.OrderByDescending(a => a.Id).FirstOrDefault();


            }
            ViewBag.RoleId = new SelectList(db.Role.Where(w => w.RoleTypeId == model.RoleTypeId).ToList(), "Id", "RoleName", model.RoleId);
            var emailList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(userContactInformation.WorkEmail))
                emailList.Add(new SelectListItem() { Text = userContactInformation.WorkEmail, Value = userContactInformation.WorkEmail });
            if (!string.IsNullOrEmpty(userContactInformation.PersonalEmail))
                emailList.Add(new SelectListItem() { Text = userContactInformation.PersonalEmail, Value = userContactInformation.PersonalEmail });
            if (!string.IsNullOrEmpty(userContactInformation.OtherEmail))
                emailList.Add(new SelectListItem() { Text = userContactInformation.OtherEmail, Value = userContactInformation.OtherEmail });
            ViewBag.LoginEmailAddressTypeId = new SelectList(emailList, "Value", "Text", model.LoginEmail);

            return PartialView(model);
        }

        public ActionResult EmployeeReportingHierarchyView(int? id)
        {
            UserContactInformation userContactInformation = null;
            userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.UserInformationId == id);

            EmployeePrivilegeViewModel model = new EmployeePrivilegeViewModel();
            model.UserInformationId = id ?? 0;

            if (userContactInformation == null)
            {
                userContactInformation = new UserContactInformation();
                userContactInformation.UserInformationId = id ?? 0;
                userContactInformation = new UserContactInformation();
                ViewBag.Label = ViewBag.Label + " - Add";
            }
            else
            {

                model.Id = userContactInformation.Id;
                model.UserInformationId = userContactInformation.UserInformationId;
                model.LoginEmail = userContactInformation.LoginEmail;
                if (userContactInformation.UserInformation.UserInformationRole.Count > 0)
                {
                    model.RoleId = userContactInformation.UserInformation.UserInformationRole.FirstOrDefault().RoleId;
                    model.RoleTypeId = userContactInformation.UserInformation.UserInformationRole.FirstOrDefault().Role.RoleTypeId ?? 0;
                    model.Role = userContactInformation.UserInformation.UserInformationRole.FirstOrDefault().Role;
                }

                ViewBag.Label = ViewBag.Label + " - Edit";
                var emailList = new List<SelectListItem>();
                if (!string.IsNullOrEmpty(userContactInformation.WorkEmail))
                    emailList.Add(new SelectListItem() { Text = userContactInformation.WorkEmail, Value = userContactInformation.WorkEmail });
                if (!string.IsNullOrEmpty(userContactInformation.PersonalEmail))
                    emailList.Add(new SelectListItem() { Text = userContactInformation.PersonalEmail, Value = userContactInformation.PersonalEmail });
                if (!string.IsNullOrEmpty(userContactInformation.OtherEmail))
                    emailList.Add(new SelectListItem() { Text = userContactInformation.OtherEmail, Value = userContactInformation.OtherEmail });
                ViewBag.LoginEmailAddressTypeId = new SelectList(emailList, "Value", "Text", userContactInformation.LoginEmail);

                model.SelectedCompanyId = String.Join(",", db.SupervisorCompany.Where(w => w.DataEntryStatus == 1 && w.Company.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.Company.CompanyName.ToString()));
                model.SelectedEmployeeGroupId = String.Join(",", db.UserEmployeeGroup.Where(w => w.DataEntryStatus == 1 && w.EmployeeGroup.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.EmployeeGroup.EmployeeGroupName.ToString()));
                model.SelectedEmployeeTypeId = String.Join(",", db.SupervisorEmployeeType.Where(w => w.DataEntryStatus == 1 && w.UserInformationId == id).Select(c => c.EmployeeType.EmployeeTypeName.ToString()));
                model.SelectedSubDepartmentId = String.Join(",", db.SupervisorSubDepartment.Where(w => w.DataEntryStatus == 1 && w.SubDepartment.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.SubDepartment.SubDepartmentName.ToString()));
                model.SelectedDepartmentId = String.Join(",", db.SupervisorDepartment.Where(w => w.DataEntryStatus == 1 && w.Department.ClientId == SessionHelper.SelectedClientId && w.UserInformationId == id).Select(c => c.Department.DepartmentName.ToString()));
                //ViewBag.ClientId = new SelectList(db.Client.Where(w => w.DataEntryStatus == 1), "Id", "ClientName");
                ViewBag.CompanyId = new SelectList(db.Company.Where(w => w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId), "Id", "CompanyName");
                ViewBag.EmployeeTypeId = new SelectList(db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeTypeName");
                ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName");
                ViewBag.SubDepartmentId = new SelectList(SubDepartmentService.SubDepartments(null), "Id", "SubDepartmentName");


                UserInformation userInformation = db.UserInformation.FirstOrDefault(u => u.Id == id);

                var supervisors = db.EmployeeSupervisor.Where(u => u.SupervisorUserId == id && u.DataEntryStatus == 1).Select(e => e.EmployeeUser).ToList();
                //List<SelectListItem> supervisorsItems = new List<SelectListItem>();
                //foreach (var team in supervisors)
                //{
                //    supervisorsItems.Add(new SelectListItem { Value = team.EmployeeUserId.ToString(), Text = team.EmployeeUser.FullName });
                //}
                model.SupervisedEmployee = supervisors;


                var supervisors2 = db.EmployeeSupervisor.Where(u => u.EmployeeUserId == id && u.DataEntryStatus == 1).Select(e => e.SupervisorUser).ToList();
                model.EmployeeSupervisor = supervisors2;
                model.SupervisedUserId = new List<string>();
            }



            return PartialView(model);
        }

        [HttpPost]
        public ActionResult SetSelected(Client client)
        {
            TimeAide.Common.Helpers.SessionHelper.SelectedClientId = client.Id;
            var companies = db.Company.Where(c => c.ClientId == client.Id && c.DataEntryStatus == 1);
            TimeAide.Common.Helpers.SessionHelper.IsTimeAideWindowClient = (DataHelper.GetSelectedClientTAWinEFContext() == null) ? false : true;
            TimeAide.Common.Helpers.SessionHelper.UserFilterInSession = null;
            UtilityHelper.UserSessionLogDetail("UserInformationController", "Change Client");
            if (companies.Count() > 0)
            {
                SessionHelper.SelectedCompanyId = companies.FirstOrDefault().Id;
                SessionHelper.SelectedCompanyId_Old = companies.FirstOrDefault().Old_Id ?? 0;
            }
            return Json(client);
        }

        [HttpPost]
        public ActionResult SetSelectedCompany(Company company)
        {
            var redirectTo = privileges.AllowView ? "UserInformation" : "CompanyPortal";
            try
            {
                UtilityHelper.UserSessionLogDetail("UserInformationController", "Change Company");
                TimeAide.Common.Helpers.SessionHelper.SelectedCompanyId = company.Id;
                TimeAide.Common.Helpers.SessionHelper.SelectedCompanyId_Old = company.Old_Id ?? 0;
                var company1 = db.Company.FirstOrDefault(c => c.Id == company.Id);
                TimeAide.Common.Helpers.SessionHelper.SelectedClientId = company1.ClientId ?? 0;
                TimeAide.Common.Helpers.SessionHelper.UserFilterInSession = null;
                TimeAide.Common.Helpers.SessionHelper.IsTimeAideWindowClient = (DataHelper.GetSelectedClientTAWinEFContext() == null) ? false : true;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
            }
            return Json($"Success,{redirectTo}");
        }

        [HttpPost]
        public JsonResult SetSelectedCompanyByGlobalEmployeeSearch(Company company)
        {
           
            try
            {
                UtilityHelper.UserSessionLogDetail("UserInformationController", "Change Company");
                TimeAide.Common.Helpers.SessionHelper.SelectedCompanyId = company.Id;
                TimeAide.Common.Helpers.SessionHelper.SelectedCompanyId_Old = company.Old_Id ?? 0;
                var company1 = db.Company.FirstOrDefault(c => c.Id == company.Id);
                TimeAide.Common.Helpers.SessionHelper.SelectedClientId = company1.ClientId ?? 0;
                TimeAide.Common.Helpers.SessionHelper.UserFilterInSession = null;
                TimeAide.Common.Helpers.SessionHelper.IsTimeAideWindowClient = (DataHelper.GetSelectedClientTAWinEFContext() == null) ? false : true;
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
            }
            return Json($"{company.CompanyName}");
        }


        [HttpPost]
        public ActionResult ResendEmail(EmployeePrivilegeViewModel viewModel)
        {
            UserContactInformation userContactInformation = db.UserContactInformation.FirstOrDefault(u => u.Id == viewModel.UserInformationId);
            UserInformationActivation userInformationActivation = new UserInformationActivation()
            {
                ActivationCode = Guid.NewGuid(),
                UserInformationId = userContactInformation.UserInformationId,
                CreatedBy = userContactInformation.UserInformationId,
                ClientId = SessionHelper.SelectedClientId
            };
            db.UserInformationActivation.Add(userInformationActivation);
            db.SaveChanges();
            if (!String.IsNullOrEmpty(userContactInformation.LoginEmail))
            {
                Dictionary<string, string> toEmails = new Dictionary<string, string>();
                toEmails.Add(userContactInformation.LoginEmail, userContactInformation.UserInformation.FirstLastName);
                UtilityHelper.SendRegistrationEmail(toEmails, userInformationActivation.ActivationCode.ToString(), db, userContactInformation.UserInformationId, null, "Registration");
            }
            return Json("success");
        }

        [HttpGet]
        public ActionResult GetChangeRequestHistoryIndex(UserInformation model)
        {
            var userInformation = db.Find<UserInformation>(model.Id, SessionHelper.SelectedClientId);
            return PartialView("EmployeeProfile/PersonalInfo/_ChangeRequestAddressList", userInformation);
        }

        public virtual ActionResult ChangeHistory(int refrenceId)
        {
            try
            {
                AllowView();
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId).ToList();
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from UserInformation where UserInformationId = " + refrenceId.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = "UserInformation";
                    ViewBag.RefrenceId = refrenceId;
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                return PartialView("~/Views/AuditLog/ChangeHistory.cshtml", entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Emergency Contact", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
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
}
