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

namespace TimeAide.Web.Controllers
{
    public class EmploymentHistoryController : TimeAideWebControllers<EmploymentHistory>
    {
        public override List<EmploymentHistory> OnIndexByUser(List<EmploymentHistory> model, int userId)
        {
            return model = model.OrderByDescending(e => e.Employment.OriginalHireDate).ThenByDescending(o=>o.StartDate).ToList();
        }
        // POST: EmploymentHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(EmploymentHistory employmentHistory)
        {
            if (ModelState.IsValid)
            {
                db.EmploymentHistory.Add(employmentHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employmentHistory);
        }

        // GET: UserContactInformation/Create
        public ActionResult AddEmploymentHistory(int? id)
        {
            EmploymentHistory EmploymentHistory = new EmploymentHistory();
            if (EmploymentHistory == null)
            {
                EmploymentHistory = new EmploymentHistory();
            }
            EmploymentHistory.UserInformationId = id ?? 0;
            ViewBag.SupervisorId = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId), "Id", "FullName");
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName");

            ViewBag.Label = ViewBag.Label + " - Add";
            return PartialView(EmploymentHistory);
        }
        [HttpPost]
        public ActionResult AddEmploymentHistory(EmploymentHistory employmentHistory)
        {
            if (ModelState.IsValid)
            {
                var currentRecord = EmploymentService.GetActiveEmployment(employmentHistory.UserInformationId ?? 0);
                if (currentRecord != null)
                {
                    if (currentRecord.OriginalHireDate.Value > employmentHistory.StartDate)
                    {
                        ModelState.AddModelError("StartDate", "The employment start sate can't be prior to current hiring start date.");
                    }
                }
                var closedEmployments = EmploymentService.GetClosedEmployments(employmentHistory.UserInformationId ?? 0);

                foreach (var eachEmplyment in closedEmployments)
                {
                    if (eachEmplyment.OriginalHireDate.Value > employmentHistory.StartDate)
                    {
                        ModelState.AddModelError("StartDate", "The employment start date can't be prior to previous hiring start date.");
                        break;
                    }

                    if (eachEmplyment.TerminationDate.Value > employmentHistory.StartDate)
                    {
                        ModelState.AddModelError("StartDate", "The employment start date can't be prior to previous hiring end date.");
                        break;
                    }
                }
            }
            if (ModelState.IsValid)
            {
                employmentHistory.EmploymentId = EmploymentService.GetActiveEmployment(employmentHistory.UserInformationId??0).Id;
                db.EmploymentHistory.Add(employmentHistory);
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new
                {
                    id = employmentHistory.UserInformationId
                });
            }
            return GetErrors();
        }

        public override EmploymentHistory OnEdit(EmploymentHistory entity)
        {
            ViewBag.SupervisorId = new SelectList(EmploymentHistoryService.GetSupervisors(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "FullName");
            entity.SelectedSupervisorId = EmploymentHistoryService.GetSupervisorSelectedIds(entity.UserInformationId??0);
            ViewBag.PositionId = new SelectList(db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "PositionName",entity.PositionId);
            return entity;
        }
        // POST: EmploymentHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(EmploymentHistory employmentHistory)
        {
            if (ModelState.IsValid)
            {
                var model = db.EmploymentHistory.FirstOrDefault(e => e.Id == employmentHistory.Id);
                model.StartDate = employmentHistory.StartDate;
                model.PositionId = employmentHistory.PositionId;
                model.LocationId = employmentHistory.LocationId;
                model.DepartmentId = employmentHistory.DepartmentId;
                model.SubDepartmentId = employmentHistory.SubDepartmentId;
                model.EmployeeTypeId = employmentHistory.EmployeeTypeId;
                model.EmploymentTypeId = employmentHistory.EmploymentTypeId;
                model.ModifiedBy = SessionHelper.LoginId;
                model.ModifiedDate = DateTime.Now;
                db.Entry(model).State = EntityState.Modified;

                List<string> supervisorByIds = (employmentHistory.SelectedSupervisorId ?? "").Split(',').ToList();
                //foreach (var eachId in supervisorByIds)
                //{
                //    //int newId;
                //    //Int32.TryParse(eachId, out newId);
                //    //if (newId > 0 && model.EmployeeSupervisor.Where(n => n.SupervisorUserId == newId).Count() == 0)
                //    //    model.EmployeeSupervisor.Add(new EmployeeSupervisor { SupervisorUserId = Convert.ToInt32(eachId), EmployeeUserId = model.UserInformationId ?? 0});
                //}
                db.SaveChanges();
                return RedirectToAction("IndexByUser", new { id = employmentHistory.UserInformationId });
            }
            return GetErrors();
        }

        public override void OnClose(EmploymentHistory entity)
        {
            int userInformationId = entity.UserInformationId ?? 0;
            ViewBag.SupervisorId = new SelectList(db.GetAll<UserInformation>(SessionHelper.SelectedClientId), "Id", "FullName", entity.SupervisorId);
            var list = db.EmployeeSupervisor.Where(e => e.ClientId == SessionHelper.SelectedClientId && e.EmployeeUserId == userInformationId).Select(e => e.SupervisorUser);
            ViewBag.SelectedAuthorizById = new SelectList(list, "Id", "FullName");
        }
        [HttpPost]
        public ActionResult Close(EmploymentHistory employmentHistory)
        {
            ModelState.Remove("StartDate");
            ModelState.Remove("EmployeeTypeId");
            ModelState.Remove("DepartmentId");
            //if (String.IsNullOrEmpty(employmentHistory.SelectedAuthorizById))
            //    ModelState.AddModelError("SelectedAuthorizById", "The Authorize by field is required.");
            if (!employmentHistory.EndDate.HasValue)
                ModelState.AddModelError("EndDate", "The end date field is required.");
            if (!employmentHistory.ApprovedDate.HasValue)
                ModelState.AddModelError("ApprovedDate", "The approved date field is required.");
            var modelFromDb = EmploymentHistoryService.GetEmploymentHistory(employmentHistory.Id,db);
            if (ModelState.IsValid)
            {
               
                if (modelFromDb.StartDate > employmentHistory.EndDate.Value)
                    ModelState.AddModelError("EndDate", "The employment End Date can't be prior to employment start Date.");

                var currentRecord = EmploymentService.GetEmployment(modelFromDb.EmploymentId);
                if (currentRecord.OriginalHireDate > employmentHistory.EndDate.Value)
                    ModelState.AddModelError("EndDate", "The employment End Date can't be prior to Hire Date.");
                if (currentRecord.EffectiveHireDate > employmentHistory.EndDate.Value)
                    ModelState.AddModelError("EndDate", "The employment End Date can't be prior to Re-Hire Date.");

                if (modelFromDb.StartDate > employmentHistory.ApprovedDate.Value)
                    ModelState.AddModelError("ApprovedDate", "The employment Approved Date can't be prior to employment start Date.");

                if (employmentHistory.EndDate.Value > employmentHistory.ApprovedDate.Value)
                    ModelState.AddModelError("ApprovedDate", "The employment Approved Date can't be prior to employment end Date.");

            }
            if (ModelState.IsValid)
            {
                var activeEmploymentRecord = EmploymentService.GetActiveEmployment(employmentHistory.UserInformationId ?? 0);
                if (activeEmploymentRecord != null)
                {
                    if (employmentHistory.EndDate >= activeEmploymentRecord.OriginalHireDate.Value && modelFromDb.EmploymentId != activeEmploymentRecord.Id)
                    {
                        ModelState.AddModelError("EndDate", "The employment end date should be prior to active period hiring date.");
                    }
                }
                var closedEmployments = EmploymentService.GetClosedEmployments(employmentHistory.UserInformationId ?? 0);
                foreach (var eachEmplyment in closedEmployments)
                {
                    if (eachEmplyment.OriginalHireDate.Value > employmentHistory.EndDate && modelFromDb.EmploymentId != eachEmplyment.Id)
                    {
                        ModelState.AddModelError("EndDate", "The employment end date can't be prior to Closed record hiring date.");
                        break;
                    }
                    if (eachEmplyment.TerminationDate.Value > employmentHistory.StartDate && modelFromDb.EmploymentId != eachEmplyment.Id)
                    {
                        ModelState.AddModelError("EndDate", "The employment end date can't be prior to  Closed record hiring end date.");
                        break;
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    
                    modelFromDb.EndDate = employmentHistory.EndDate;
                    modelFromDb.ApprovedDate = employmentHistory.ApprovedDate;
                    modelFromDb.ChangeReason = employmentHistory.ChangeReason;
                    modelFromDb.ModifiedBy = SessionHelper.LoginId;
                    modelFromDb.ModifiedDate = DateTime.Now;
                    modelFromDb.ClosedBy = SessionHelper.LoginId;
                    var existingApproverList = db.EmploymentHistoryAuthorizer.Where(w => w.DataEntryStatus == 1 && w.EmploymentHistoryId == employmentHistory.Id);
                    //foreach (var existingApprover in existingApproverList)
                    //{
                    //    existingApprover.DataEntryStatus = 0;
                    //    existingApprover.ModifiedBy = SessionHelper.LoginId;
                    //    existingApprover.ModifiedDate = DateTime.Now;
                    //}
                    db.EmploymentHistoryAuthorizer.RemoveRange(existingApproverList);
                    db.SaveChanges();
                    List<string> AuthorizByIds = (employmentHistory.SelectedAuthorizById ?? "").Split(',').ToList();
                    
                    foreach (var eachId in AuthorizByIds)
                    {
                       // int newId;
                       // Int32.TryParse(eachId, out newId);
                       // if (newId > 0 && modelFromDb.EmploymentHistoryAuthorizer.Where(n => n.AuthorizeById == newId).Count() == 0)
                       modelFromDb.EmploymentHistoryAuthorizer.Add(new EmploymentHistoryAuthorizer { AuthorizeById = Convert.ToInt32(eachId), EmploymentHistoryId = modelFromDb.Id });
                    }
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                }
                return RedirectToAction("IndexByUser", new { id = employmentHistory.UserInformationId });
            }
            return GetErrors();
        }

        public override void OnDetails(EmploymentHistory entity)
        {
        }
        public override EmploymentHistory OnMostRecentRecord(List<EmploymentHistory> model, int userId)
        {
            var retModel= model.OrderByDescending(e => e.StartDate).FirstOrDefault();
            if (retModel != null)
            {
                var closedByUser = db.UserInformation.Find(retModel.ClosedBy ?? 0);
                var closedByName = closedByUser != null ? closedByUser.ShortFullName : "";
                retModel.ClosedByName = retModel.EndDate == null ? "" : closedByName;
            }
            return retModel;
        }
        public virtual ActionResult ViewSelected(int? id)
        {
            try
            {
                AllowView();
                if (id == null)
                {
                    return PartialView();
                }
                EmploymentHistory model = db.EmploymentHistory.Where(u => u.Id == id && u.DataEntryStatus != 0).OrderByDescending(u => u.CreatedDate).FirstOrDefault();
                if (model == null)
                {
                    return PartialView("Details");
                }
                //return View(city);
                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView("Details", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "City", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public override ActionResult ChangeHistory(int refrenceId)
        {
            try
            {
                AllowChangeHistory();
                var parentEntitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == refrenceId && d.AuditLog.TableName == FormName && d.ClientId == SessionHelper.SelectedClientId).ToList();
                
                var childEntitySet = db.AuditLogDetail.Where(d => (d.AuditLog.ReferenceId1 == refrenceId) && d.AuditLog.TableName == "EmploymentHistoryAuthorizer" && d.ClientId == SessionHelper.SelectedClientId)
                                                        .ToList();
                foreach (var parentEntity in parentEntitySet)
                {
                    if (parentEntity.ColumnName == "ClosedBy")
                    {
                        if (!string.IsNullOrEmpty(parentEntity.NewValue))
                        {
                            var closedById = int.Parse(parentEntity.NewValue);
                            parentEntity.NewValue = db.UserInformation.Find(closedById).ShortFullName;
                        }
                    }
                }
                foreach (var childEntity in childEntitySet)
                {
                    if (childEntity.ColumnName == "AuthorizeById")
                    {
                        childEntity.ColumnName = "ApprovedBy";
                        if (!string.IsNullOrEmpty(childEntity.NewValue))
                        {
                            var approvedById = int.Parse(childEntity.NewValue);
                            childEntity.NewValue = db.UserInformation.Find(approvedById).ShortFullName;
                        }
                    }                    
                }                                      
                var entitySet = parentEntitySet.Union(childEntitySet).OrderBy(o=>o.CreatedDate);
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + refrenceId.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = FormName;
                    ViewBag.RefrenceId = refrenceId;
                    ViewBag.UserInformation = db.UserInformation.FirstOrDefault(u => u.Id == refrenceObject.CreatedBy);
                }
                return PartialView("~/Views/AuditLog/ChangeHistory.cshtml", entitySet);
            }
            catch (AuthorizationException ex)
            {
                if (Form.IsTab)
                {
                    return PartialView();
                }
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmploymentHistory", "Index");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
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
