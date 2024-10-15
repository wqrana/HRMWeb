using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class DepartmentController : TimeAideWebControllers<Department>
    {

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Department.Add(department);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                }
                return Json(department);
            }
            //ViewBag.ClientId = new SelectList(db.Client.Where(u => u.DataEntryStatus == 1), "ClientId", "Client");
            //ViewBag.CFSECodeId = new SelectList(db.CFSECode.Where(c=>c.DataEntryStatus==1), "Id", "CFSECodeDescription", department.CFSECodeId);

            return GetErrors();
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                department.SetUpdated<Department>();
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }
        [HttpPost]
        public ActionResult CreateAdhocMasterData(Department model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Department.Add(model);
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

            return Json(new { status = status, message = message, id = model.Id, text = model.DepartmentName });
        }
        public override bool CheckBeforeDelete(int id)
        {
            var department = db.Department.Include(u => u.SubDepartments)
                         .Include(u => u.UserInformations)
                         .Include(u => u.SupervisorDepartment)
                         .Include(u => u.EmploymentHistory).FirstOrDefault(c => c.Id == id);
            if (department.SubDepartments.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (department.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (department.SupervisorDepartment.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (department.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            return true;
        }

        public JsonResult AjaxGetDepartment(EmployeePrivilegeViewModel model)
        {
            var departmentList = db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                .Select(s => new { id = s.Id, name = s.DepartmentName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = departmentList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
            return jsonResult;
        }

        public JsonResult AjaxGetDepartmentByCompnay(string companyIds)
        {
            List<string> ids = companyIds.Split(',').ToList();
            ids.Remove("");
            var departmentList = db.GetAll<Department>().Where(d => (ids.Count > 0 && ids.Contains(d.CompanyId.ToString()) || (ids.Count > 0 && !d.CompanyId.HasValue)) && d.ClientId == SessionHelper.SelectedClientId && d.CompanyId.HasValue)
                                .Select(s => new { id = s.Id, name = s.DepartmentName + " - " + s.Company.CompanyName + "" }).ToList();

            var departmentList2 = db.GetAll<Department>().Where(d => (ids.Count > 0 && ids.Contains(d.CompanyId.ToString()) || (ids.Count > 0 && !d.CompanyId.HasValue)) && d.ClientId == SessionHelper.SelectedClientId && !d.CompanyId.HasValue)
                                .Select(s => new { id = s.Id, name = s.DepartmentName }).ToList();
            departmentList = departmentList.Union(departmentList2).ToList();
            //if (departmentList.Count > 0)
            //    departmentList.Insert(0, new { id = 0, name = "All" });
            JsonResult jsonResult = new JsonResult()
            {
                Data = departmentList,
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
