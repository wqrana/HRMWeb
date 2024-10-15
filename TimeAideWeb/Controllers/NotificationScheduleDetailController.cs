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
    public class NotificationScheduleDetailController : TimeAideWebControllers<NotificationScheduleDetail>
    {
        public ActionResult IndexByNotificationSchedule(int? id)
        {
            List<NotificationScheduleDetail> model = null;
            var notificationSchedule = db.NotificationSchedule.FirstOrDefault(w => w.Id == (id ?? 0));
            ViewBag.NotificationScheduleId = id ?? 0;
            if (id.HasValue)
            {
                ViewBag.NotificationScheduleName = notificationSchedule.NotificationScheduleName;
                model = notificationSchedule.NotificationScheduleDetail.OrderBy(o => o.DaysBefore).ToList();
            }
            return PartialView("Index", model);
        }
        public override void OnCreate()
        {
            SetViewBag();
        }
        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create1([Bind(Include = "Id,NotificationScheduleId,DaysBefore,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationScheduleDetail notificationScheduleDetail)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.NotificationScheduleDetail.Add(notificationScheduleDetail);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
                return Json(notificationScheduleDetail);
            }

            return GetErrors();
        }
        [HttpPost]
        public JsonResult Create(NotificationScheduleDetail model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            NotificationScheduleDetail NotificationScheduleEntity = null;
            try
            {
                var isAlreadyExist = db.NotificationScheduleDetail
                                        .Where(w => w.DataEntryStatus == 1 &&
                                                    w.NotificationScheduleId == model.NotificationScheduleId &&
                                                    w.Id != model.Id && w.DaysBefore == model.DaysBefore)
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Scale Value/Name is already added";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        NotificationScheduleEntity = new NotificationScheduleDetail();
                        NotificationScheduleEntity.NotificationScheduleId = model.NotificationScheduleId;
                        

                        List<string> employeeGroupIds = (model.SelectedEmployeeGroupId ?? "").Split(',').ToList();
                        foreach (var eachId in employeeGroupIds)
                        {
                            int newId;
                            Int32.TryParse(eachId, out newId);
                            if (newId > 0)
                                NotificationScheduleEntity.NotificationScheduleEmployeeGroup.Add(new NotificationScheduleEmployeeGroup { EmployeeGroupId = Convert.ToInt32(eachId) });
                        }


                        //NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
                        db.NotificationScheduleDetail.Add(NotificationScheduleEntity);
                    }
                    else
                    {
                        NotificationScheduleEntity = db.NotificationScheduleDetail.Find(model.Id);
                        NotificationScheduleEntity.ModifiedBy = SessionHelper.LoginId;
                        NotificationScheduleEntity.ModifiedDate = DateTime.Now;
                        
                        var selectedEmployeeGroupIds = (model.SelectedEmployeeGroupId ?? "").Split(',').ToList();
                        List<NotificationScheduleEmployeeGroup> employeeGroupAddList = new List<NotificationScheduleEmployeeGroup>();
                        List<NotificationScheduleEmployeeGroup> employeeGroupRemoveList = new List<NotificationScheduleEmployeeGroup>();
                        var existingEmployeeGroupList = db.NotificationScheduleEmployeeGroup.Where(w => w.NotificationScheduleDetailId == model.Id).ToList();

                        foreach (var eachExisting in existingEmployeeGroupList)
                        {
                            var RecCnt = selectedEmployeeGroupIds.Where(w => w == eachExisting.EmployeeGroupId.ToString()).Count();
                            if (RecCnt == 0)
                            {
                                employeeGroupRemoveList.Add(eachExisting);
                            }

                        }
                        foreach (var eachSelected in selectedEmployeeGroupIds)
                        {
                            if (eachSelected == "") continue;
                            int employeeGroupId = int.Parse(eachSelected);
                            var recExists = existingEmployeeGroupList.Where(w => w.EmployeeGroupId == employeeGroupId).Count();
                            if (recExists == 0)
                            {
                                NotificationScheduleEntity.NotificationScheduleEmployeeGroup.Add(new NotificationScheduleEmployeeGroup { EmployeeGroupId = employeeGroupId });
                            }
                        }

                        db.NotificationScheduleEmployeeGroup.RemoveRange(employeeGroupRemoveList);
                        db.NotificationScheduleEmployeeGroup.AddRange(employeeGroupAddList);

                        //NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
                    }
                    NotificationScheduleEntity.DaysBefore = model.DaysBefore;
                    NotificationScheduleEntity.ExcludeUser = model.ExcludeUser;
                    NotificationScheduleEntity.ExcludeSupervisor = model.ExcludeSupervisor;
                    NotificationScheduleEntity.NotificationMessageId = model.NotificationMessageId;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

        public override NotificationScheduleDetail OnEdit(NotificationScheduleDetail entity)
        {
            SetViewBag();
            entity.SelectedEmployeeGroupId = String.Join(",", db.NotificationScheduleEmployeeGroup.Where(w => w.DataEntryStatus == 1 && w.NotificationScheduleDetailId == entity.Id).Select(c => c.EmployeeGroup.Id.ToString()));
            return entity;
        }

        private void SetViewBag()
        {
            ViewBag.EmployeeGroupId = new SelectList(db.GetAllByCompany<EmployeeGroup>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "EmployeeGroupName");
        }

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
