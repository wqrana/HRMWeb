using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Services.Helpers;
using TimeAide.Web.Controllers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Controllers
{
    public class CompensationTransactionController : TimeAideWebControllers<CompensationTransaction>
    {

        public ActionResult CreateCompensationTransaction()
        {
            ViewBag.CompensationTransactions = db.GetAll<TransactionConfiguration>(SessionHelper.SelectedClientId);
            return PartialView();
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
            ViewBag.SupervisorListObject = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.UserEmployeeGroup.Count > 0 && u.DataEntryStatus == 1 && u.Id != userId && u.UserEmployeeGroup.Any(g => g.EmployeeGroup.EmployeeGroupTypeId == 2));
            ViewBag.SelectedSupervisorListObject = db.GetAll<EmployeeSupervisor>(SessionHelper.SelectedClientId).Where(e => e.EmployeeUserId == userId);
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
            ViewBag.SupervisorListObject = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.UserEmployeeGroup.Count > 0 && u.DataEntryStatus == 1 && u.Id != userId && u.UserEmployeeGroup.Any(g => g.EmployeeGroup.EmployeeGroupTypeId == 2));
            ViewBag.SelectedSupervisorListObject = db.GetAll<EmployeeSupervisor>(SessionHelper.SelectedClientId).Where(e => e.EmployeeUserId == userId);

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
                    if (supervisorUser.Department != null)
                        userInformationViewModel.DepartmentName = supervisorUser.Department.DepartmentName;
                    if (supervisorUser.Position != null)
                        userInformationViewModel.PositionName = supervisorUser.Position.PositionName;
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
        public ActionResult CreateEditCompensationTransaction(int? companyCompensationId)
        {
            ViewBag.CompanyCompensationId = companyCompensationId;
            ViewBag.SelectedCompensationTransactions = db.GetAll<CompensationTransaction>(SessionHelper.SelectedClientId)
                                                         .Where(e => e.CompanyCompensationId == companyCompensationId);
            ViewBag.CompensationTransactions = db.GetAll<TransactionConfiguration>(SessionHelper.SelectedClientId)
                                                .Where(t => !t.CompensationTransaction.Any(c => c.CompanyCompensationId == companyCompensationId));
            //ViewBag.SupervisorListObject = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == Convert.ToInt32(EmployeeGroupTypes.Supervisor) && u.UserInformationId != userId);

            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEditCompensationTransaction(int id, string selectedTransactionIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                CompanyCompensationService.UpdateCompensationTransaction(id, selectedTransactionIds);

            }
            catch (Exception ex)
            {
                Web.Helpers.ErrorLogHelper.InsertLog(Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

        

        public virtual ActionResult CreateIndex(string id)
        {
            try
            {
                List<int> selectedTransactionsList = new List<int>();
                if (!String.IsNullOrEmpty(id))
                {
                    foreach (string each in id.Split(','))
                    {
                        int transactionId;
                        if (int.TryParse(each, out transactionId))
                            selectedTransactionsList.Add(transactionId);
                    }

                }
                if (!SecurityHelper.IsAvailable("ReportingHierarchy"))
                {
                    Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                    return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
                }
                var supervisors = db.TransactionConfiguration.Where(u => selectedTransactionsList.Contains(u.Id) && u.DataEntryStatus == 1).ToList();
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
        public virtual ActionResult IndexCompensationTransaction(int? id)
        {
            try
            {
                if (!SecurityHelper.IsAvailable("ReportingHierarchy"))
                {
                    Exception exception = new Exception("You are not authorization to access requested information. Please contact system admin");
                    HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "User Information", "Index");
                    return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
                }
                var supervisors = db.CompensationTransaction.Where(u => u.CompanyCompensationId == id && u.DataEntryStatus == 1).ToList();
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
