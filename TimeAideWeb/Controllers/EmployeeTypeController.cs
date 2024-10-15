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
    public class EmployeeTypeController : TimeAideWebControllers<EmployeeType>
    {

        // POST: EmployeeType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmployeeTypeName,EmployeeTypeDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeType.Add(employeeType);
                db.SaveChanges();
                return Json(employeeType);
            }

            return GetErrors();
        }


        // POST: EmployeeType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmployeeTypeName,EmployeeTypeDescription,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] EmployeeType employeeType)
        {
            if (ModelState.IsValid)
            {
                employeeType.SetUpdated<EmployeeType>();
                db.Entry(employeeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(EmployeeType model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.EmployeeType.Add(model);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }

            }
            else
            {
                status = "Error";
                message = "Missing Required field(s)";

            }

            return Json(new { status = status, message = message, id = model.Id, text = model.EmployeeTypeName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var entity = db.EmployeeType.Include(u => u.UserInformations)
                                            .Include(u => u.EmploymentHistory)
                                            .Include(u => u.SupervisorEmployeeType)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.SupervisorEmployeeType.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }

        public JsonResult AjaxGetEmployeeTypeByCompnay(string companyIds)
        {
            List<string> ids = companyIds.Split(',').ToList();
            ids.Remove("");
            var employeeTypeList = db.GetAll<EmployeeType>().Where(d => (ids.Count > 0 && ids.Contains(d.CompanyId.ToString()) || (ids.Count > 0 && !d.CompanyId.HasValue)) && d.ClientId == SessionHelper.SelectedClientId)
                                .Select(s => new { id = s.Id, name = s.EmployeeTypeName }).ToList();
            //if (employeeTypeList.Count > 0)
            //    employeeTypeList.Insert(0, new { id = 0, name = "All" });
            JsonResult jsonResult = new JsonResult()
            {
                Data = employeeTypeList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
            return jsonResult;
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
