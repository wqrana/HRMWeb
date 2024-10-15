using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class NotificationScheduleRoleController : TimeAideWebControllers<NotificationScheduleEmployeeGroup>
    {

        public ActionResult CreateEdit(int? userId)
        {
            IEnumerable<SelectListItem> notificationList = null;
            notificationList = db.GetAll<EmployeeGroup>(SessionHelper.SelectedClientId).
                                      Select(s => new SelectListItem
                                      {
                                          Text = s.EmployeeGroupName,
                                          Value = s.Id.ToString()

                                      });

            ViewBag.NotificationList = notificationList;
            string[] TempData = db.GetAll<UserEmployeeGroup>(SessionHelper.SelectedClientId).Where(e=>e.UserInformationId==userId).Select(s => s.EmployeeGroupId.ToString()).ToArray<string>();
            ViewBag.SelectedNotificationList = TempData;
            ViewBag.EmployeeNotificationRoleId = userId;
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateEdit(int id, string selectedCredentialIds)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            try
            {
                var selectedEmployeeGroupList = selectedCredentialIds.Split(',').ToList();
                List<UserEmployeeGroup> employeeGroupAddList = new List<UserEmployeeGroup>();
                List<UserEmployeeGroup> employeeGroupRemoveList = new List<UserEmployeeGroup>();
                var existingCredentialList = db.UserEmployeeGroup.Where(w => w.UserInformationId == id).ToList();

                foreach (var employeeGroupItem in existingCredentialList)
                {
                    var RecCnt = selectedEmployeeGroupList.Where(w => w == employeeGroupItem.EmployeeGroupId.ToString()).Count();
                    if (RecCnt == 0)
                    {
                        employeeGroupRemoveList.Add(employeeGroupItem);
                    }

                }
                foreach (var selectedEmployeeGroupId in selectedEmployeeGroupList)
                {
                    if (selectedEmployeeGroupId == "") continue;
                    int credentialId = int.Parse(selectedEmployeeGroupId);
                    var recExists = existingCredentialList.Where(w => w.EmployeeGroupId == credentialId).Count();
                    if (recExists == 0)
                    {
                        employeeGroupAddList.Add(new UserEmployeeGroup() { EmployeeGroupId = credentialId , UserInformationId = id });

                    }
                }

                db.UserEmployeeGroup.RemoveRange(employeeGroupRemoveList);
                db.UserEmployeeGroup.AddRange(employeeGroupAddList);

                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

        public override bool CheckBeforeDelete(int id)
        {
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
