using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class SubDepartmentController : TimeAideWebControllers<SubDepartment>
    {

        public override List<SubDepartment> OnIndex(List<SubDepartment> model)
        {
            return db.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).AsQueryable().Where(SubDepartment.GetPredicate(SessionHelper.SelectedCompanyId, null)).ToList();
        }
        // POST: SubDepartment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(SubDepartment subDepartment)
        {
            if (ModelState.IsValid)
            {
                db.SubDepartment.Add(subDepartment);
                db.SaveChanges();
                return Json(subDepartment);
            }
            return GetErrors();
        }


        // POST: SubDepartment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(SubDepartment subDepartment)
        {
            if (ModelState.IsValid)
            {
                subDepartment.SetUpdated<SubDepartment>();
                db.Entry(subDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return GetErrors();
        }

        public JsonResult AjaxGetSubDepartment(EmployeePrivilegeViewModel model)
        {
            List<string> selectedClientList = (model.SelectedClientId ?? "").Split(',').ToList();
            var stateList = db.SubDepartment
                                .Where(w => selectedClientList.Contains(w.DepartmentId.ToString()) && w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId)
                                .Select(s => new { id = s.Id, name = s.SubDepartmentName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = stateList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }

        public JsonResult SubDepartmentByDepartment(int? departmentId)
        {
            var subDepartments = SubDepartmentService.SubDepartments(departmentId).Select(s => new { id = s.Id, name = s.SubDepartmentName }).ToList();

            JsonResult jsonResult = new JsonResult()
            {
                Data = subDepartments,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public JsonResult SupervisorSubDepartmentByDepartment(int? departmentId)
        {
            var loginUser = db.UserInformation.Include("SupervisorSubDepartment.SubDepartment").FirstOrDefault(u => u.Id == SessionHelper.LoginId);
            List<SubDepartment> sub = new List<SubDepartment>();
            if (loginUser != null && loginUser.SupervisorSubDepartment.Count > 0)
            {
                if (departmentId.HasValue && departmentId.Value > 0)
                {
                    sub = loginUser.SupervisorSubDepartment.Where(s => s.SubDepartment.DepartmentId == departmentId || !s.SubDepartment.DepartmentId.HasValue).Select(s => s.SubDepartment).ToList();
                }
                else
                {
                    sub = loginUser.SupervisorSubDepartment.Select(s => s.SubDepartment).ToList();
                }
            }
            else
            {
                sub = db.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                if (departmentId.HasValue && departmentId.Value > 0)
                {
                    sub = sub.Where(d => d.DepartmentId == departmentId || !d.DepartmentId.HasValue).ToList();
                }
            }

            var subDepartments = sub.Select(s => new { id = s.Id, name = s.SubDepartmentName }).ToList();

            JsonResult jsonResult = new JsonResult()
            {
                Data = subDepartments,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public JsonResult AjaxGetSubDepartmentByDepartmentAndCompany(string selectedDepartmentId, string selectedCompanyId)
        {
            List<string> companyIdList = (selectedCompanyId ?? "").Split(',').ToList();
            companyIdList.Remove("");
            List<string> departmentIdList = (selectedDepartmentId ?? "").Split(',').ToList();
            departmentIdList.Remove("");
            var sudDepartmentList = db.SubDepartment
                                .Where(w => (!w.DepartmentId.HasValue || departmentIdList.Contains(w.DepartmentId.ToString())) &&
                                            (!w.CompanyId.HasValue || companyIdList.Contains(w.CompanyId.ToString())) &&
                                            w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId && w.CompanyId.HasValue)
                                .Select(s => new { id = s.Id, name = s.SubDepartmentName + " - " + s.Company.CompanyName + "" }).ToList();
            var sudDepartmentList2 = db.SubDepartment
                                .Where(w => (!w.DepartmentId.HasValue || departmentIdList.Contains(w.DepartmentId.ToString())) &&
                                            (!w.CompanyId.HasValue || companyIdList.Contains(w.CompanyId.ToString())) &&
                                            w.DataEntryStatus == 1 && w.ClientId == SessionHelper.SelectedClientId && !w.CompanyId.HasValue && companyIdList.Count > 0)
                                .Select(s => new { id = s.Id, name = s.SubDepartmentName  }).ToList();
            sudDepartmentList = sudDepartmentList.Union(sudDepartmentList2).ToList();
            //if (sudDepartmentList.Count > 0)
            //    sudDepartmentList.Insert(0, new { id = 0, name = "All" });
            JsonResult jsonResult = new JsonResult()
            {
                Data = sudDepartmentList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };
            return jsonResult;
        }
        public override bool CheckBeforeDelete(int id)
        {
            var subDepartment = db.SubDepartment.Include(u => u.EmploymentHistory)
                                                .Include(u => u.UserInformations)
                                                .Include(u => u.SupervisorSubDepartment)
                                                .FirstOrDefault(c => c.Id == id);
            if (subDepartment.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (subDepartment.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;
            if (subDepartment.SupervisorSubDepartment.Where(t => t.DataEntryStatus == 1).Count() > 0)
                return false;

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
