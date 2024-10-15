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
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class ChangeRequestController : TimeAideWebBaseControllers
    {
        public virtual ActionResult NotiificationListActive()
        {
            try
            {
                NotificationViewModel model = new NotificationViewModel();
                //if (SecurityHelper.IsAvailable("ShowAllWorkflowNotifications"))
                //{
                //    var entitySet = TimeAide.Services.WorkflowService.GetWorkFlowNotificationHistory();
                //    var entitySet1 = TimeAide.Services.WorkflowService.GetWorkFlowClosingNotifications(false);
                //    model.WorkflowTriggerRequestDetail = entitySet.Union(entitySet1).OrderByDescending(e => e.CreatedDate).ToList();
                //}
                //if (SecurityHelper.IsAvailable("ShowAllExpirationNotification"))
                //{
                //    model.NotificationLog = TimeAide.Services.NotificationScheduleServiceManager.GetNotificationList(SessionHelper.LoginEmail, false);
                //}
                ////ViewBag.RequestTypeId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.WorkflowTriggerTypeId = new SelectList((new TimeAideContext()).GetAll<WorkflowTriggerType>(), "Id", "WorkflowTriggerTypeName");
                ViewBag.NotificationTypeId = new SelectList((new TimeAideContext()).GetAll<NotificationType>(), "Id", "NotificationTypeName");

                //List<SelectListItem> workflowStatusItems = new List<SelectListItem>();
                //workflowStatusItems.Add(new SelectListItem() { Value = "1", Text = "In-Progress" });
                //workflowStatusItems.Add(new SelectListItem() { Value = "2", Text = "Closed" });
                //SelectList selectWorkflowStatusListItems = new SelectList(workflowStatusItems, "Value", "Text", 1);
                ViewBag.WorkflowStatusId = new SelectList((new TimeAideContext()).GetAll<WorkflowActionType>(), "Id", "WorkflowActionTypeName");



                //ViewBag.WorkflowStatusId = selectWorkflowStatusListItems;

                List<SelectListItem> notificationStatusItems = new List<SelectListItem>();
                notificationStatusItems.Add(new SelectListItem() { Value = "1", Text = "Un-read" });
                notificationStatusItems.Add(new SelectListItem() { Value = "2", Text = "Read" });
                SelectList selectNotificationStatusListItems = new SelectList(notificationStatusItems, "Value", "Text", 1);

                ViewBag.NotificationStatusId = selectNotificationStatusListItems;
                ViewBag.IsViewActive = true;

            NotificationFilterViewModel filterViewModel = new NotificationFilterViewModel()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
            };
                SessionHelper.NotificationFilterViewModel = filterViewModel;
                

                return PartialView("NotiificationList", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult NotiificationList()
        {

            try
            {
                NotificationViewModel model = new NotificationViewModel();
                //if (SecurityHelper.IsAvailable("ShowAllWorkflowNotifications"))
                //{
                //    var entitySet = TimeAide.Services.WorkflowService.GetWorkFlowNotificationHistory();
                //    var entitySet1 = TimeAide.Services.WorkflowService.GetWorkFlowClosingNotifications(false);
                //    model.WorkflowTriggerRequestDetail = entitySet.Union(entitySet1).OrderByDescending(e => e.CreatedDate).ToList();
                //}
                //if (SecurityHelper.IsAvailable("ShowAllExpirationNotification"))
                //{
                //    model.NotificationLog = TimeAide.Services.NotificationScheduleServiceManager.GetNotificationList(SessionHelper.LoginEmail, false);
                //}
                ////ViewBag.RequestTypeId = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.WorkflowTriggerTypeId = new SelectList((new TimeAideContext()).GetAll<WorkflowTriggerType>(), "Id", "WorkflowTriggerTypeName");
                ViewBag.NotificationTypeId = new SelectList((new TimeAideContext()).GetAll<NotificationType>(), "Id", "NotificationTypeName");

                //List<SelectListItem> workflowStatusItems = new List<SelectListItem>();
                //workflowStatusItems.Add(new SelectListItem() { Value = "1", Text = "In-Progress" });
                //workflowStatusItems.Add(new SelectListItem() { Value = "2", Text = "Closed" });
                //SelectList selectWorkflowStatusListItems = new SelectList(workflowStatusItems, "Value", "Text");
                ViewBag.WorkflowStatusId = new SelectList((new TimeAideContext()).GetAll<WorkflowActionType>(), "Id", "WorkflowActionTypeName");

                //ViewBag.WorkflowStatusId = selectWorkflowStatusListItems;

                List<SelectListItem> notificationStatusItems = new List<SelectListItem>();
                notificationStatusItems.Add(new SelectListItem() { Value = "1", Text = "Un-read" });
                notificationStatusItems.Add(new SelectListItem() { Value = "2", Text = "Read" });
                SelectList selectNotificationStatusListItems = new SelectList(notificationStatusItems, "Value", "Text");

                ViewBag.NotificationStatusId = selectNotificationStatusListItems;
                ViewBag.IsViewActive = false;
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult Notifications(NotificationFilterViewModel filterModel)
        {
            try
            {
                SessionHelper.NotificationFilterViewModel = filterModel;


                NotificationViewModel model = new NotificationViewModel();
                if (SecurityHelper.IsAvailable("ShowAllWorkflowNotifications"))
                {
                    //var entitySet = TimeAide.Services.WorkflowService.GetWorkFlowNotificationHistory();
                    // var entitySet1 = TimeAide.Services.WorkflowService.GetWorkFlowClosingNotifications(false);

                    var entitySet = TimeAide.Services.WorkflowService.GetNewWorkFlowNotificationHistory(filterModel);



                    var entitySet1 = TimeAide.Services.WorkflowService.GetNewWorkFlowClosingNotifications(false, filterModel);


                    var unionSet = entitySet.Union(entitySet1);


                    //if (filterModel != null && filterModel.StartDate.HasValue && filterModel.EndDate.HasValue)
                    //{
                    //    unionSet = unionSet.Where(w => w.WorkflowTriggerRequest.CreatedDate >= filterModel.StartDate && w.WorkflowTriggerRequest.CreatedDate <= filterModel.EndDate);
                    //}
                    //if (filterModel != null && filterModel.EmployeeId.HasValue)
                    //{
                    //    unionSet = unionSet.Where(w => w.EmployeeId == filterModel.EmployeeId.Value);
                    //}
                    //if (filterModel != null && !string.IsNullOrEmpty(filterModel.EmployeeName))
                    //{
                    //    unionSet = unionSet.Where(w => w.ShortFullName.ToLower().Contains(filterModel.EmployeeName.ToLower()));
                    //}
                    //if (filterModel != null && filterModel.WorkflowTriggerTypeId.HasValue)
                    //{
                    //    unionSet = unionSet.Where(w => w.WorkflowTriggerTypeId == filterModel.WorkflowTriggerTypeId);
                    //}
                    //if (unionSet != null && filterModel.WorkflowStatusId.HasValue && filterModel.WorkflowStatusId.Value>0)
                    //if (unionSet != null && !string.IsNullOrEmpty(filterModel.SelectedWorkflowStatusId))
                    //{
                    //    var selectedSupervisorCompanyIds = (filterModel.SelectedWorkflowStatusId ?? "").Split(',').ToList();
                    //    //unionSet = unionSet.Where(w => w.WorkflowActionTypeId == filterModel.WorkflowStatusId.Value);
                    //    unionSet = unionSet.Where(w => selectedSupervisorCompanyIds.Contains(w.WorkflowActionTypeId.ToString()));
                    //}
                    if (unionSet == null)
                        unionSet = new List<BellIconNotificationViewModel>();
                    model.bellIconNotificationViewModelList = unionSet.OrderByDescending(e => e.CreatedDate).ToList();
                }
                if (SecurityHelper.IsAvailable("ShowAllExpirationNotification"))
                {
                    var exp = TimeAide.Services.NotificationScheduleServiceManager.GetNotificationList(SessionHelper.NotificationEmail ?? "", false).AsEnumerable<NotificationLog>();

                    if (exp != null && filterModel.StartDate.HasValue && filterModel.StartDate.HasValue)
                    {
                        exp = exp.Where(w => w.CreatedDate >= filterModel.StartDate && w.CreatedDate <= filterModel.EndDate);
                    }
                    if (exp != null && filterModel.EmployeeId.HasValue)
                    {
                        exp = exp.Where(w => w.UserInformation.EmployeeId == filterModel.EmployeeId.Value);
                    }
                    if (exp != null && !string.IsNullOrEmpty(filterModel.EmployeeName))
                    {
                        exp = exp.Where(w => w.UserInformation.ShortFullName.Contains(filterModel.EmployeeName));
                    }
                    if (exp != null && filterModel.WorkflowTriggerTypeId.HasValue)
                    {
                        exp = exp.Where(w => w.NotificationTypeId == filterModel.WorkflowTriggerTypeId);
                    }
                    if (exp != null && filterModel.WorkflowStatusId.HasValue)
                    {
                        if (filterModel.NotificationStatusId.Value == 1)
                        {
                            exp = exp.Where(w => !w.NotificationLogMessageReadBy.Any(i => i.ReadById == TimeAide.Common.Helpers.SessionHelper.LoginId));
                        }
                        else if (filterModel.NotificationStatusId.Value == 2)
                        {
                            exp = exp.Where(w => w.NotificationLogMessageReadBy.Any(i => i.ReadById == TimeAide.Common.Helpers.SessionHelper.LoginId));

                        }
                    }
                    if (exp == null)
                        exp = new List<NotificationLog>();
                    model.NotificationLog = exp.ToList();
                }
                //ViewBag.RequestTypeId = new SelectList(new List<SelectListItem>(), "Value", "Text");

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public virtual ActionResult EmailInfo(int id)
        {
            try
            {
                var entitySet = (new TimeAideContext()).GetAll<WorkflowTriggerRequestDetail>().Where(d => d.Id == id).Select(u => u.WorkflowTriggerRequest).FirstOrDefault();
                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult ExpirationEmailInfo(int id)
        {
            try
            {
                var entitySet = (new TimeAideContext()).GetAll<NotificationLogEmail>().Where(d => d.NotificationLogId == id);
                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult BellIcon()
        {
            try
            {
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(ChangeRequestAddress).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
    }
}