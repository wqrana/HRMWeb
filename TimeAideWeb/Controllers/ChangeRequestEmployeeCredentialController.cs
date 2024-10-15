using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ChangeRequestEmployeeCredentialController : TimeAideWebSecurotyBaseControllers
    {
        public ChangeRequestEmployeeCredentialController() : base("ChangeRequestEmployeeCredential")
        {
        }
        public ActionResult ViewSelfServiceEmployeeCredential(int id)
        {
            try
            {
                AllowView();
                var item = db.SelfServiceEmployeeCredential.FirstOrDefault(c => c.Id == id);
                int workflowTriggerRequestId = item.WorkflowTriggerRequest.OrderByDescending(c => c.Id).FirstOrDefault().Id;
                if (!db.NotificationLogMessageReadBy.Any(n => n.WorkflowTriggerRequestId == workflowTriggerRequestId && n.ReadById == SessionHelper.LoginId))
                {
                    NotificationLogMessageReadBy notificationLogMessageReadBy = new NotificationLogMessageReadBy();
                    notificationLogMessageReadBy.WorkflowTriggerRequestId = workflowTriggerRequestId;
                    notificationLogMessageReadBy.ReadById = SessionHelper.LoginId;
                    db.NotificationLogMessageReadBy.Add(notificationLogMessageReadBy);
                    db.SaveChanges();
                }
                item.ChangeRequestRemarks = "";
                ViewBag.WorkflowTriggerRequestDetail = item.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
                ViewBag.CanTakeAction = false;
                ViewBag.IsViewOnly = true;
                ViewBag.Label = "Employee Credential Upload";
                return PartialView("~/Views/ApproveChangeRequestEmployeeCredential/ApproveSelfServiceEmployeeCredential.cshtml", item);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveSelfServiceEmployeeCredential", "ApproveSelfServiceEmployeeCredential");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
    }
}