using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeGroupController : TimeAideWebControllers<EmployeeGroup>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeGroup employeeGroup)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.EmployeeGroup.Add(employeeGroup);
                    db.SaveChanges();
                    return Json(employeeGroup);
                }
                catch(Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeGroup employeeGroup)
        {
            if (ModelState.IsValid)
            {
                employeeGroup.SetUpdated<EmployeeGroup>();
                db.Entry(employeeGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        public ActionResult AddRemoveMembers(int? groupId)
        {
            var employeeGroup = db.Find<EmployeeGroup>(groupId.Value, SessionHelper.SelectedClientId);
            ViewBag.EmployeeGroupName = employeeGroup.EmployeeGroupName;
            ViewBag.EmployeeGroupId = groupId;
            ViewBag.SelectedEmployeeGroupMembers = db.GetAll<UserEmployeeGroup>(SessionHelper.SelectedClientId).Where(e => e.EmployeeGroupId == groupId);
            ViewBag.AvailableEmployees = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.DataEntryStatus == 1 && u.Id != groupId);
            //ViewBag.SupervisorListObject = db.GetAllByCompany<UserEmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.EmployeeGroup.EmployeeGroupTypeId == Convert.ToInt32(EmployeeGroupTypes.Supervisor) && u.UserInformationId != userId);

            return PartialView();
        }
        [HttpPost]
        public JsonResult AddRemoveMembers(int groupId, string selectedMemberIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedMembersList = selectedMemberIds.Split(',').ToList();
                List<UserEmployeeGroup> membersAddList = new List<UserEmployeeGroup>();
                List<UserEmployeeGroup> membersRemoveList = new List<UserEmployeeGroup>();
                var existingmemberList = db.UserEmployeeGroup.Where(w => w.EmployeeGroupId == groupId).ToList();

                foreach (var credentialItem in existingmemberList)
                {
                    var RecCnt = selectedMembersList.Where(w => w == credentialItem.UserInformationId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        membersRemoveList.Add(credentialItem);
                    }

                }
                foreach (var selectedMemberId in selectedMembersList)
                {
                    if (selectedMemberId == "") continue;
                    int userInformationId = int.Parse(selectedMemberId);
                    var recExists = existingmemberList.Where(w => w.UserInformationId == userInformationId).Count();
                    if (recExists == 0)
                    {
                        membersAddList.Add(new UserEmployeeGroup() { EmployeeGroupId = groupId, UserInformationId = userInformationId });

                    }
                }

                db.UserEmployeeGroup.RemoveRange(membersRemoveList);
                db.UserEmployeeGroup.AddRange(membersAddList);

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
        public virtual ActionResult EmployeeGroupMembersHistory(int refrenceId)
        {
            try
            {
                AllowView();
                string type = "";
                //if (columnName == "EmployeeUserId")
                //    type = "Supervisor";
                //else
                //    type = "Supervised Employee";
                //string[] tables = { "SupervisorCompany", "SupervisorDepartment", "SupervisorEmployeeType", "SupervisorSubDepartment", "UserEmployeeGroup", "UserInformationRole" };
                var entitySet1 = db.AuditLogDetail.Where(d => d.ColumnName == "EmployeeGroupId" && d.NewValue == refrenceId.ToString() && d.AuditLog.TableName == "UserEmployeeGroup")
                                  .Select(d => d.AuditLog).ToList();
                var entitySet = entitySet1.SelectMany(a => a.AuditLogDetail).Where(d => d.ColumnName != "EmployeeGroupId").ToList();
                foreach (var each in entitySet)
                {

                    int userId;
                    if (int.TryParse(each.NewValue, out userId))
                    {
                        var user = db.UserInformation.FirstOrDefault(u => u.Id == userId); //db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                        each.ColumnName = user.FullName + "(" + user.EmployeeId + ")";
                    }
                    each.NewValue = each.AuditLog.TableName;
                    //if (columnName == "EmployeeUserId")
                    //    each.NewValue = "Supervisor";
                    //else
                    //    each.NewValue = "Supervised Employee";
                }
                var userInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceId);
                ViewBag.Title = type + " Change History for " + userInformation.FullName;
                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeSupervisor", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmployeeGroup.Include(u => u.UserEmployeeGroup)
                                            .Include(u => u.WorkflowLevelGroup)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserEmployeeGroup.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.WorkflowLevelGroup.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
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
