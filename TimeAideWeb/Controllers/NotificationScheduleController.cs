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
    public class NotificationScheduleController : TimeAideWebControllers<NotificationSchedule>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NotificationScheduleName,NotificationScheduleDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationSchedule NotificationSchedule)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.NotificationSchedule.Add(NotificationSchedule);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
                return Json(NotificationSchedule);
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NotificationScheduleName,NotificationScheduleDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationSchedule NotificationSchedule)
        {
            if (ModelState.IsValid)
            {
                NotificationSchedule.SetUpdated<NotificationSchedule>();
                db.Entry(NotificationSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        [HttpPost]
        public JsonResult CreateEdit(NotificationSchedule model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            NotificationSchedule notificationScheduleEntity = null;
            try
            {
                var isAlreadyExist = db.NotificationSchedule
                                        .Where(w => w.DataEntryStatus == 1 && (w.Id != model.Id) && (w.NotificationScheduleName.ToLower() == model.NotificationScheduleName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Schedule Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        notificationScheduleEntity = new NotificationSchedule();
                        db.NotificationSchedule.Add(notificationScheduleEntity);
                    }
                    else
                    {
                        notificationScheduleEntity = db.NotificationSchedule.Find(model.Id);
                        notificationScheduleEntity.ModifiedBy = SessionHelper.LoginId;
                        notificationScheduleEntity.ModifiedDate = DateTime.Now;
                    }
                    notificationScheduleEntity.NotificationScheduleName = model.NotificationScheduleName;
                    notificationScheduleEntity.NotificationScheduleDescription = model.NotificationScheduleDescription;
                    db.SaveChanges();
                }
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
