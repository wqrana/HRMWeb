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
    public class NotificationTypeController : TimeAideWebControllers<NotificationType>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(NotificationType notificationType)
        {
            if (ModelState.IsValid)
            {
                db.NotificationType.Add(notificationType);
                db.SaveChanges();
                return Json(notificationType);
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(NotificationType notificationType)
        {
            if (ModelState.IsValid)
            {
                notificationType.SetUpdated<NotificationType>();
                db.Entry(notificationType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public override bool CheckBeforeDelete(int id)
        {
            //var entity = db.NotificationMessage.Include(u => u.UserInformations)
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
