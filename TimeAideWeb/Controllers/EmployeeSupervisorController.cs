using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services.Helpers;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using Microsoft.Office.Interop.Excel;

namespace TimeAide.Controllers
{
    public class EmployeeSupervisorController : TimeAideWebControllers<EmployeeSupervisor>
    {

        // GET: EmployeeSupervisor/Create
        public override ActionResult Create()
        {
            //ViewBag.EmployeeUserId = new SelectList(db.UserInformation, "UserId", "FullName");
            //ViewBag.SupervisorUserId = new SelectList(db.UserInformation, "UserId", "FullName");

            var users = db.UserInformation.ToList();
            //ViewBag.Users = new MultiSelectList(users, "UserId", "FullName");

            List<SelectListItem> items = new List<SelectListItem>();

            // Loop over each team in our teams List...
            foreach (var team in users)
            {
                // ... and instantiate a new SelectListItem for each one...
                var item = new SelectListItem
                {
                    Value = team.Id.ToString(),
                    Text = team.FullName
                };
                // ... then add to our items List
                items.Add(item);
            };

            var model = new EmployeeSupervisorViewModel();
            model.Users = new MultiSelectList(items.OrderBy(i => i.Text), "Value", "Text"); // new MultiSelectList(users, "UserId", "FullName");
            model.AllUserId = new List<string>();
            List<SelectListItem> items2 = new List<SelectListItem>();
            //items2.Add(new SelectListItem() { Text = "Name", Value = "Name" });
            model.SupervisedUsers = new MultiSelectList(items2.OrderBy(i => i.Text), "Value", "Text");
            model.SupervisedUserId = new List<string>();

            return View(model);
        }

