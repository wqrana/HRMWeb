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
    public class ApproveChangeRequestAddressController : TimeAideWebSecurotyBaseControllers
    {
        public ApproveChangeRequestAddressController() : base("ApproveChangeRequestAddress")
        {
        }
        public ActionResult ApproveMailingChangeRequestAddress(int id)
        {
            try
            {
                AllowAdd();
                var item = db.ChangeRequestAddress.FirstOrDefault(c => c.Id == id);
                item.ChangeRequestRemarks = "";
                if (item.UserContactInformation != null)
                    item.UserContactInformation.AddressType = item.AddressType;
                ViewBag.WorkflowTriggerRequestDetail = item.WorkflowTriggerRequest.FirstOrDefault().WorkflowTriggerRequestDetail.ToList();
                ViewBag.CanTakeAction = WorkflowService.CanTakeAction(item);
                ViewBag.MarkAsRead = WorkflowService.IsClosingNotification(item);
                ViewBag.IsViewOnly = false;
                return PartialView(item);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveChangeRequestAddress", "ApproveMailingChangeRequestAddress");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public ActionResult ApproveMailingChangeRequestAddress(ChangeRequestViewModel model)
        {
            try
            {
                AllowAdd();
                //db.Entry(ChangeRequestAddress).State = EntityState.Modified;
                ChangeRequestAddress changeRequest = db.ChangeRequestAddress.FirstOrDefault(i => i.Id == model.Id);
                changeRequest.ChangeRequestStatusId = model.ChangeRequestStatusId;
                changeRequest.ChangeRequestRemarks = model.ChangeRequestRemarks;

                WorkflowTriggerRequest workflowTriggerRequest = db.WorkflowTriggerRequest.FirstOrDefault(t => t.ChangeRequestAddressId == changeRequest.Id);
                WorkflowService.GetNextWorkflowLevel<ChangeRequestAddress>(db, workflowTriggerRequest, changeRequest);

                return Json(model);
            }

            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveChangeRequestAddress", "ApproveMailingChangeRequestAddress");
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