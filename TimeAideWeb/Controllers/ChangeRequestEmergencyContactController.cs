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
    public class ChangeRequestEmergencyContactController : TimeAideWebSecurotyBaseControllers
    {
        public ChangeRequestEmergencyContactController() : base("ChangeRequestEmergencyContact")
        {
        }
        // GET: ChangeRequestAddress
        public ActionResult ViewChangeRequestEmergencyContact(int id)
        {
            try
            {
                AllowView();
                var item = db.ChangeRequestEmergencyContact.FirstOrDefault(c => c.Id == id);
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
                return PartialView("~/Views/ApproveChangeRequestEmergencyContact/ApproveChangeRequestEmergencyContact.cshtml", item);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveChangeRequestEmergencyContact", "ApproveChangeRequestEmergencyContact");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
    }
}