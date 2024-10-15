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
    public class ChangeRequestEmployeeDocumentController : TimeAideWebSecurotyBaseControllers
    {
        public ChangeRequestEmployeeDocumentController() : base("ChangeRequestEmployeeDocument")
        {
        }
        // GET: ChangeRequestAddress
        public ActionResult ViewSelfServiceEmployeeDocument(int id)
        {
            try
            {
                //AllowAdd();
                ViewBag.Label = "Employee Document Upload";
                var item = db.SelfServiceEmployeeDocument.FirstOrDefault(c => c.Id == id);
                item.ChangeRequestRemarks = "";
                ViewBag.WorkflowTriggerRequestDetail = item.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
                int workflowTriggerRequestId = item.WorkflowTriggerRequest.OrderByDescending(c => c.Id).FirstOrDefault().Id;
                if (!db.NotificationLogMessageReadBy.Any(n => n.WorkflowTriggerRequestId == workflowTriggerRequestId && n.ReadById == SessionHelper.LoginId))
                {
                    NotificationLogMessageReadBy notificationLogMessageReadBy = new NotificationLogMessageReadBy();
                    notificationLogMessageReadBy.WorkflowTriggerRequestId = workflowTriggerRequestId;
                    notificationLogMessageReadBy.ReadById = SessionHelper.LoginId;
                    db.NotificationLogMessageReadBy.Add(notificationLogMessageReadBy);
                    db.SaveChanges();
                }
                ViewBag.CanTakeAction = false;
                ViewBag.IsViewOnly = true;
                return PartialView("~/Views/ApproveChangeRequestEmployeeDocument/ApproveSelfServiceEmployeeDocument.cshtml", item);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveSelfServiceEmployeeDocument", "ApproveSelfServiceEmployeeDocument");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
    }
}