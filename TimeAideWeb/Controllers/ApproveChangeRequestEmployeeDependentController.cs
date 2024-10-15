using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class ApproveChangeRequestEmployeeDependentController : TimeAideWebSecurotyBaseControllers
    {
        public ApproveChangeRequestEmployeeDependentController() : base("ChangeRequestEmployeeDependent")
        {
        }
        public ActionResult ApproveChangeRequestEmployeeDependent(int id)
        {
            try
            {
                AllowAdd();
                var item = db.ChangeRequestEmployeeDependent.FirstOrDefault(c => c.Id == id);
                ViewBag.WorkflowTriggerRequestDetail = item.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
                item.ChangeRequestRemarks = "";
                ViewBag.CanTakeAction = WorkflowService.CanTakeAction(item);
                ViewBag.IsViewOnly = false;
                return PartialView(item);
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


                WorkflowTriggerRequest workflowTriggerRequest = db.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestEmployeeDependentId == changeRequest.Id);
                var detail = WorkflowService.GetNextWorkflowLevel<ChangeRequestEmployeeDependent>(db, workflowTriggerRequest, changeRequest);

                StringBuilder li = new StringBuilder();
                var canTakeAction = WorkflowService.CanTakeAction(changeRequest);
                if (canTakeAction)
                    WorkflowService.GetNewListItem(detail, li);
                //return Json(li.ToString());
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
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                return View();
            }
        }
    }
}