        // POST: EmployeeSupervisor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SupervisedUsers,Users,SupervisedUserId,UserId")] EmployeeSupervisorViewModel employeeSupervisor)
        {
            if (ModelState.IsValid)
            {
                //db.EmployeeSupervisor.Add(employeeSupervisor);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.EmployeeUserId = new SelectList(db.UserInformation, "UserId", "FullName", employeeSupervisor.EmployeeUserId);
            //ViewBag.SupervisorUserId = new SelectList(db.UserInformation, "UserId", "FullName", employeeSupervisor.SupervisorUserId);
            return View(employeeSupervisor);
        }

        // GET: EmployeeSupervisor/Edit/5
        public override ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeSupervisor employeeSupervisor = db.EmployeeSupervisor.Find(id);
            if (employeeSupervisor == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeUserId = new SelectList(db.UserInformation, "UserId", "FullName", employeeSupervisor.EmployeeUserId);
            ViewBag.SupervisorUserId = new SelectList(db.UserInformation, "UserId", "FullName", employeeSupervisor.SupervisorUserId);
            return View(employeeSupervisor);
        }

        // POST: EmployeeSupervisor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeUserId,SupervisorUserId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,DataEntryStatus")] EmployeeSupervisor employeeSupervisor)
        {
            if (ModelState.IsValid)
            {
                employeeSupervisor.ModifiedBy = SessionHelper.LoginId;
                employeeSupervisor.ModifiedDate = DateTime.Now;
                db.Entry(employeeSupervisor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeUserId = new SelectList(db.UserInformation, "UserId", "FullName", employeeSupervisor.EmployeeUserId);
            ViewBag.SupervisorUserId = new SelectList(db.UserInformation, "UserId", "FullName", employeeSupervisor.SupervisorUserId);
            return View(employeeSupervisor);
        }

        public ActionResult CreateEdit(int? userId)
        {
            ViewBag.EmployeeSupervisorId = userId;
            var  selectedSupervisorListObject = db.GetAll<EmployeeSupervisor>(SessionHelper.SelectedClientId).Where(e => e.EmployeeUserId == userId);
            ViewBag.SelectedSupervisorListObject = selectedSupervisorListObject;


            ViewBag.SupervisorListObject = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.UserEmployeeGroup.Count > 0 && u.DataEntryStatus == 1 && u.Id != userId && u.UserEmployeeGroup.Any(g => g.EmployeeGroup.EmployeeGroupTypeId == 2) && !selectedSupervisorListObject.Any(y => y.SupervisorUserId == u.Id));
            
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedCredentialIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedCredentialIds.Split(',').ToList();
                List<EmployeeSupervisor> credentialAddList = new List<EmployeeSupervisor>();
                List<EmployeeSupervisor> credentialRemoveList = new List<EmployeeSupervisor>();
                var existingCredentialList = db.EmployeeSupervisor.Where(w => w.EmployeeUserId == id).ToList();

                foreach (var credentialItem in existingCredentialList)
                {
                    var RecCnt = selectedCredentialsList.Where(w => w == credentialItem.SupervisorUserId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        credentialRemoveList.Add(credentialItem);
                    }

                }
                foreach (var selectedCredentialId in selectedCredentialsList)
                {
                    if (selectedCredentialId == "") continue;
                    int credentialId = int.Parse(selectedCredentialId);
                    var recExists = existingCredentialList.Where(w => w.SupervisorUserId == credentialId).Count();
                    if (recExists == 0)
                    {
                        credentialAddList.Add(new EmployeeSupervisor() { SupervisorUserId = credentialId, EmployeeUserId = id });

                    }
                }

                db.EmployeeSupervisor.RemoveRange(credentialRemoveList);
                db.EmployeeSupervisor.AddRange(credentialAddList);

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public ActionResult CreateEdit1(int? userId)
        {
            ViewBag.EmployeeSupervisorId = userId;

            var selectedSupervisorListObject = db.GetAll<EmployeeSupervisor>(SessionHelper.SelectedClientId).Where(e => e.EmployeeUserId == userId);
            ViewBag.SelectedSupervisorListObject = selectedSupervisorListObject;

            ViewBag.SupervisorListObject = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.UserEmployeeGroup.Count > 0 && u.DataEntryStatus == 1 && u.Id != userId && u.UserEmployeeGroup.Any(g => g.EmployeeGroup.EmployeeGroupTypeId == 2) && !selectedSupervisorListObject.Any(y => y.SupervisorUserId == u.Id));




            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit1(int id, string selectedCredentialIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedCredentialIds.Split(',').ToList();
                List<EmployeeSupervisor> credentialAddList = new List<EmployeeSupervisor>();
                List<EmployeeSupervisor> credentialRemoveList = new List<EmployeeSupervisor>();
                var existingCredentialList = db.EmployeeSupervisor.Where(w => w.EmployeeUserId == id).ToList();

                foreach (var credentialItem in existingCredentialList)
                {
                    var RecCnt = selectedCredentialsList.Where(w => w == credentialItem.SupervisorUserId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        credentialRemoveList.Add(credentialItem);
                    }

                }
                foreach (var selectedCredentialId in selectedCredentialsList)
                {
                    if (selectedCredentialId == "") continue;
                    int credentialId = int.Parse(selectedCredentialId);
                    var recExists = existingCredentialList.Where(w => w.SupervisorUserId == credentialId).Count();
                    if (recExists == 0)
                    {
                        credentialAddList.Add(new EmployeeSupervisor() { SupervisorUserId = credentialId, EmployeeUserId = id });

                    }
                }
                db.EmployeeSupervisor.RemoveRange(credentialRemoveList);
                db.EmployeeSupervisor.AddRange(credentialAddList);
                db.SaveChanges();

                existingCredentialList = existingCredentialList.Except(credentialRemoveList).ToList();
                foreach (var item in credentialAddList) existingCredentialList.Add(item);
                List<UserInformationViewModel> newSupervisorList = new List<UserInformationViewModel>();
                foreach (var item in existingCredentialList)
                {
                    UserInformationViewModel userInformationViewModel = new UserInformationViewModel();
                    userInformationViewModel.Id = item.SupervisorUserId;
                    var supervisorUser = db.UserInformation.FirstOrDefault(u => u.Id == item.SupervisorUserId);
                    userInformationViewModel.FirstName = supervisorUser.FirstName;
                    userInformationViewModel.FirstLastName = supervisorUser.FirstLastName;
                    userInformationViewModel.SecondLastName = supervisorUser.SecondLastName;

                    var activeEmployment = TimeAide.Services.EmploymentService.GetActiveEmployment(item.SupervisorUserId);
                    TimeAide.Web.Models.EmploymentHistory activeEmploymentHistory = null;
                    if (activeEmployment != null)
                    {
                        activeEmploymentHistory = TimeAide.Services.EmploymentHistoryService.GetActiveEmploymentHistory(item.SupervisorUserId, activeEmployment.Id);

                    }
                    if (supervisorUser.Department != null)
                    {
                        userInformationViewModel.DepartmentName = supervisorUser.Department.DepartmentName;
                    }
                        
                    else if(activeEmploymentHistory != null && activeEmploymentHistory.Department != null)
                    {
                        userInformationViewModel.DepartmentName = activeEmploymentHistory.Department.DepartmentName;
                    }
                       
                    if (supervisorUser.Position != null)
                    {
                        userInformationViewModel.PositionName = supervisorUser.Position.PositionName;

                    }
                    else if (activeEmploymentHistory != null && activeEmploymentHistory.Position != null)
                    {
                        userInformationViewModel.PositionName = activeEmploymentHistory.Position.PositionName;
                    }

                    if (userInformationViewModel.DepartmentName == null)
                        userInformationViewModel.DepartmentName = "";
                    if (userInformationViewModel.PositionName == null)
                        userInformationViewModel.PositionName = "";


                    newSupervisorList.Add(userInformationViewModel);
                }
                //existingCredentialList.AsEnumerable<EmployeeSupervisor>().RemoveRange(credentialRemoveList);
                //db.EmployeeSupervisor.AddRange(credentialAddList);
                //db.SaveChanges();

                return Json(new { newSupervisorList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }


        public ActionResult CreateEditSupervisedEmplyees(int? userId)
        {
            ViewBag.EmployeeSupervisorId = userId;
            var selectedSupervisorListObject = db.GetAll<EmployeeSupervisor>(SessionHelper.SelectedClientId).Where(e => e.SupervisorUserId == userId);
            ViewBag.SelectedSupervisorListObject = selectedSupervisorListObject;
            ViewBag.SupervisorListObject = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.DataEntryStatus == 1 && u.Id != userId && !selectedSupervisorListObject.Any(y => y.EmployeeUserId == u.Id));
            //ViewBag.SupervisorListObject = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == Convert.ToInt32(EmployeeGroupTypes.Supervisor) && u.UserInformationId != userId);

            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEditSupervisedEmplyees(int id, string selectedSupervisorIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedSupervisorIds.Split(',').ToList();
                List<EmployeeSupervisor> supervisorAddList = new List<EmployeeSupervisor>();
                List<EmployeeSupervisor> supervisorRemoveList = new List<EmployeeSupervisor>();
                var existingSupervisorList = db.EmployeeSupervisor.Where(w => w.SupervisorUserId == id).ToList();

                foreach (var credentialItem in existingSupervisorList)
                {
                    var RecCnt = selectedCredentialsList.Where(w => w == credentialItem.EmployeeUserId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        supervisorRemoveList.Add(credentialItem);
                    }

                }
                foreach (var selectedCredentialId in selectedCredentialsList)
                {
                    if (selectedCredentialId == "") continue;
                    int credentialId = int.Parse(selectedCredentialId);
                    var recExists = existingSupervisorList.Where(w => w.EmployeeUserId == credentialId).Count();
                    if (recExists == 0)
                    {
                        supervisorAddList.Add(new EmployeeSupervisor() { SupervisorUserId = id, EmployeeUserId = credentialId });

                    }
                }

                db.EmployeeSupervisor.RemoveRange(supervisorRemoveList);
                db.EmployeeSupervisor.AddRange(supervisorAddList);

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }


        public ActionResult SelectEmplyees(int? userId)
        {

            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName").OrderBy(o => o.Text);
            ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus.Where(w => w.DataEntryStatus == 1), "Id", "EmployeeStatusName").OrderBy(o => o.Text);


            ViewBag.EmployeeSupervisorId = userId;
            ViewBag.SelectedSupervisorListObject = new List<UserInformation>();
            ViewBag.SupervisorListObject = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.DataEntryStatus == 1 && u.EmployeeStatusId == 1);
            //ViewBag.SupervisorListObject = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == Convert.ToInt32(EmployeeGroupTypes.Supervisor) && u.UserInformationId != userId);

            return PartialView();
        }

        [HttpPost]
        public JsonResult SelectEmplyees(int id, string selectedSupervisorIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedSupervisorIds.Split(',').ToList();

                var employeeList = db.SP_UserInformation<UserInformationViewModel>(0, "", 0, 0, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).Where(u => selectedCredentialsList.Contains(u.Id.ToString()));
                List<UserInformationViewModel> employeeList2 = GetSelectedEmployeedModel(employeeList);

                return Json(employeeList2);

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public ActionResult SelectEmailBlastEmplyees(int? userId)
        {

            ViewBag.DepartmentId = new SelectList(db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "DepartmentName").OrderBy(o => o.Text);
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName").OrderBy(o => o.Text);
            ViewBag.EmployeeStatusId = new SelectList(db.EmployeeStatus.Where(w => w.DataEntryStatus == 1), "Id", "EmployeeStatusName").OrderBy(o => o.Text);
            ViewBag.EmployeeSupervisorId = userId;
            //ViewBag.SupervisorListObject = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == Convert.ToInt32(EmployeeGroupTypes.Supervisor) && u.UserInformationId != userId);

            return PartialView();
        }

        [HttpPost]
        public JsonResult SelectEmailBlastEmplyees(int id, string selectedSupervisorIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedCredentialsList = selectedSupervisorIds.Split(',').ToList();

                var employeeList = db.SP_UserInformation<UserInformationViewModel>(0, "", 0, 0, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId).Where(u => selectedCredentialsList.Contains(u.Id.ToString()));
                List<UserInformationViewModel> employeeList2 = GetSelectedEmployeedModel(employeeList);

                return Json(employeeList2);

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
        public virtual ActionResult SelectEmailBlastEmplyeeList(EmailBlastEmployeesFilterViewModel filterModel)
        {
            try
            {
                var employeeList = db.SP_UserInformation<UserInformationViewModel>(0, "", filterModel.PositionId ?? 0, filterModel.EmployeeStatusId ?? 0, SessionHelper.LoginId, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);
                if (filterModel != null && filterModel.DepartmentId.HasValue)
                {
                    employeeList = employeeList.Where(w => w.DepartmentId == filterModel.DepartmentId).ToList();
                }
                ViewBag.SelectedSupervisorListObject = new List<UserInformation>();
                ViewBag.SupervisorListObject = employeeList.ToList();

                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        private List<UserInformationViewModel> GetSelectedEmployeedModel(IEnumerable<UserInformationViewModel> employeeList)
        {
            var employeeList2 = new List<UserInformationViewModel>();
            foreach (var row in employeeList)
            {
                var employee = new UserInformationViewModel()
                {
                    Id = row.Id,
                    FirstLastName = row.FirstLastName,
                    FirstName = row.FirstName,
                    SecondLastName = row.SecondLastName,
                    ShortFullName = row.ShortFullName,
                    EmployeeId = row.EmployeeId,
                    DepartmentName = row.DepartmentName,
                    EmployeeTypeName = row.EmployeeTypeName,
                    PositionName = row.PositionName,

                    UserRole = TimeAide.Web.Extensions.UserManagmentService.GetUserRole(row.Id),
                    EmployeeGroups = TimeAide.Web.Extensions.UserManagmentService.GetUserEmployeeGroups(row.Id)
                };
                var userContactInformation = db.UserContactInformation.FirstOrDefault(c => c.UserInformationId == row.Id);
                if (userContactInformation != null)
                {
                    if (!String.IsNullOrEmpty(userContactInformation.WorkEmail))
                        employee.ListOfEmails.Add("WorkEmail", userContactInformation.WorkEmail);
                    if (!String.IsNullOrEmpty(userContactInformation.PersonalEmail))
                        employee.ListOfEmails.Add("PersonalEmail", userContactInformation.PersonalEmail);
                    if (!String.IsNullOrEmpty(userContactInformation.OtherEmail))
                        employee.ListOfEmails.Add("OtherEmail", userContactInformation.OtherEmail);
                    employee.LoginEmail = userContactInformation.LoginEmail;
                    employee.NotificationEmail = userContactInformation.NotificationEmail;
                }
                employeeList2.Add(employee);
            }

            return employeeList2;
        }
        private UserInformationViewModel GetSelectedEmployeedModel(string employeeId)
        {
            var row = db.UserInformation.FirstOrDefault(u => u.EmployeeId.ToString() == employeeId && u.ClientId == SessionHelper.SelectedClientId);
            if (row != null)
            {
                var activeEmployment = TimeAide.Services.EmploymentService.GetActiveEmployment(row.Id);
                EmploymentHistory activeEmploymentHistory = null;
                if (activeEmployment != null)
                {
                    activeEmploymentHistory = TimeAide.Services.EmploymentHistoryService.GetActiveEmploymentHistory(row.Id, activeEmployment.Id);
                }

                var employee = new UserInformationViewModel()
                {
                    Id = row.Id,
                    FirstLastName = row.FirstLastName,
                    FirstName = row.FirstName,
                    SecondLastName = row.SecondLastName,
                    ShortFullName = row.ShortFullName,
                    EmployeeId = row.EmployeeId,
                    //DepartmentName = row.DepartmentName,
                    //EmployeeTypeName = row.EmployeeTypeName,
                    //PositionName = row.PositionName,

                    UserRole = TimeAide.Web.Extensions.UserManagmentService.GetUserRole(row.Id),
                    EmployeeGroups = TimeAide.Web.Extensions.UserManagmentService.GetUserEmployeeGroups(row.Id)
                };
                if (activeEmploymentHistory != null)
                {
                    if (activeEmploymentHistory.Department != null)
                        employee.DepartmentName = activeEmploymentHistory.Department.DepartmentName;
                    if (activeEmploymentHistory.SubDepartment != null)
                        employee.EmployeeTypeName = activeEmploymentHistory.SubDepartment.SubDepartmentName;
                    if (activeEmploymentHistory.Position != null)
                        employee.PositionName = activeEmploymentHistory.Position.PositionName;
                }
                var userContactInformation = db.UserContactInformation.FirstOrDefault(c => c.UserInformationId == row.Id);
                if (userContactInformation != null)
                {
                    if (!String.IsNullOrEmpty(userContactInformation.WorkEmail))
                        employee.ListOfEmails.Add("WorkEmail", userContactInformation.WorkEmail);
                    if (!String.IsNullOrEmpty(userContactInformation.PersonalEmail))
                        employee.ListOfEmails.Add("PersonalEmail", userContactInformation.PersonalEmail);
                    if (!String.IsNullOrEmpty(userContactInformation.OtherEmail))
                        employee.ListOfEmails.Add("OtherEmail", userContactInformation.OtherEmail);
                    employee.LoginEmail = userContactInformation.LoginEmail;
                    employee.NotificationEmail = userContactInformation.NotificationEmail;
                }

                return employee;
            }
            return null;
        }

        [HttpPost]
        public ActionResult SelectEmplyeesFromExcelFile()
        {
            HttpPostedFileBase postedFile = Request.Files[0];

            string filePath = string.Empty;
            if (postedFile != null)
            {
                //string path = Server.MapPath("~/Uploads/");
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                filePath = Path.GetTempFileName();
                string folder = Path.GetTempPath();
                postedFile.SaveAs(filePath);
                //ReadExcel(filePath);
                var data = ReadExcelUsingOledb(filePath, extension);
                //var selectedCredentialsList = employeeIds.Split(',').ToList();
                //var employeeListQ = db.SP_UserInformation<UserInformationViewModel>(0, "", 0, 1, 1, SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId);
                //var employeeList = employeeListQ.Where(u => selectedCredentialsList.Contains(u.EmployeeId.ToString()));
                List<UserInformationViewModel> employeeList2 = new List<UserInformationViewModel>();
                string listNotFound = "";
                foreach (DataRow each in data.Rows)
                {
                    var e = GetSelectedEmployeedModel(each["EmployeeId"].ToString());
                    if (e == null)
                        listNotFound += each + ",";
                    else
                    {
                        e.LoginEmail = each["LoginEmail"].ToString();
                        employeeList2.Add(e);
                    }
                }


                //foreach (var each in selectedCredentialsList)
                //{
                //    if (!employeeList.Any(u => u.EmployeeId.ToString() == each))
                //    {

                //    }
                //}

                return Json(new { SelectedEmployees = employeeList2, NotFoundList = listNotFound });
            }

            return Json(new List<UserInformationViewModel>());
        }

        private static void ReadExcel(string filePath)
        {
            //create a instance for the Excel object  
            Application oExcel = new Application();

            //specify the file name where its actually exist  
            //string filepath = @ "D:\TPMS\Uploaded_Boq\Raveena_boq_From_Db.xlsx";

            //pass that to workbook object  
            Workbook WB = oExcel.Workbooks.Open(filePath);


            // statement get the workbookname  
            string ExcelWorkbookname = WB.Name;

            // statement get the worksheet count  
            int worksheetcount = WB.Worksheets.Count;

            Worksheet wks = (Worksheet)WB.Worksheets[1];

            // statement get the firstworksheetname  

            string firstworksheetname = wks.Name;

            //statement get the first cell value  
            var firstcellvalue = ((Range)wks.Cells[1, 1]).Value;
        }
        private System.Data.DataTable ReadExcelUsingOledb(string filePath, string extension)
        {
            DataSet ds = new DataSet();
            string conString = string.Empty;
            switch (extension)
            {
                case ".xls": //Excel 97-03.
                    conString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
                case ".xlsx": //Excel 07 and above.
                    conString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                    break;
            }

            conString = string.Format(conString, filePath);
            using (OleDbConnection connExcel = new OleDbConnection(conString))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        //Get the name of First Sheet.
                        connExcel.Open();
                        System.Data.DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        //Read Data from First Sheet.
                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(ds);
                        connExcel.Close();
                        //var list = ds.Tables[0].AsEnumerable().Select(r => r["EmployeeId"].ToString());
                        return ds.Tables[0];
                    }
                }
            }

        }

        public virtual ActionResult IndexSupervisedEmployees(int? id)
        {
            try
            {
                if (!SecurityHelper.IsAvailable("ReportingHierarchy"))
                {
                    Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                    return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
                }
                var supervisors = db.EmployeeSupervisor.Where(u => u.SupervisorUserId == id && u.DataEntryStatus == 1).Select(e => e.EmployeeUser).Distinct().ToList();
                return PartialView(supervisors);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult IndexEmployeeSupervisers(int? id)
        {
            try
            {
                if (!SecurityHelper.IsAvailable("ReportingHierarchy"))
                {
                    Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                    return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
                }
                var supervisors = db.EmployeeSupervisor.Where(u => u.EmployeeUserId == id && u.DataEntryStatus == 1).Select(e => e.SupervisorUser).Distinct().ToList();
                return PartialView(supervisors);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult SupervisedEmployeesChangeHistory(int refrenceId, string columnName)
        {
            try
            {
                AllowView();
                string type = "";
                if (columnName == "EmployeeUserId")
                    type = "Supervisor";
                else
                    type = "Supervised Employee";
                var entitySet1 = db.AuditLogDetail.Where(d => d.ColumnName == columnName && d.NewValue == refrenceId.ToString() && d.AuditLog.TableName == "EmployeeSupervisor")
                                  .Select(d => d.AuditLog).ToList();
                var entitySet = entitySet1.SelectMany(a => a.AuditLogDetail).Where(d => d.ColumnName != columnName).ToList();
                foreach (var each in entitySet)
                {

                    int userId;
                    if (int.TryParse(each.NewValue, out userId))
                    {
                        var user = db.UserInformation.FirstOrDefault(u => u.Id == userId); //db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                        each.ColumnName = user.FullName + "(" + user.EmployeeId + ")";
                    }
                    if (columnName == "EmployeeUserId")
                        each.NewValue = "Supervisor";
                    else
                        each.NewValue = "Supervised Employee";
                }
                var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceId);
                ViewBag.Title = type + " Change History for " + userInformation.FullName;
                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
