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
    public class ChangeRequestAddressController : TimeAideWebControllers<ChangeRequestAddress>
    {
        public virtual ActionResult ChangeRequestAddressList()
        {
            try
            {
                AllowView();
                var entitySet = TimeAide.Services.WorkflowService.GetWorkFlowNotifications();
                //var entitySet1 = TimeAide.Services.WorkflowService.GetWorkFlowClosingNotifications();
                //entitySet.Union(entitySet1);
                return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).ToList());
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        // GET: ChangeRequestAddress

        public ActionResult ViewMailingChangeRequestAddress(int id)
        {
            try
            {
                //AllowAdd();
                var item = db.ChangeRequestAddress.FirstOrDefault(c => c.Id == id);
                int workflowTriggerRequestId = item.WorkflowTriggerRequest.OrderByDescending(c => c.Id).FirstOrDefault().Id;
                if (item.UserContactInformation != null)
                    item.UserContactInformation.AddressType = item.AddressType;
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
                return PartialView("~/Views/ApproveChangeRequestAddress/ApproveMailingChangeRequestAddress.cshtml", item);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApproveChangeRequestAddress", "ApproveMailingChangeRequestAddress");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
    }
}