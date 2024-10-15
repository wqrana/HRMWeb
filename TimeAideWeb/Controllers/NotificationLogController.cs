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
    public class NotificationLogController : TimeAideWebControllers<NotificationLog>
    {
        public ActionResult IndexByNotificationSchedule(int? id)
        {
            ViewBag.NotificationScheduleId = id;
            //ViewBag.SelectedScale = new { Id = id, ScaleName = scaleRecord.ScaleName, MaxValue = scaleRecord.MaxValue };
            var model = db.NotificationScheduleDetail.Where(w => w.NotificationScheduleId == (id ?? 0)).OrderBy(o => o.DaysBefore).ToList();
            return PartialView("Index", model);
        }

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NotificationScheduleId,DaysBefore,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationScheduleDetail notificationScheduleDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.NotificationScheduleDetail.Add(notificationScheduleDetail);
                    db.SaveChanges();
                }
                catch 
                {
                }
                return Json(notificationScheduleDetail);
            }

            return GetErrors();
        }
        //[HttpPost]
        //public JsonResult CreateEdit(NotificationScheduleDetail model)
        //{
        //    string status = "Success";
        //    string message = "Successfully Added/Updated!";
        //    NotificationScheduleDetail NotificationScheduleEntity = null;
        //    try
        //    {
        //        var isAlreadyExist = db.NotificationScheduleDetail
        //                                .Where(w => w.DataEntryStatus == 1 && 
        //                                            w.NotificationScheduleId == model.NotificationScheduleId && 
        //                                            w.Id != model.Id && w.DaysBefore==model.DaysBefore)
        //                                .Count();
        //        if (isAlreadyExist > 0)
        //        {
        //            status = "Error";
        //            message = "Scale Value/Name is already added";
        //        }
        //        else
        //        {
        //            if (model.Id == 0)
        //            {
        //                NotificationScheduleEntity = new NotificationScheduleDetail();
        //                NotificationScheduleEntity.NotificationScheduleId = model.NotificationScheduleId;
        //                NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
        //                NotificationScheduleEntity.NotificationScheduleRole = model.NotificationRoleId;
        //                //NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
        //                db.NotificationScheduleDetail.Add(NotificationScheduleEntity);
        //            }
        //            else
        //            {
        //                NotificationScheduleEntity = db.NotificationScheduleDetail.Find(model.Id);
        //                NotificationScheduleEntity.ModifiedBy = SessionHelper.LoginId;
        //                NotificationScheduleEntity.ModifiedDate = DateTime.Now;
        //                NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
        //                NotificationScheduleEntity.NotificationRoleId = model.NotificationRoleId;
        //                //NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
        //            }
        //            NotificationScheduleEntity.DaysBefore = model.DaysBefore;
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = "Error";
        //        message = ex.Message;
        //    }

        //    return Json(new { status = status, message = message });
        //}

        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NotificationScheduleId,DaysBefore,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationScheduleDetail notificationScheduleDetail)
        {
            if (ModelState.IsValid)
            {
                notificationScheduleDetail.SetUpdated<NotificationScheduleDetail>();
                db.Entry(notificationScheduleDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public virtual ActionResult ReadNotification(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                NotificationLog model = db.Find<NotificationLog>(id.Value, SessionHelper.SelectedClientId);
                if (model == null)
                {
                    return PartialView();
                }

                ViewBag.Label = ViewBag.Label + " - Detail";


                if (!model.NotificationLogMessageReadBy.Any(n => n.ReadById == SessionHelper.LoginId))
                {
                    NotificationLogMessageReadBy notificationLogMessageReadBy = new NotificationLogMessageReadBy();
                    notificationLogMessageReadBy.NotificationLog = model;
                    notificationLogMessageReadBy.ReadById = SessionHelper.LoginId;
                    db.NotificationLogMessageReadBy.Add(notificationLogMessageReadBy);
                    db.SaveChanges();
                    ViewBag.MarkAsRead = true;
                }
                
                return PartialView("Details", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public virtual ActionResult ReadNotificationEmail(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                NotificationLog model = db.Find<NotificationLog>(id.Value, SessionHelper.SelectedClientId);
                if (model == null)
                {
                    return PartialView();
                }

                ViewBag.Label = ViewBag.Label + " - Detail";
                NotificationLogMessageReadBy notificationLogMessageReadBy = new NotificationLogMessageReadBy();
                notificationLogMessageReadBy.NotificationLog = model;
                notificationLogMessageReadBy.ReadById = SessionHelper.LoginId;
                db.NotificationLogMessageReadBy.Add(notificationLogMessageReadBy);
                db.SaveChanges();
                return View(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.NotificationSchedule.Include(u => u.UserInformations)
            //                                .Include(u => u.EmploymentHistory)
            //                                .Include(u => u.SupervisorEmployeeType)
            //             .FirstOrDefault(c => c.Id == id);
            //if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.SupervisorEmployeeType.Where(t => t.DataEntryStatus == 1).Count() > 0)
            //    return false;
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
