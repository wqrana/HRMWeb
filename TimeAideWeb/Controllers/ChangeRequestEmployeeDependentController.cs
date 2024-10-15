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
    public class ChangeRequestEmployeeDependentController : TimeAideWebSecurotyBaseControllers
    {
        public ChangeRequestEmployeeDependentController() : base("ChangeRequestEmployeeDependent")
        {
        }
        public ActionResult ViewChangeRequestEmployeeDependent(int id)
        {
            try
            {
                AllowView();
                var item = db.ChangeRequestEmployeeDependent.FirstOrDefault(c => c.Id == id);
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
                item.ChangeRequestRemarks = "";
                ViewBag.CanTakeAction = false;
                ViewBag.IsViewOnly = true;
                return PartialView("~/Views/ApproveChangeRequestEmployeeDependent/ApproveChangeRequestEmployeeDependent.cshtml", item);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveChangeRequestEmployeeDependent", "ApproveChangeRequestEmployeeDependent");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public ActionResult ApproveChangeRequestEmployeeDependent(ChangeRequestEmployeeDependent model)
        {
            try
            {
                AllowAdd();
                //db.Entry(ChangeRequestAddress).State = EntityState.Modified;
                ChangeRequestEmployeeDependent changeRequest = db.ChangeRequestEmployeeDependent.FirstOrDefault(i => i.Id == model.Id);
                changeRequest.ChangeRequestStatusId = model.ChangeRequestStatusId;
                changeRequest.ChangeRequestRemarks = model.ChangeRequestRemarks;


                WorkflowTriggerRequest workflowTriggerRequest = db.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestEmailNumbersId == changeRequest.Id);
                WorkflowService.GetNextWorkflowLevel<ChangeRequestEmployeeDependent>(db, workflowTriggerRequest, changeRequest);

                return Json(model);
            }

            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveChangeRequestEmployeeDependent", "ApproveChangeRequestEmployeeDependent");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}