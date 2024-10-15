
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
    public class JobCertificationTemplateController : TimeAideWebControllers<JobCertificationTemplate>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,NotificationMessageName,NotificationMessageDescription,Message,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationMessage notificationMessage)
        public ActionResult Create(JobCertificationTemplate model)
        {
            if (ModelState.IsValid)
            {
                db.JobCertificationTemplate.Add(model);
                db.SaveChanges();
                return Json(model);
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,NotificationMessageName,NotificationMessageDescription,Message,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] NotificationMessage notificationMessage)
        public ActionResult Edit(JobCertificationTemplate model)
        {
            if (ModelState.IsValid)
            {
                model.SetUpdated<JobCertificationTemplate>();
                db.Entry(model).State = EntityState.Modified;
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
        public JsonResult AjaxGetJobCertificationTemplate(bool IsAllCompanies,int? CompanyId)
        {
            var templateList = new List<dynamic>();
            if (CompanyId == null)
            {
                templateList = db.GetAllByCompany<JobCertificationTemplate>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                    .Where(w => IsAllCompanies ? (w.CompanyId == null) : true)
                                    .Where(w => w.TemplateTypeId == null || w.TemplateTypeId == 1)
                                    .Select(s => new { id = s.Id, name = s.Name }).ToList<dynamic>();
            }
            else
            {
                templateList = db.GetAllByCompany<JobCertificationTemplate>(CompanyId, SessionHelper.SelectedClientId)                                   
                                   .Where(w => w.TemplateTypeId == null || w.TemplateTypeId == 1)
                                   .Select(s => new { id = s.Id, name = s.Name }).ToList<dynamic>();
            }
            JsonResult jsonResult = new JsonResult()
            {
                Data = templateList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
            //return Json(cityList);
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
