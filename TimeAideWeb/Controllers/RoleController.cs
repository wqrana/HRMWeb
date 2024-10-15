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
    public class RoleController : TimeAideWebControllers<Role>
    {
        public override List<Role> OnIndex(List<Role> model)
        {
            model = model.Where(e => e.Id!=1).ToList();
            return model;
        }
        // POST: Role/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,RoleName,Description,RoleTypeId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] Role role)
        {
            if (ModelState.IsValid)
            {
                db.Role.Add(role);
                db.SaveChanges();
                return Json(role);
            }
            return GetErrors();
        }

        // POST: Role/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,RoleTypeId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,CompanyId,ClientId")] Role role)
        {
            if (ModelState.IsValid)
            {
                role.SetUpdated<Role>();
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(role);
        }

        public override bool CheckBeforeDelete(int id)
        {
            var role = db.Role.Include(u => u.RoleInterfaceControlPrivilege)
                         .Include(u => u.RoleFormPrivilege)
                         .Include(u => u.UserInformationRole)
                         .FirstOrDefault(c => c.Id == id);
            if (role.RoleInterfaceControlPrivilege.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (role.RoleFormPrivilege.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (role.UserInformationRole.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }

        public JsonResult AjaxGetRoleByRoleType(int? roleTypeId)
        {
            var cityList = db.Role
                                .Where(w => w.RoleTypeId == roleTypeId && w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId)
                                .Select(s => new { id = s.Id, name = s.RoleName }).ToList();

            JsonResult jsonResult = new JsonResult()
            {
                Data = cityList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
            //return Json(cityList);
        }

        public ActionResult AddRemoveMembers(int? roleId)
        {
            var role = db.Role.FirstOrDefault(r=>r.Id==roleId.Value);
            ViewBag.RoleName = role.RoleName;
            ViewBag.RoleId = roleId;
            ViewBag.SelectedRoleMembers = db.GetAll<UserInformationRole>(SessionHelper.SelectedClientId).Where(e => e.RoleId == roleId);
            ViewBag.AvailableEmployees = db.GetAllByCompany<UserInformation>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(u => u.DataEntryStatus == 1 && !u.UserInformationRole.Any());
            return PartialView();
        }
        [HttpPost]
        public JsonResult AddRemoveMembers(int roleId, string selectedMemberIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedMembersList = selectedMemberIds.Split(',').ToList();
                List<UserInformationRole> membersAddList = new List<UserInformationRole>();
                List<UserInformationRole> membersRemoveList = new List<UserInformationRole>();
                var existingmemberList = db.UserInformationRole.Where(w => w.RoleId == roleId).ToList();

                foreach (var eachMember in existingmemberList)
                {
                    var RecCnt = selectedMembersList.Where(w => w == eachMember.UserInformationId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        membersRemoveList.Add(eachMember);
                    }

                }
                foreach (var selectedMemberId in selectedMembersList)
                {
                    if (selectedMemberId == "") continue;
                    int userInformationId = int.Parse(selectedMemberId);
                    var recExists = existingmemberList.Where(w => w.UserInformationId == userInformationId).Count();
                    if (recExists == 0)
                    {
                        membersAddList.Add(new UserInformationRole() { RoleId = roleId, UserInformationId = userInformationId });

                    }
                }

                db.UserInformationRole.RemoveRange(membersRemoveList);
                db.UserInformationRole.AddRange(membersAddList);

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
        public virtual ActionResult RoleMembersHistory(int refrenceId)
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
                var entitySet1 = db.AuditLogDetail.Where(d => d.ColumnName == "RoleId" && d.NewValue == refrenceId.ToString() && d.AuditLog.TableName== "UserInformationRole")
                                  .Select(d => d.AuditLog).ToList();
                var entitySet = entitySet1.SelectMany(a => a.AuditLogDetail).Where(d => d.ColumnName != "RoleId").ToList();
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
