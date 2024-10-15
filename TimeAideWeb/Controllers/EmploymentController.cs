using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Services;
using TimeAide.Web.Controllers;
using TimeAide.Web.Extensions;
using TimeAide.Web.Models;

namespace WebApplication4.Controllers
{
    public class EmploymentController : TimeAideWebControllers<Employment>
    {
        public override List<Employment> OnIndexByUser(List<Employment> model, int userId)
        {
            return model = model.OrderByDescending(e=>e.OriginalHireDate).ToList();
        }

        public override Employment OnMostRecentRecord(List<Employment> model, int userId)
        {
            return model.OrderByDescending(e => e.OriginalHireDate).FirstOrDefault();
        }

        // POST: Employment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult AddEmployment(Employment employment)
        {
            if (ModelState.IsValid)
            {
                var latestRecord = db.Employment.Where(e => e.DataEntryStatus == 1 && e.UserInformationId == employment.UserInformationId).OrderByDescending(e => e.EffectiveHireDate).FirstOrDefault();
                if (latestRecord != null)
                {
                    if (employment.OriginalHireDate.Value <= latestRecord.OriginalHireDate.Value)
                        ModelState.AddModelError("OriginalHireDate", "The Hire Date can't be prior to previous hirings.");
                }

                if (employment.OriginalHireDate.Value > employment.EffectiveHireDate.Value)
                    ModelState.AddModelError("EffectiveHireDate", "The Re-Hire Date can't be prior to Hire Date.");

                if (!employment.ProbationStartDate.HasValue && employment.ProbationEndDate.HasValue)
                {
                    ModelState.AddModelError("ProbationStartDate", "The Probation Start Date is required or remove Probation End Date.");
                }
                if (employment.ProbationStartDate.HasValue)
                {
                    if (employment.OriginalHireDate.Value > employment.ProbationStartDate.Value)
                        ModelState.AddModelError("ProbationStartDate", "The Probation Start Date can't be prior to Hire Date.");
                    if (employment.EffectiveHireDate.Value > employment.ProbationStartDate.Value)
                        ModelState.AddModelError("ProbationStartDate", "The Probation Start Date can't be prior to Re-Hire Date.");
                    if (!employment.ProbationEndDate.HasValue)
                        ModelState.AddModelError("ProbationEndDate", "The Probation End Date is required or remove Probation Start Date.");
                    else if (employment.ProbationStartDate.Value > employment.ProbationEndDate.Value)
                        ModelState.AddModelError("ProbationStartDate", "The Probation End Date can't be prior to Probation Start Date.");
                }
            }
            if (ModelState.IsValid)
            {
                db.Employment.Add(employment);
                db.SaveChanges();
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
                return Json(employment);
            }
            return GetErrors();
        }

        // POST: Employment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Employment employment)
        {
            if (ModelState.IsValid)
            {
                var latestRecord = db.Employment.Where(e => e.DataEntryStatus == 1 && e.UserInformationId == employment.UserInformationId && e.Id != employment.Id).OrderByDescending(e => e.EffectiveHireDate).FirstOrDefault();
                if (latestRecord != null)
                {
                    if (employment.OriginalHireDate.Value <= latestRecord.OriginalHireDate.Value)
                        ModelState.AddModelError("OriginalHireDate", "The Hire Date can't be prior to previous hirings.");
                }

                if (employment.OriginalHireDate.Value > employment.EffectiveHireDate.Value)
                    ModelState.AddModelError("EffectiveHireDate", "The Re-Hire Date can't be prior to Hire Date.");

                if (!employment.ProbationStartDate.HasValue && employment.ProbationEndDate.HasValue)
                {
                    ModelState.AddModelError("ProbationStartDate", "The Probation Start Date is required or remove Probation End Date.");
                }
                if (employment.ProbationStartDate.HasValue)
                {
                    if (employment.OriginalHireDate.Value > employment.ProbationStartDate.Value)
                        ModelState.AddModelError("ProbationStartDate", "The Probation Start Date can't be prior to Hire Date.");
                    if (employment.EffectiveHireDate.Value > employment.ProbationStartDate.Value)
                        ModelState.AddModelError("ProbationStartDate", "The Probation Start Date can't be prior to Re-Hire Date.");
                    if (!employment.ProbationEndDate.HasValue)
                        ModelState.AddModelError("ProbationEndDate", "The Probation End Date is required or remove Probation Start Date.");
                    else if (employment.ProbationStartDate.Value > employment.ProbationEndDate.Value)
                        ModelState.AddModelError("ProbationStartDate", "The Probation End Date can't be prior to Probation Start Date.");
                }
            }
            if (ModelState.IsValid)
            {
                var model = db.Employment.FirstOrDefault(e => e.Id == employment.Id);
                model.OriginalHireDate = employment.OriginalHireDate;
                model.EffectiveHireDate = employment.EffectiveHireDate;
                model.ProbationStartDate = employment.ProbationStartDate;
                model.ProbationEndDate = employment.ProbationEndDate;
                model.EmploymentStatusId = employment.EmploymentStatusId;
                model.UseHireDateforYearsInService = employment.UseHireDateforYearsInService;
                db.SaveChanges();
                return Json(employment);
                //return RedirectToAction("IndexByUser", new { id = employment.UserInformationId });
            }
            return GetErrors();
        }


        [HttpGet]
        public ActionResult Activate(int? id)
        {
            UserActivationViewModel model = new UserActivationViewModel();
            model.UserInformationId = id ?? 0;
            model.CompanyList = db.GetAll<Company>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.CompanyName });
            model.DepartmentList = db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId,SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.DepartmentName });
            model.EmployeeTypeList = db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EmployeeTypeName });
            model.EmploymentTypeList = db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EmploymentTypeName });
            model.EmploymentStatusList = db.GetAll<EmploymentStatus>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EmploymentStatusName });
            model.PositionList = db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.PositionName });
            model.RateFrequencyList = db.GetAll<RateFrequency>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.RateFrequencyName });
            model.PayFrequencyList = db.GetAll<PayFrequency>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.PayFrequencyName });
            model.PayTypeList = db.GetAll<PayType>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.PayTypeName });
            model.EEOCategoryList = db.GetAllByCompany<EEOCategory>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EEOCategoryName });

            var userInformationEntity = db.UserInformation.Where(w => w.Id == model.UserInformationId).FirstOrDefault();
            model.CompanyId = userInformationEntity.CompanyId;

            var employmentHiringEntity = db.Employment.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.TerminationDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHiringEntity != null)
            {
                model.EmploymentId = employmentHiringEntity.Id;
                model.OriginalHireDate = employmentHiringEntity.OriginalHireDate;
                model.ProbationStartDate = employmentHiringEntity.ProbationStartDate;
                model.ProbationEndDate = employmentHiringEntity.ProbationEndDate;
                model.EffectiveHireDate = employmentHiringEntity.EffectiveHireDate;
                model.EmploymentStatusId = employmentHiringEntity.EmploymentStatusId;

            }
            var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.EmploymentId== (model.EmploymentId??0) && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHistoryEntity != null)
            {
                model.EmploymentHistoryId = employmentHistoryEntity.Id;
                model.EmploymentStartDate = employmentHistoryEntity.StartDate;
                model.DepartmentId = employmentHistoryEntity.DepartmentId;
                model.CompanyId = employmentHistoryEntity.CompanyId;
                model.PositionId = employmentHistoryEntity.PositionId;
                model.EmployeeTypeId = employmentHistoryEntity.EmployeeTypeId;
                model.EmploymentTypeId = employmentHistoryEntity.EmploymentTypeId;


            }
            var payInfoHistoryEntity = db.PayInformationHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.EmploymentId == (model.EmploymentId ?? 0) && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (payInfoHistoryEntity != null)
            {
                model.PayInformationHistoryId = payInfoHistoryEntity.Id;
                model.PayStartDate = payInfoHistoryEntity.StartDate;
                model.RateAmount = payInfoHistoryEntity.RateAmount;
                model.RateFrequencyId = payInfoHistoryEntity.RateFrequencyId;
                // model.PositionId = payInfoHistoryEntity.PositionId;
                model.PayFrequencyId = payInfoHistoryEntity.PayFrequencyId;
                model.PeriodHours = payInfoHistoryEntity.PeriodHours??0;
                model.PeriodGrossPay = payInfoHistoryEntity.PeriodGrossPay;
                model.YearlyGrossPay = payInfoHistoryEntity.YearlyGrossPay;
                model.EEOCategoryId = payInfoHistoryEntity.EEOCategoryId;
                model.PayTypeId = payInfoHistoryEntity.PayTypeId;

            }
            return PartialView("_ActivateEmploymentPop", model);
        }
        [HttpPost]
        public JsonResult Activate(UserActivationViewModel model)
        {
            string status = "Success";
            string message = "User is successfuly activated!";
            var userStatus = new { id = 0, name = "" };
            try
            {
                var userInformationEntity = db.UserInformation.Find(model.UserInformationId);
                Employment EmploymentHiringEntity = null;
                EmploymentHistory EmploymentHistoryEntity = null;
                PayInformationHistory PayInfoHistoryEntity = null;
                //Employment Detail
                if (model.EmploymentId == null)
                {
                    EmploymentHiringEntity = new Employment();
                    EmploymentHiringEntity.UserInformationId = model.UserInformationId;
                    //EmploymentHiringEntity.CreatedBy = SessionHelper.LoginId;
                    //EmploymentHiringEntity.CreatedDate = DateTime.Now;
                    db.Employment.Add(EmploymentHiringEntity);
                }
                else
                {
                    EmploymentHiringEntity = db.Employment.Find(model.EmploymentId);
                    EmploymentHiringEntity.ModifiedBy = SessionHelper.LoginId;
                    EmploymentHiringEntity.ModifiedDate = DateTime.Now;

                }
                //Hiring detail
                EmploymentHiringEntity.OriginalHireDate = model.OriginalHireDate;
                EmploymentHiringEntity.ProbationStartDate = model.ProbationStartDate;
                EmploymentHiringEntity.ProbationEndDate = model.ProbationEndDate;
                EmploymentHiringEntity.EffectiveHireDate = model.EffectiveHireDate;
                EmploymentHiringEntity.EmploymentStatusId = model.EmploymentStatusId;
                if (model.EmploymentId == null)
                {
                    db.SaveChanges();
                }

                //Employment History Detail
                if (model.EmploymentHistoryId == null)
                {
                    EmploymentHistoryEntity = new EmploymentHistory();
                    EmploymentHistoryEntity.UserInformationId = model.UserInformationId;
                    //EmploymentHistoryEntity.CreatedBy = SessionHelper.LoginId;
                    //EmploymentHistoryEntity.CreatedDate = DateTime.Now;
                    db.EmploymentHistory.Add(EmploymentHistoryEntity);
                }
                else
                {
                    EmploymentHistoryEntity = db.EmploymentHistory.Find(model.EmploymentHistoryId);
                    EmploymentHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                    EmploymentHistoryEntity.ModifiedDate = DateTime.Now;
                }
                // Pay info History
                if (model.PayInformationHistoryId == null)
                {
                    PayInfoHistoryEntity = new PayInformationHistory();
                    PayInfoHistoryEntity.UserInformationId = model.UserInformationId;
                    //PayInfoHistoryEntity.CreatedBy = SessionHelper.LoginId;
                    //PayInfoHistoryEntity.CreatedDate = DateTime.Now;
                    db.PayInformationHistory.Add(PayInfoHistoryEntity);

                }
                else
                {
                    PayInfoHistoryEntity = db.PayInformationHistory.Find(model.PayInformationHistoryId);
                    PayInfoHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                    PayInfoHistoryEntity.ModifiedDate = DateTime.Now;

                }

                //Employment History
                EmploymentHistoryEntity.StartDate = model.EmploymentStartDate.Value;
                EmploymentHistoryEntity.DepartmentId = model.DepartmentId.Value;
                EmploymentHistoryEntity.CompanyId = model.CompanyId.Value;
                EmploymentHistoryEntity.PositionId = model.PositionId;
                // EmploymentHistoryEntity.EmployeeTypeId = model.EmployeeTypeId.Value;
                EmploymentHistoryEntity.EmployeeTypeId = model.EmployeeTypeId;
                EmploymentHistoryEntity.EmploymentTypeId = model.EmploymentTypeId.Value;
                EmploymentHistoryEntity.EmploymentId = EmploymentHiringEntity.Id;
                //Pay Info History
                PayInfoHistoryEntity.EmploymentId = EmploymentHiringEntity.Id;
                PayInfoHistoryEntity.StartDate = model.PayStartDate.Value;
                PayInfoHistoryEntity.RateAmount = model.RateAmount;
                PayInfoHistoryEntity.RateFrequencyId = model.RateFrequencyId.Value;
                //PayInfoHistoryEntity.PositionId = model.PositionId.Value;
                PayInfoHistoryEntity.PayFrequencyId = model.PayFrequencyId.Value;
                PayInfoHistoryEntity.PeriodHours = model.PeriodHours;
                PayInfoHistoryEntity.PeriodGrossPay = model.PeriodGrossPay;
                PayInfoHistoryEntity.YearlyGrossPay = model.YearlyGrossPay;
                PayInfoHistoryEntity.YearlyBaseNCommPay = (PayInfoHistoryEntity.YearlyCommBasePay ?? 0) + model.YearlyGrossPay;
                PayInfoHistoryEntity.EEOCategoryId = model.EEOCategoryId;
                PayInfoHistoryEntity.PayTypeId = model.PayTypeId.Value;
                //Activate User
                userInformationEntity.EmployeeStatusId = 1;
                userInformationEntity.EmployeeStatusDate = DateTime.Now;
                // userInformationEntity.DepartmentId = model.DepartmentId.Value;
                // userInformationEntity.EmployeeTypeID = model.EmployeeTypeId;
                // userInformationEntity.EmploymentStatusId = model.EmploymentStatusId;
                // userInformationEntity.PositionId = model.PositionId;


                userInformationEntity.ModifiedBy = SessionHelper.LoginId;
                userInformationEntity.ModifiedDate = DateTime.Now;
                //Save to db
                db.SaveChanges();
                //get active user status detail
                userStatus = db.EmployeeStatus.Select(s => new { id = s.Id, name = s.EmployeeStatusName }).Where(w => w.id == 1).FirstOrDefault();
            }
            catch (Exception ex)
            {
                TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message, userStatus = userStatus });
        }

        [HttpGet]
        public ActionResult Deactivate(int? id)
        {
            var model = new Employment();
            var userInfo = db.UserInformation.Find(id);
            model.UserInformation = userInfo;
            if (userInfo.EmployeeStatusId == 3)
            {
                var employmentEntity = db.Employment.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.TerminationDate!=null).OrderByDescending(o => o.Id).Include(i => i.UserInformation).FirstOrDefault();
                var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == id && w.DataEntryStatus == 1 && w.EndDate!=null).OrderByDescending(o => o.Id).FirstOrDefault();
                var authorizedBy=db.EmploymentHistoryAuthorizer.Where(w => w.DataEntryStatus == 1 && w.EmploymentHistoryId == employmentHistoryEntity.Id).FirstOrDefault();
                if (employmentEntity != null)
                {
                    model = employmentEntity;
                    //model.TerminationDate= employmentEntity.TerminationDate ;
                    //model.TerminationTypeId= employmentEntity.TerminationTypeId;
                    //model.TerminationReasonId= employmentEntity.TerminationReasonId;
                    //model.TerminationEligibilityId=employmentEntity.TerminationEligibilityId;
                    //model.TerminationNotes=employmentEntity.TerminationNotes;

                }
                if (employmentHistoryEntity != null)
                {
                    model.ApprovedDate = employmentHistoryEntity.ApprovedDate;
                }
                if (authorizedBy != null)
                {
                    model.ApprovedById = authorizedBy.AuthorizeById;
                }
            }

            ViewBag.TerminationEligibilityId = new SelectList(db.GetAll<TerminationEligibility>(SessionHelper.SelectedClientId), "Id", "TerminationEligibilityName",model.TerminationEligibilityId);
            ViewBag.TerminationReasonId = new SelectList(db.GetAll<TerminationReason>(SessionHelper.SelectedClientId), "Id", "TerminationReasonName",model.TerminationReasonId);
            ViewBag.TerminationTypeId = new SelectList(db.GetAll<TerminationType>(SessionHelper.SelectedClientId), "Id", "TerminationTypeName",model.TerminationTypeId);           
            //Employee superviser
            ViewBag.ApprovedById = new SelectList(db.EmployeeSupervisor.Where(e => e.ClientId == SessionHelper.SelectedClientId && e.EmployeeUserId == id).Select(e => e.SupervisorUser), "Id", "ShortFullName",model.ApprovedById);
            ViewBag.EmployeeStatusIdDA = new SelectList(db.EmployeeStatus.Where(w => w.DataEntryStatus == 1 & w.Id>1 && w.Id<4), "Id", "EmployeeStatusName", 2).OrderBy(o => o.Text);
            model.LockAccount = true;
            return PartialView("_DeactivateEmploymentPop", model);
        }
        [HttpPost]
        public JsonResult Deactivate(Employment model)
        {
            string status = "Success";
            string message = "User is successfuly deactivated!";
            var userStatus = new { id = 0, name = "" };
            JavaScriptSerializer ser = new JavaScriptSerializer();
            model = ser.Deserialize<Employment>(Request.Form["DeactivationData"]);

            var employeeStatusId = db.UserInformation.Find(model.UserInformationId).EmployeeStatusId;
            var employmentEntity = db.Employment.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.TerminationDate == null).OrderByDescending(o => o.Id).Include(i => i.UserInformation).FirstOrDefault();
            if(employmentEntity==null && (employeeStatusId == 3|| employeeStatusId == 2))
            {
                employmentEntity = db.Employment.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 ).OrderByDescending(o => o.Id).Include(i => i.UserInformation).FirstOrDefault();
            }
            if (employmentEntity == null)
            {
                status = "Error";
                message = "Hiring detail is not available";
            }
            else
            {
                using (var terminationDBTrans = db.Database.BeginTransaction())
                {
                    try
                    {
                        employmentEntity.TerminationDate = model.TerminationDate;
                        employmentEntity.TerminationTypeId = model.TerminationTypeId;
                        employmentEntity.TerminationReasonId = model.TerminationReasonId;
                        employmentEntity.TerminationEligibilityId = model.TerminationEligibilityId;
                        employmentEntity.IsExitInterview = model.IsExitInterview;
                        employmentEntity.TerminationNotes = model.TerminationNotes;
                        employmentEntity.ModifiedBy = SessionHelper.LoginId;
                        employmentEntity.ModifiedDate = DateTime.Now;
                        employmentEntity.UserInformation.EmployeeStatusId = model.EmployeeStatusId;
                        employmentEntity.UserInformation.EmployeeStatusDate = DateTime.Now;
                        employmentEntity.UserInformation.ModifiedBy = SessionHelper.LoginId;
                        employmentEntity.UserInformation.ModifiedDate = DateTime.Now;
                        if (model.IsExitInterviewMarkDel == true)
                        {
                            employmentEntity.ExitInterviewDocPath = null;
                            employmentEntity.ExitInterviewDocName = null;
                        }
                        if (Request.Files.Count > 0)
                        {
                            HttpPostedFileBase exitDocFile = Request.Files[0];
                            var docName = exitDocFile.FileName;
                            var fileExt = Path.GetExtension(exitDocFile.FileName);
                            //var docName = "Employee-" + model.UserInformationId + "-ExitInterviewDoc" + fileExt;
                            var applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                            
                            FilePathHelper filePathHelper = new FilePathHelper(SessionHelper.SelectedClientId, SessionHelper.SelectedCompanyId, applicationPath);
                            var serverFilePath = filePathHelper.GetPath("ExitInterviewDoc", ref docName, employmentEntity.UserInformationId??0, employmentEntity.Id);
                            //var serverFilePath = filePathHelper.GetPath("ExitInterview", docName);
                            exitDocFile.SaveAs(serverFilePath);
                            var relativeFilePath = filePathHelper.RelativePath;
                            employmentEntity.ExitInterviewDocPath = relativeFilePath;
                            employmentEntity.ExitInterviewDocName = docName;
                           // db.SaveChanges();
                        }
                        var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
                        if(employmentHistoryEntity==null && employeeStatusId == 3)
                        {
                            employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1).OrderByDescending(o => o.Id).FirstOrDefault();
                        }
                        if (employmentHistoryEntity != null)
                        {
                            employmentHistoryEntity.EndDate = model.TerminationDate;
                            //employmentHistoryEntity.AuthorizeById = model.ApprovedById;
                            employmentHistoryEntity.ApprovedDate = model.ApprovedDate;
                            employmentHistoryEntity.ChangeReason = model.TerminationNotes;
                            employmentHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                            employmentHistoryEntity.ModifiedDate = DateTime.Now;
                           employmentHistoryEntity.ClosedBy = SessionHelper.LoginId; 

                            if (model.ApprovedById != null)
                            {
                               var existingApproverList= db.EmploymentHistoryAuthorizer.Where(w => w.DataEntryStatus == 1 && w.EmploymentHistoryId == employmentHistoryEntity.Id);
                                db.EmploymentHistoryAuthorizer.RemoveRange(existingApproverList);
                                //foreach(var existingApprover in existingApproverList)
                                //{
                                //    existingApprover.DataEntryStatus = 0;
                                //    existingApprover.ModifiedBy = SessionHelper.LoginId;
                                //    existingApprover.ModifiedDate = DateTime.Now;
                                //}
                                db.EmploymentHistoryAuthorizer.Add(new EmploymentHistoryAuthorizer { AuthorizeById = model.ApprovedById, EmploymentHistoryId = employmentHistoryEntity.Id });
                            }
                        }
                        var payInfoHistoryEntity = db.PayInformationHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
                        if(payInfoHistoryEntity == null && employeeStatusId == 3)
                        {
                            payInfoHistoryEntity = db.PayInformationHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1).OrderByDescending(o => o.Id).FirstOrDefault();
                        }
                        if (payInfoHistoryEntity != null)
                        {
                            payInfoHistoryEntity.EndDate = model.TerminationDate;
                            if (model.ApprovedById != null)
                            {
                                if (!payInfoHistoryEntity.PayInformationHistoryAuthorizer.Any(a => a.AuthorizeById == model.ApprovedById && a.DataEntryStatus == 1))
                                {
                                    payInfoHistoryEntity.PayInformationHistoryAuthorizer.Add(new PayInformationHistoryAuthorizer() { AuthorizeById = model.ApprovedById, PayInformationHistoryId = payInfoHistoryEntity.Id });
                                }
                            }
                            payInfoHistoryEntity.ChangeReason = model.TerminationNotes;
                            payInfoHistoryEntity.ApprovedDate = model.ApprovedDate;
                            payInfoHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                            payInfoHistoryEntity.ModifiedDate = DateTime.Now;
                        }
                        db.SaveChanges();
                        userStatus = db.EmployeeStatus.Select(s => new { id = s.Id, name = s.EmployeeStatusName }).Where(w => w.id == model.EmployeeStatusId).FirstOrDefault();
                        var userContactInformation = db.UserContactInformation.Where(w => w.UserInformationId == model.UserInformationId).FirstOrDefault();
                        if (userContactInformation != null)
                        {
                            if (!string.IsNullOrEmpty(userContactInformation.LoginEmail))
                            {
                                if (model.LockAccount)
                                {
                                    string errorMessage = "";
                                    if (!UserManagmentService.SetLockout(userContactInformation.LoginEmail, out errorMessage))
                                    {
                                        throw new Exception(errorMessage);
                                    }
                                }
                                else
                                {
                                    if (!UserManagmentService.SetLockout(userContactInformation.LoginEmail, DateTime.UtcNow))
                                    {
                                        throw new Exception("Error while un-locking the user account.");
                                    }
                                }
                            }
                        }
                        terminationDBTrans.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        terminationDBTrans.Rollback();
                        TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        status = "Error";
                        message = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        terminationDBTrans.Rollback();
                        TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        status = "Error";
                        message = ex.Message;
                    }
                }
            }

            return Json(new { status = status, message = message, userStatus = userStatus });
        }
        [HttpPost]
        public JsonResult AjaxCheckForValidEmployment(int userID)
        {
            dynamic retResult = null;
            string status = "Success";
            string message = "";             
            try
               {
                 var employmentEntity = db.Employment.Where(w => w.UserInformationId == userID && w.DataEntryStatus == 1 && w.TerminationDate == null).OrderByDescending(o => o.Id).Include(i => i.UserInformation).FirstOrDefault();
                 if (employmentEntity == null)
                   {
                        employmentEntity = db.Employment.Where(w => w.UserInformationId == userID && w.DataEntryStatus == 1).OrderByDescending(o => o.Id).Include(i => i.UserInformation).FirstOrDefault();
                   }
                 if (employmentEntity == null)
                   {
                    status = "Error";
                    message = "Employee must have a hiring record before they can be deactivated.";

                    }
                }
                catch (Exception ex)
                {                  
                    status = "Error";
                    message = ex.Message;
                }
           
            retResult = new { status = status, message = message };
            return Json(retResult);
        }
        public ActionResult DownloadExitInterviewDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var empt = db.Employment.Find(id);
            if (empt != null)
            {
                if (!string.IsNullOrEmpty(empt.ExitInterviewDocPath))
                {
                    relativeFilePath = empt.ExitInterviewDocPath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadDocFile = new FileInfo(serverFilePath);

                    if (downloadDocFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadDocFile.Name);
                    }
                }

            }

            return null;

        }
        public ActionResult DownloadContractDocument(int id)
        {
            string relativeFilePath = "", serverFilePath = "";
            FileInfo downloadDocFile = null;
            byte[] fileBytes;
            var empt = db.Employment.Find(id);
            if (empt != null)
            {
                if (!string.IsNullOrEmpty(empt.DocumentPath))
                {
                    relativeFilePath = empt.DocumentPath;
                    var tempPath = "~" + relativeFilePath;
                    serverFilePath = Server.MapPath(tempPath);
                    downloadDocFile = new FileInfo(serverFilePath);

                    if (downloadDocFile.Exists)
                    {
                        fileBytes = System.IO.File.ReadAllBytes(serverFilePath);
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadDocFile.Name);
                    }
                }

            }

            return null;

        }
        //[HttpPost]
        //public JsonResult AjaxDeleteExitInterviewDocument(int id)
        //{
        //    string status = "Success";
        //    string message = "Exit Interview File is Successfully Removed!";
        //    string relativeFilePath = "", serverFilePath = "";
        //    FileInfo deleteExitInterviewFile = null;
        //    var employmentEntityEntity = db.Employment.Find(id);
        //    try
        //    {
        //        relativeFilePath = employmentEntityEntity.ExitInterviewDocPath;
        //        var tempPath = "~" + relativeFilePath;
        //        serverFilePath = Server.MapPath(tempPath);
        //        deleteExitInterviewFile = new FileInfo(serverFilePath);

        //        employmentEntityEntity.ExitInterviewDocPath = null;
        //        employmentEntityEntity.ExitInterviewDocName = null;
        //        employmentEntityEntity.ModifiedBy = SessionHelper.LoginId;
        //        employmentEntityEntity.ModifiedDate = DateTime.Now;

        //        db.SaveChanges();

        //        deleteExitInterviewFile.Delete();
        //    }
        //    catch (Exception ex)
        //    {
        //        TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
        //        status = "Error";
        //        message = ex.Message;
        //    }
        //    return Json(new { status = status, message = message });

        //}
        [HttpGet]
        public ActionResult EmployeeTransfer(int? id)
        {
            UserActivationViewModel model = new UserActivationViewModel();
            model.UserInformationId = id ?? 0;
            model.CompanyList = db.GetAll<Company>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.CompanyName });
            model.DepartmentList = db.GetAllByCompany<Department>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w=>id == 0).Select(s => new { id = s.Id, text = s.DepartmentName });
            model.SubDepartmentList = db.GetAllByCompany<SubDepartment>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => id == 0).Select(s => new { id = s.Id, text = s.SubDepartmentName });
            model.EmployeeTypeList = db.GetAllByCompany<EmployeeType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => id == 0).Select(s => new { id = s.Id, text = s.EmployeeTypeName });
            model.EmploymentTypeList = db.GetAllByCompany<EmploymentType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => id == 0).Select(s => new { id = s.Id, text = s.EmploymentTypeName });
            model.EmploymentStatusList = db.GetAll<EmploymentStatus>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EmploymentStatusName });
            model.PositionList = db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => id == 0).Select(s => new { id = s.Id, text = s.PositionName });
            model.RateFrequencyList = db.GetAll<RateFrequency>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.RateFrequencyName });
            model.PayFrequencyList = db.GetAll<PayFrequency>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.PayFrequencyName });
            model.PayTypeList = db.GetAll<PayType>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.PayTypeName });
            model.EEOCategoryList = db.GetAll<EEOCategory>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EEOCategoryName });
            model.TerminationTypeList = db.GetAll<TerminationType>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.TerminationTypeName });
            model.TerminationReasonList = db.GetAll<TerminationReason>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.TerminationReasonName });
            model.SuperviserList = db.EmployeeSupervisor.Where(e => e.ClientId == SessionHelper.SelectedClientId && e.EmployeeUserId == id).Select(e => e.SupervisorUser).Select(s => new { id = s.Id, text = s.ShortFullName });

            var userInformationEntity = db.UserInformation.Where(w => w.Id == model.UserInformationId).FirstOrDefault();
            model.CompanyId = userInformationEntity.CompanyId;
            model.ToCompanyList = db.GetAll<Company>(SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.CompanyName }).Where(w => w.id != model.CompanyId).OrderBy(o=>o.text);
            model.EmployeeId = userInformationEntity.EmployeeId;
            model.TerminationDate = DateTime.Today;
            var employmentHiringEntity = db.Employment.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.TerminationDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHiringEntity != null)
            {
                model.EmploymentId = employmentHiringEntity.Id;
                model.OriginalHireDate = employmentHiringEntity.OriginalHireDate;
                model.ProbationStartDate = employmentHiringEntity.ProbationStartDate;
                model.ProbationEndDate = employmentHiringEntity.ProbationEndDate;
                //model.EffectiveHireDate = employmentHiringEntity.EffectiveHireDate;
                model.EffectiveHireDate = DateTime.Today;
                model.EmploymentStatusId = employmentHiringEntity.EmploymentStatusId;

            }
            var employmentHistoryEntity = db.EmploymentHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.EmploymentId == (model.EmploymentId??0) && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (employmentHistoryEntity != null)
            {
                model.EmploymentHistoryId = employmentHistoryEntity.Id;
                //model.EmploymentStartDate = employmentHistoryEntity.StartDate;
                model.EmploymentStartDate = DateTime.Today;
               // model.DepartmentId = employmentHistoryEntity.DepartmentId;
               // model.SubDepartmentId = employmentHistoryEntity.SubDepartmentId;
                //  model.CompanyId = employmentHistoryEntity.CompanyId;
            //    model.PositionId = employmentHistoryEntity.PositionId;
             //   model.EmployeeTypeId = employmentHistoryEntity.EmployeeTypeId;
             //   model.EmploymentTypeId = employmentHistoryEntity.EmploymentTypeId;


            }
            var payInfoHistoryEntity = db.PayInformationHistory.Where(w => w.UserInformationId == model.UserInformationId && w.DataEntryStatus == 1 && w.EmploymentId == (model.EmploymentId ?? 0) && w.EndDate == null).OrderByDescending(o => o.Id).FirstOrDefault();
            if (payInfoHistoryEntity != null)
            {
                model.PayInformationHistoryId = payInfoHistoryEntity.Id;
                //model.PayStartDate = payInfoHistoryEntity.StartDate;
                model.PayStartDate = DateTime.Today;
                model.RateAmount = payInfoHistoryEntity.RateAmount;
                model.RateFrequencyId = payInfoHistoryEntity.RateFrequencyId;
                // model.PositionId = payInfoHistoryEntity.PositionId;
                model.PayFrequencyId = payInfoHistoryEntity.PayFrequencyId;
                model.PeriodHours = payInfoHistoryEntity.PeriodHours ?? 0;
                model.PeriodGrossPay = payInfoHistoryEntity.PeriodGrossPay;
                model.YearlyGrossPay = payInfoHistoryEntity.YearlyGrossPay;
                model.EEOCategoryId = payInfoHistoryEntity.EEOCategoryId;
                model.PayTypeId = payInfoHistoryEntity.PayTypeId;

            }
            return PartialView("_EmployeeTransferPop", model);
        }
        public ActionResult EmployeeTransferCmpBasedEmploymentData(int? id)
        {
            var transferCmpId = id ?? 0;
            UserActivationViewModel model = new UserActivationViewModel();
            model.EmploymentStartDate = DateTime.Today;
            model.DepartmentList = db.GetAllByCompany<Department>(transferCmpId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.DepartmentName });
            model.SubDepartmentList = db.GetAllByCompany<SubDepartment>(transferCmpId, SessionHelper.SelectedClientId).Where(w => id == 0).Select(s => new { id = s.Id, text = s.SubDepartmentName });
            model.EmployeeTypeList = db.GetAllByCompany<EmployeeType>(transferCmpId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EmployeeTypeName });
            model.EmploymentTypeList = db.GetAllByCompany<EmploymentType>(transferCmpId, SessionHelper.SelectedClientId).Select(s => new { id = s.Id, text = s.EmploymentTypeName });
            model.PositionList = db.GetAllByCompany<Position>(transferCmpId, SessionHelper.SelectedClientId).Select(s => new {id = s.Id, text = s.PositionName });

            return PartialView(model);
        }
        public JsonResult GetTransferToCmpSubDeptByDept(int? departmentId, int TransferToCmpId)
        {
          var subDepartments=  db.GetAllByCompany<SubDepartment>(TransferToCmpId, SessionHelper.SelectedClientId)
                .Where(c => !c.DepartmentId.HasValue || !departmentId.HasValue || c.DepartmentId == departmentId)
                .Select(s => new { id = s.Id, name = s.SubDepartmentName }).ToList(); ;

            JsonResult jsonResult = new JsonResult()
            {
                Data = subDepartments,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        [HttpPost]
        public JsonResult EmployeeTransfer(UserActivationViewModel model)
        {
            string status = "Success";
            string transactionPoint = "";
            
            int statusCode = 0; // 0- success, -1 Employee Id exist, -2 SSN exists, -3 exception
            string message = "Employee is successfuly Transferred!";
            var userStatus = new { id = 0, name = "" };
            UserInformation fromUserInformationEntity = null, toUserInformationEntity = null;
            Employment EmploymentHiringEntity = null, EmploymentHiringTransferEntity = null;
            EmploymentHistory EmploymentHistoryEntity = null, EmploymentHistoryTransferEntity = null;
            PayInformationHistory PayInfoHistoryEntity = null, PayInfoHistoryTranferEntity = null; ;
            int? refUserInformationId;
            int? refEmployeeId;
            fromUserInformationEntity = db.UserInformation.Find(model.UserInformationId);
            refUserInformationId = fromUserInformationEntity.RefUserInformationId == null ? fromUserInformationEntity.Id : fromUserInformationEntity.RefUserInformationId;

            var chkEmpIdFromTransfer = db.EmployeeCompanyTransfer.Where(w => w.UserInformationId == refUserInformationId &&
                                                                                w.FromCompanyId == model.ToCompanyId && w.DataEntryStatus == 1).OrderByDescending(o => o.Id).FirstOrDefault();

            refEmployeeId = chkEmpIdFromTransfer == null ? model.EmployeeId : chkEmpIdFromTransfer.FromUserInformation.EmployeeId;

            int IsEmployeeIdExist =
            db.UserInformation.Where(w => w.EmployeeId == refEmployeeId && w.CompanyId == model.ToCompanyId && (w.RefUserInformationId ?? 0) != refUserInformationId)
                                .Count();

            if (IsEmployeeIdExist == 0)
            {// Transfer employee handling multiple transactions
                using (var employeeTransferDBTrans = db.Database.BeginTransaction())
                {
                    try
                    {
                        EmployeeCompanyTransfer employeeCompanyTransferHistoryEntity = db.EmployeeCompanyTransfer.Where(w => w.UserInformationId == refUserInformationId &&
                                                                                  w.FromCompanyId == model.ToCompanyId && w.DataEntryStatus == 1).OrderByDescending(o => o.Id).FirstOrDefault();

                        fromUserInformationEntity.AspNetUserId = null;
                        if (employeeCompanyTransferHistoryEntity == null)
                        {
                            var tempEntity = db.Entry(fromUserInformationEntity).CurrentValues.ToObject();
                            toUserInformationEntity = (UserInformation)tempEntity;
                            toUserInformationEntity.Id = 0;
                            toUserInformationEntity.CompanyId = model.ToCompanyId;
                            toUserInformationEntity.EmployeeId = model.EmployeeId;
                            toUserInformationEntity.RefUserInformationId = refUserInformationId;
                            toUserInformationEntity.CreatedBy = SessionHelper.LoginId;
                            toUserInformationEntity.CreatedDate = DateTime.Now;
                            toUserInformationEntity.ModifiedBy = SessionHelper.LoginId;
                            toUserInformationEntity.ModifiedDate = DateTime.Now;
                            toUserInformationEntity.EmployeeStatusId = 1; //Active
                                                                          //Add replica user in new company
                            db.UserInformation.Add(toUserInformationEntity);


                        }
                        else
                        {
                            toUserInformationEntity = db.UserInformation.Find(employeeCompanyTransferHistoryEntity.FromUserInformationId);
                            //toUserInformationEntity.CompanyId = model.ToCompanyId;                            
                            toUserInformationEntity.RefUserInformationId = refUserInformationId;
                            toUserInformationEntity.FirstName = fromUserInformationEntity.FirstName;
                            toUserInformationEntity.MiddleInitial = fromUserInformationEntity.MiddleInitial;
                            toUserInformationEntity.FirstLastName = fromUserInformationEntity.FirstLastName;
                            toUserInformationEntity.SecondLastName = fromUserInformationEntity.SecondLastName;
                            toUserInformationEntity.ShortFullName = fromUserInformationEntity.ShortFullName;
                            toUserInformationEntity.BirthDate = fromUserInformationEntity.BirthDate;
                            toUserInformationEntity.BirthPlace = fromUserInformationEntity.BirthPlace;
                            toUserInformationEntity.GenderId = fromUserInformationEntity.GenderId;
                            toUserInformationEntity.MaritalStatusId = fromUserInformationEntity.MaritalStatusId;
                            toUserInformationEntity.SSNEnd = fromUserInformationEntity.SSNEnd;
                            toUserInformationEntity.SSN = fromUserInformationEntity.SSN;
                            toUserInformationEntity.DisabilityId = fromUserInformationEntity.DisabilityId;
                            toUserInformationEntity.EthnicityId = fromUserInformationEntity.EthnicityId;
                            toUserInformationEntity.CreatedBy = SessionHelper.LoginId;
                            toUserInformationEntity.CreatedDate = DateTime.Now;
                            toUserInformationEntity.ModifiedBy = SessionHelper.LoginId;
                            toUserInformationEntity.ModifiedDate = DateTime.Now;
                            toUserInformationEntity.AspNetUserId = null;
                            toUserInformationEntity.EmployeeStatusId = 1; //Active

                        }
                        //fromUserInformationEntity = db.UserInformation.Find(model.UserInformationId);
                        fromUserInformationEntity.EmployeeStatusId = 4; // Employee Transfer
                        fromUserInformationEntity.ModifiedDate = DateTime.Now;
                        fromUserInformationEntity.ModifiedBy = SessionHelper.LoginId;
                        fromUserInformationEntity.RefUserInformationId = refUserInformationId;
                        //Save user information
                        transactionPoint = " (while saving user information)";
                        db.SaveChanges();
                        var EmployeeFromVeteranStatusList = db.EmployeeVeteranStatus.Where(w => w.UserInformationId == fromUserInformationEntity.Id);
                        var EmployeeToVeteranStatusList = db.EmployeeVeteranStatus.Where(w => w.UserInformationId == toUserInformationEntity.Id);
                        if (EmployeeFromVeteranStatusList.Count() > 0)
                        {//Get from user and populate to user
                            foreach (var employeeFromVeteranStatus in EmployeeFromVeteranStatusList)
                            {
                                //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                                var tempVeteranStatusEntity = (EmployeeVeteranStatus)db.Entry(employeeFromVeteranStatus).CurrentValues.ToObject();
                                tempVeteranStatusEntity.UserInformationId = toUserInformationEntity.Id;//set the to user id
                                tempVeteranStatusEntity.ModifiedBy = SessionHelper.LoginId;
                                tempVeteranStatusEntity.ModifiedDate = DateTime.Today;
                                db.EmployeeVeteranStatus.Add(tempVeteranStatusEntity);
                            }
                        }
                        if (EmployeeToVeteranStatusList.Count() > 0)
                        {//remove existing veteran from to user
                            foreach (var employeeToVeteranStatus in EmployeeToVeteranStatusList)
                            {
                                db.EmployeeVeteranStatus.Remove(employeeToVeteranStatus);
                            }
                        }
                        //Save changes for veteran records
                        transactionPoint = " (while saving veteran record)";
                        db.SaveChanges();
                        //Insert the transfer employee row in Employee Transfer History table
                        var empCompanyTransferHistory = new EmployeeCompanyTransfer();
                        //From information
                        empCompanyTransferHistory.UserInformationId = refUserInformationId;
                        empCompanyTransferHistory.FromCompanyId = fromUserInformationEntity.CompanyId ?? 0;
                        empCompanyTransferHistory.FromUserInformationId = fromUserInformationEntity.Id;
                        //To Information
                        empCompanyTransferHistory.ToCompanyId = toUserInformationEntity.CompanyId ?? 0;
                        empCompanyTransferHistory.ToUserInformationId = toUserInformationEntity.Id;
                        empCompanyTransferHistory.TransferDate = model.TerminationDate ?? DateTime.Today;
                        db.EmployeeCompanyTransfer.Add(empCompanyTransferHistory);
                        //Save transfer history in EmployeeCompanyTransfer table
                        db.SaveChanges();

                        //Employment Detail
                        if (model.EmploymentId > 0)
                        {
                            EmploymentHiringEntity = db.Employment.Find(model.EmploymentId);
                            //create new transfer hiring detail
                            //EmploymentHiringTransferEntity = EmploymentHiringEntity;
                            EmploymentHiringTransferEntity = (Employment)db.Entry(EmploymentHiringEntity).CurrentValues.ToObject();
                            EmploymentHiringTransferEntity.Id = 0;
                            EmploymentHiringTransferEntity.UserInformationId = toUserInformationEntity.Id;
                            EmploymentHiringTransferEntity.OriginalHireDate = model.OriginalHireDate;
                            EmploymentHiringTransferEntity.ProbationStartDate = model.ProbationStartDate;
                            EmploymentHiringTransferEntity.ProbationEndDate = model.ProbationEndDate;
                            EmploymentHiringTransferEntity.EffectiveHireDate = model.EffectiveHireDate;
                            EmploymentHiringTransferEntity.EmploymentStatusId = model.EmploymentStatusId;

                            EmploymentHiringTransferEntity.CreatedBy = SessionHelper.LoginId;
                            EmploymentHiringTransferEntity.CreatedDate = DateTime.Now;
                            EmploymentHiringTransferEntity.ModifiedBy = SessionHelper.LoginId;
                            EmploymentHiringTransferEntity.ModifiedDate = DateTime.Now;
                            db.Employment.Add(EmploymentHiringTransferEntity);
                            //update current hiring detail
                            //EmploymentHiringEntity = db.Employment.Find(model.EmploymentId);
                            EmploymentHiringEntity.TerminationDate = model.TerminationDate;
                            EmploymentHiringEntity.TerminationTypeId = model.TerminationTypeId;
                            EmploymentHiringEntity.TerminationReasonId = model.TerminationReasonId;
                            EmploymentHiringEntity.ApprovedById = model.ApprovedById;
                            EmploymentHiringEntity.ModifiedBy = SessionHelper.LoginId;
                            EmploymentHiringEntity.ModifiedDate = DateTime.Now;
                            //Save hiring detail
                            transactionPoint = " (while saving hiring detail)";
                            db.SaveChanges();
                        }

                        //Employment History Detail
                        if (model.EmploymentHistoryId > 0)
                        {
                            // EmploymentHistoryEntity = new EmploymentHistory();
                            EmploymentHistoryEntity = db.EmploymentHistory.Find(model.EmploymentHistoryId);
                            // EmploymentHistoryTransferEntity = EmploymentHistoryEntity;
                            EmploymentHistoryTransferEntity = (EmploymentHistory)db.Entry(EmploymentHistoryEntity).CurrentValues.ToObject();
                            EmploymentHistoryTransferEntity.UserInformationId = toUserInformationEntity.Id;
                            EmploymentHistoryTransferEntity.EmploymentId = EmploymentHiringTransferEntity.Id;
                            EmploymentHistoryTransferEntity.StartDate = model.EmploymentStartDate.Value;
                            EmploymentHistoryTransferEntity.DepartmentId = model.DepartmentId.Value;
                            EmploymentHistoryTransferEntity.SubDepartmentId = model.SubDepartmentId;
                            EmploymentHistoryTransferEntity.CompanyId = model.ToCompanyId ?? 0;
                            EmploymentHistoryTransferEntity.PositionId = model.PositionId;
                            // EmploymentHistoryEntity.EmployeeTypeId = model.EmployeeTypeId.Value;
                            EmploymentHistoryTransferEntity.EmployeeTypeId = model.EmployeeTypeId;
                            EmploymentHistoryTransferEntity.EmploymentTypeId = model.EmploymentTypeId.Value;
                            EmploymentHistoryTransferEntity.CreatedBy = SessionHelper.LoginId;
                            EmploymentHistoryTransferEntity.CreatedDate = DateTime.Now;
                            EmploymentHistoryTransferEntity.ModifiedBy = SessionHelper.LoginId;
                            EmploymentHistoryTransferEntity.ModifiedDate = DateTime.Now;
                            //Add new transfer employment  detail
                            db.EmploymentHistory.Add(EmploymentHistoryTransferEntity);
                            // EmploymentHistoryEntity = db.EmploymentHistory.Find(model.EmploymentHistoryId);
                            EmploymentHistoryEntity.EndDate = model.TerminationDate;
                            EmploymentHistoryEntity.ApprovedDate = model.TerminationDate;
                            EmploymentHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                            EmploymentHistoryEntity.ModifiedDate = DateTime.Now;
                            //add authorizer detail
                            if (model.ApprovedById != null)
                            {
                                var isExisting = EmploymentHistoryEntity.EmploymentHistoryAuthorizer.Where(w => w.DataEntryStatus == 1 &&                                                                                               
                                                                                                w.AuthorizeById == model.ApprovedById).Count();
                                if (isExisting == 0)
                                {
                                    var empHistoryAuth = new EmploymentHistoryAuthorizer();
                                    empHistoryAuth.EmploymentHistoryId = EmploymentHistoryEntity.Id;
                                    empHistoryAuth.AuthorizeById = model.ApprovedById;
                                    db.EmploymentHistoryAuthorizer.Add(empHistoryAuth);
                                }
                            }
                                //Save Employment History detail
                            transactionPoint = " (while saving employment history detail)";
                            db.SaveChanges();
                        }
                        // Pay info History
                        if (model.PayInformationHistoryId > 0)
                        {
                            PayInfoHistoryEntity = db.PayInformationHistory.Find(model.PayInformationHistoryId);
                            PayInfoHistoryTranferEntity = (PayInformationHistory)db.Entry(PayInfoHistoryEntity).CurrentValues.ToObject();
                            // PayInfoHistoryTranferEntity = PayInfoHistoryEntity;
                            PayInfoHistoryTranferEntity.UserInformationId = toUserInformationEntity.Id;
                            PayInfoHistoryTranferEntity.EmploymentId = EmploymentHiringTransferEntity.Id; // link payinfo with hiring
                            PayInfoHistoryTranferEntity.StartDate = model.PayStartDate.Value;
                            PayInfoHistoryTranferEntity.RateAmount = model.RateAmount;
                            PayInfoHistoryTranferEntity.RateFrequencyId = model.RateFrequencyId.Value;

                            PayInfoHistoryTranferEntity.PayFrequencyId = model.PayFrequencyId.Value;
                            PayInfoHistoryTranferEntity.PeriodHours = model.PeriodHours;
                            PayInfoHistoryTranferEntity.PeriodGrossPay = model.PeriodGrossPay;
                            PayInfoHistoryTranferEntity.YearlyGrossPay = model.YearlyGrossPay;
                            PayInfoHistoryTranferEntity.YearlyBaseNCommPay = (PayInfoHistoryEntity.YearlyCommBasePay ?? 0) + model.YearlyGrossPay;
                            PayInfoHistoryTranferEntity.EEOCategoryId = model.EEOCategoryId;
                            PayInfoHistoryTranferEntity.PayTypeId = model.PayTypeId.Value;
                            PayInfoHistoryTranferEntity.CreatedBy = SessionHelper.LoginId;
                            PayInfoHistoryTranferEntity.CreatedDate = DateTime.Now;
                            PayInfoHistoryTranferEntity.ModifiedBy = SessionHelper.LoginId;
                            PayInfoHistoryTranferEntity.ModifiedDate = DateTime.Now;
                            //Add transfer record
                            db.PayInformationHistory.Add(PayInfoHistoryTranferEntity);

                            //PayInfoHistoryEntity = db.PayInformationHistory.Find(model.PayInformationHistoryId);
                            PayInfoHistoryEntity.EndDate = model.TerminationDate;
                            PayInfoHistoryEntity.ApprovedDate = model.TerminationDate;
                            if (model.ApprovedById != null)
                            {
                                if (!PayInfoHistoryEntity.PayInformationHistoryAuthorizer.Any(a => a.AuthorizeById == model.ApprovedById && a.DataEntryStatus == 1))
                                {
                                    PayInfoHistoryEntity.PayInformationHistoryAuthorizer.Add(new PayInformationHistoryAuthorizer() { AuthorizeById = model.ApprovedById, PayInformationHistoryId = PayInfoHistoryEntity.Id });
                                }
                            }
                            PayInfoHistoryEntity.ModifiedBy = SessionHelper.LoginId;
                            PayInfoHistoryEntity.ModifiedDate = DateTime.Now;
                            //Save PayInfo history changes
                            transactionPoint = " (while saving pay info history detail)";
                            db.SaveChanges();
                        }
                        //Update Contact info
                        transactionPoint = " (while saving Contact Info detail)";
                        EmployeeTransferUpdateContactInfo(fromUserInformationEntity, toUserInformationEntity);
                        transactionPoint = " (while saving Education detail)";
                        EmployeeTransferUpdateEducationInfo(fromUserInformationEntity, toUserInformationEntity);
                        transactionPoint = " (while saving Training detail)";
                        EmployeeTransferUpdateTrainingInfo(fromUserInformationEntity, toUserInformationEntity);
                        transactionPoint = " (while saving Dependents detail)";
                        EmployeeTransferUpdateDependentsInfo(fromUserInformationEntity, toUserInformationEntity);
                        transactionPoint = " (while saving Documents detail)";
                        EmployeeTransferUpdateDocumentInfo(fromUserInformationEntity, toUserInformationEntity);
                        transactionPoint = " (while saving Credentials detail)";
                        EmployeeTransferUpdateCredentialInfo(fromUserInformationEntity, toUserInformationEntity);
                        //Commit all transaction
                        transactionPoint = " (while committing transaction)";
                        employeeTransferDBTrans.Commit();
                        //get user status detail
                        userStatus = db.EmployeeStatus.Select(s => new { id = s.Id, name = s.EmployeeStatusName }).Where(w => w.id == 4).FirstOrDefault();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        string validationErrors = "";
                        foreach (var eve in ex.EntityValidationErrors)
                        {
                            foreach (var ve in eve.ValidationErrors)
                            {
                                validationErrors += String.Format("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        employeeTransferDBTrans.Rollback();
                        status = "Error";
                        statusCode = -3;
                        message = transactionPoint + " " + ex.Message + " \" (" + validationErrors + ")";
                    }
                    catch (Exception ex)
                    {
                        TimeAide.Web.Helpers.ErrorLogHelper.InsertLog(TimeAide.Web.Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        //Rollback all employee transfer db transactions incase expection
                        employeeTransferDBTrans.Rollback();
                        status = "Error";
                        statusCode = -3;
                        message = transactionPoint + " " + ex.Message;

                    }

                }
            }
            else
            {
                status = "Error";
                statusCode = -1;
                message = "EmployeeId is already taken in 'Transfer To Company'. Please enter new available EmployeeId";
            }



            return Json(new { status = status, statusCode = statusCode, message = message, userStatus = userStatus });
        }
        private bool EmployeeTransferUpdateContactInfo(UserInformation fromUser, UserInformation toUser)
        {
            var loginEmail = "";
            var fromEmployeeContactInfo = db.UserContactInformation.Where(w => w.UserInformationId == fromUser.Id).FirstOrDefault();
            var toEmployeeContactInfo = db.UserContactInformation.Where(w => w.UserInformationId == toUser.Id).FirstOrDefault();
            if (fromEmployeeContactInfo != null)
            {
                loginEmail = fromEmployeeContactInfo.LoginEmail;
                fromEmployeeContactInfo.LoginEmail = null;
                if (toEmployeeContactInfo == null)
                {
                    var tempContactInfoEntity = (UserContactInformation)db.Entry(fromEmployeeContactInfo).CurrentValues.ToObject();
                    tempContactInfoEntity.UserInformationId = toUser.Id;//set the to user id
                    tempContactInfoEntity.ModifiedBy = SessionHelper.LoginId;
                    tempContactInfoEntity.ModifiedDate = DateTime.Today;
                    
                    db.UserContactInformation.Add(tempContactInfoEntity);
                }
                else
                {
                    toEmployeeContactInfo.HomeNumber = fromEmployeeContactInfo.HomeNumber;
                    toEmployeeContactInfo.CelNumber = fromEmployeeContactInfo.CelNumber;
                    toEmployeeContactInfo.FaxNumber = fromEmployeeContactInfo.FaxNumber;
                    toEmployeeContactInfo.OtherNumber = fromEmployeeContactInfo.OtherNumber;
                    toEmployeeContactInfo.WorkNumber = fromEmployeeContactInfo.WorkNumber;
                    toEmployeeContactInfo.WorkExtension = fromEmployeeContactInfo.WorkExtension;

                    toEmployeeContactInfo.LoginEmail = null;                    
                    toEmployeeContactInfo.PersonalEmail = fromEmployeeContactInfo.PersonalEmail;
                    toEmployeeContactInfo.WorkEmail = fromEmployeeContactInfo.WorkEmail;
                    toEmployeeContactInfo.OtherEmail = fromEmployeeContactInfo.OtherEmail;

                    toEmployeeContactInfo.MailingAddress1 = fromEmployeeContactInfo.MailingAddress1;
                    toEmployeeContactInfo.MailingAddress2 = fromEmployeeContactInfo.MailingAddress2;
                    toEmployeeContactInfo.MailingCountryId = fromEmployeeContactInfo.MailingCountryId;
                    toEmployeeContactInfo.MailingStateId = fromEmployeeContactInfo.MailingStateId;
                    toEmployeeContactInfo.MailingCityId = fromEmployeeContactInfo.MailingCityId;
                    toEmployeeContactInfo.MailingZipCode = fromEmployeeContactInfo.MailingZipCode;

                    toEmployeeContactInfo.IsSameHomeAddress = fromEmployeeContactInfo.IsSameHomeAddress;
                    toEmployeeContactInfo.HomeAddress1 = fromEmployeeContactInfo.HomeAddress1;
                    toEmployeeContactInfo.HomeAddress2 = fromEmployeeContactInfo.HomeAddress2;
                    toEmployeeContactInfo.HomeCountryId = fromEmployeeContactInfo.HomeCountryId;
                    toEmployeeContactInfo.HomeStateId = fromEmployeeContactInfo.HomeStateId;
                    toEmployeeContactInfo.HomeCityId = fromEmployeeContactInfo.HomeCityId;
                    toEmployeeContactInfo.HomeZipCode = fromEmployeeContactInfo.HomeZipCode;
                    toEmployeeContactInfo.ModifiedBy = SessionHelper.LoginId;
                    toEmployeeContactInfo.ModifiedDate = DateTime.Today;
                }
                //Save Changes
                db.SaveChanges();
                //Employee Emergency contacts
                var fromEmergencyContactList = db.EmergencyContact.Where(w => w.UserInformationId == fromEmployeeContactInfo.UserInformationId && w.DataEntryStatus == 1);
                if (fromEmergencyContactList.Count() > 0)
                {
                    foreach (var fromEmergencyContact in fromEmergencyContactList)
                    {
                        //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                        var tempEmergencyEntity = (EmergencyContact)db.Entry(fromEmergencyContact).CurrentValues.ToObject();
                        tempEmergencyEntity.UserInformationId = toUser.Id;//set the to user id
                        tempEmergencyEntity.ModifiedBy = SessionHelper.LoginId;
                        tempEmergencyEntity.ModifiedDate = DateTime.Today;
                        db.EmergencyContact.Add(tempEmergencyEntity);
                    }
                }
                var toEmergencyContactList = db.EmergencyContact.Where(w => w.UserInformationId == toUser.Id && w.DataEntryStatus == 1);
                if (toEmergencyContactList.Count() > 0)
                {
                    foreach (var toEmergencyContact in toEmergencyContactList)
                    {
                        //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                        toEmergencyContact.DataEntryStatus = 0;
                        toEmergencyContact.ModifiedBy = SessionHelper.LoginId;
                        toEmergencyContact.ModifiedDate = DateTime.Today;
                    }
                }
                //Save Emergency Contacts
                db.SaveChanges();
                //Delete login user
                if (!string.IsNullOrEmpty(loginEmail))
                {
                    UserManagmentService.DeleteUser(loginEmail);
                }
            }
            return true;
        }
        private bool EmployeeTransferUpdateEducationInfo(UserInformation fromUser, UserInformation toUser)
        {
            var fromEmployeeEducationList = db.EmployeeEducation.Where(w => w.UserInformationId == fromUser.Id && w.DataEntryStatus == 1);
            if (fromEmployeeEducationList.Count() > 0)
            {
                foreach (var fromEmployeeEducation in fromEmployeeEducationList)
                {
                    //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                    var tempEmployeeEducationEntity = (EmployeeEducation)db.Entry(fromEmployeeEducation).CurrentValues.ToObject();
                    tempEmployeeEducationEntity.UserInformationId = toUser.Id;//set the to user id
                    tempEmployeeEducationEntity.ModifiedBy = SessionHelper.LoginId;
                    tempEmployeeEducationEntity.ModifiedDate = DateTime.Today;
                    db.EmployeeEducation.Add(tempEmployeeEducationEntity);
                }
            }
            var toEmployeeEducationList = db.EmployeeEducation.Where(w => w.UserInformationId == toUser.Id && w.DataEntryStatus == 1);
            if (toEmployeeEducationList.Count() > 0)
            {
                foreach (var toEmployeeEducation in toEmployeeEducationList)
                {

                    toEmployeeEducation.DataEntryStatus = 0;
                    toEmployeeEducation.ModifiedBy = SessionHelper.LoginId;
                    toEmployeeEducation.ModifiedDate = DateTime.Today;

                }
            }
            db.SaveChanges();
            return true;
        }
        private bool EmployeeTransferUpdateTrainingInfo(UserInformation fromUser, UserInformation toUser)
        {
            var fromEmployeeTrainingList = db.EmployeeTraining.Where(w => w.UserInformationId == fromUser.Id && w.DataEntryStatus == 1);
            if (fromEmployeeTrainingList.Count() > 0)
            {
                foreach (var fromEmployeeTraining in fromEmployeeTrainingList)
                {
                    //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                    var tempEmployeeTrainingEntity = (EmployeeTraining)db.Entry(fromEmployeeTraining).CurrentValues.ToObject();
                    tempEmployeeTrainingEntity.UserInformationId = toUser.Id;//set the to user id
                    tempEmployeeTrainingEntity.ModifiedBy = SessionHelper.LoginId;
                    tempEmployeeTrainingEntity.ModifiedDate = DateTime.Today;
                    db.EmployeeTraining.Add(tempEmployeeTrainingEntity);
                }
            }
            var toEmployeeTrainingList = db.EmployeeTraining.Where(w => w.UserInformationId == toUser.Id && w.DataEntryStatus == 1);
            if (toEmployeeTrainingList.Count() > 0)
            {
                foreach (var toEmployeeTraining in toEmployeeTrainingList)
                {

                    toEmployeeTraining.DataEntryStatus = 0;
                    toEmployeeTraining.ModifiedBy = SessionHelper.LoginId;
                    toEmployeeTraining.ModifiedDate = DateTime.Today;

                }
            }
            db.SaveChanges();
            return true;

        }
        private bool EmployeeTransferUpdateDependentsInfo(UserInformation fromUser, UserInformation toUser)
        {
            var fromEmployeeDependentList = db.EmployeeDependent.Where(w => w.UserInformationId == fromUser.Id && w.DataEntryStatus == 1);
            if (fromEmployeeDependentList.Count() > 0)
            {
                foreach (var fromEmployeeDependent in fromEmployeeDependentList)
                {
                    //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                    var tempEmployeeDependentEntity = (EmployeeDependent)db.Entry(fromEmployeeDependent).CurrentValues.ToObject();
                    tempEmployeeDependentEntity.UserInformationId = toUser.Id;//set the to user id
                    tempEmployeeDependentEntity.ModifiedBy = SessionHelper.LoginId;
                    tempEmployeeDependentEntity.ModifiedDate = DateTime.Today;
                    db.EmployeeDependent.Add(tempEmployeeDependentEntity);
                }
            }
            var toEmployeeDependentList = db.EmployeeDependent.Where(w => w.UserInformationId == toUser.Id && w.DataEntryStatus == 1);
            if (toEmployeeDependentList.Count() > 0)
            {
                foreach (var toEmployeeDependent in toEmployeeDependentList)
                {
                    toEmployeeDependent.DataEntryStatus = 0;
                    toEmployeeDependent.ModifiedBy = SessionHelper.LoginId;
                    toEmployeeDependent.ModifiedDate = DateTime.Today;
                }
            }
            db.SaveChanges();
            return true;
        }
        private bool EmployeeTransferUpdateDocumentInfo(UserInformation fromUser, UserInformation toUser)
        {
            var fromEmployeeDocumentList = db.EmployeeDocument.Where(w => w.UserInformationId == fromUser.Id && w.DataEntryStatus == 1);
            if (fromEmployeeDocumentList.Count() > 0)
            {
                foreach (var fromEmployeeDocument in fromEmployeeDocumentList)
                {
                    //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                    var tempEmployeeDocumentEntity = (EmployeeDocument)db.Entry(fromEmployeeDocument).CurrentValues.ToObject();
                    tempEmployeeDocumentEntity.UserInformationId = toUser.Id;//set the to user id
                    tempEmployeeDocumentEntity.ModifiedBy = SessionHelper.LoginId;
                    tempEmployeeDocumentEntity.ModifiedDate = DateTime.Today;
                    db.EmployeeDocument.Add(tempEmployeeDocumentEntity);
                }
            }
            var toEmployeeDocumentList = db.EmployeeDocument.Where(w => w.UserInformationId == toUser.Id && w.DataEntryStatus == 1);
            if (toEmployeeDocumentList.Count() > 0)
            {
                foreach (var toEmployeeDocument in toEmployeeDocumentList)
                {
                    toEmployeeDocument.DataEntryStatus = 0;
                    toEmployeeDocument.ModifiedBy = SessionHelper.LoginId;
                    toEmployeeDocument.ModifiedDate = DateTime.Today;
                }
            }
            db.SaveChanges();
            return true;
        }
        private bool EmployeeTransferUpdateCredentialInfo(UserInformation fromUser, UserInformation toUser)
        {
            var fromEmployeeCredentialList = db.EmployeeCredential.Where(w => w.UserInformationId == fromUser.Id && w.DataEntryStatus == 1);
            if (fromEmployeeCredentialList.Count() > 0)
            {
                foreach (var fromEmployeeCredential in fromEmployeeCredentialList)
                {
                    //db.Entry(employeeFromVeteranStatus).State = EntityState.Detached;
                    var tempEmployeeCredentialEntity = (EmployeeCredential)db.Entry(fromEmployeeCredential).CurrentValues.ToObject();
                    tempEmployeeCredentialEntity.UserInformationId = toUser.Id;//set the to user id
                    tempEmployeeCredentialEntity.ModifiedBy = SessionHelper.LoginId;
                    tempEmployeeCredentialEntity.ModifiedDate = DateTime.Today;
                    db.EmployeeCredential.Add(tempEmployeeCredentialEntity);
                }
            }
            var toEmployeeCredentialList = db.EmployeeCredential.Where(w => w.UserInformationId == toUser.Id && w.DataEntryStatus == 1);
            if (toEmployeeCredentialList.Count() > 0)
            {
                foreach (var toEmployeeCredential in toEmployeeCredentialList)
                {
                    toEmployeeCredential.DataEntryStatus = 0;
                    toEmployeeCredential.ModifiedBy = SessionHelper.LoginId;
                    toEmployeeCredential.ModifiedDate = DateTime.Today;
                }
            }
            db.SaveChanges();
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

        public ActionResult AddEmployment(int? id)
        {
            Employment emergencyContact = new Employment();
            ViewBag.Label = ViewBag.Label + " - Add";
            emergencyContact.UserInformationId = id;
            //ViewBag.EmploymentStatusId = new SelectList(db.EmploymentStatus, "Id", "EmploymentStatusName");
            //ViewBag.TerminationEligibilityId = new SelectList(db.TerminationEligibility, "Id", "TerminationEligibilityName");
            //ViewBag.TerminationReasonId = new SelectList(db.TerminationReason, "Id", "TerminationReasonName");
            //ViewBag.TerminationTypeId = new SelectList(db.TerminationType, "Id", "TerminationTypeName");
            //ViewBag.UserInformationId = new SelectList(db.UserInformation, "Id", "IdNumber");
            return PartialView(emergencyContact);
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
                Employment model = db.Employment.Where(u => u.Id == id && u.DataEntryStatus != 0).OrderByDescending(u => u.CreatedDate).FirstOrDefault();
                if (model == null)
                {
                    return PartialView();
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

        public ActionResult UploadDocument(int? id)
        {
            var model = db.Employment.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Education document is Successfully uploaded!";
            // var model = db.EmployeeEducation.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase docFile = Request.Files[0];
                    string employmentId = Request.Form["EmploymentId"];
                    var employmentEntity = db.Employment.Find(int.Parse(employmentId));
                    if (employmentEntity != null)
                    {
                        var fileName = Path.GetFileName(docFile.FileName);
                        var fileExt = Path.GetExtension(docFile.FileName);
                        string docName = "" + employmentEntity.UserInformationId + "_" + employmentId + "-" + fileName;


                        FilePathHelper filePathHelper = new FilePathHelper();
                        string serverFilePath = filePathHelper.GetPath("EmploymentContract", docName);
                        docFile.SaveAs(serverFilePath);
                        employmentEntity.DocumentPath = filePathHelper.RelativePath;
                        employmentEntity.DocumentName = docName;
                        employmentEntity.ModifiedDate = DateTime.Now;
                        db.SaveChanges();
                        //retResult = new { status = "Success", message = "CV is successfully Uploaded!" };
                        //status = "Success";
                        //message = "CV is successfully Uploaded!";
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid document record data!";
                    }

                }
                catch (Exception ex)
                {
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        [HttpPost]
        public ActionResult Close([Bind(Include = "Id,UserInformationId,CurrentRecord,OriginalHireDate,EffectiveHireDate,ProbationStartDate,ProbationEndDate,EmploymentStatusId,TerminationDate,TerminationTypeId,TerminationReasonId,DocumentName,DocumentExtension,DocumentFile,TerminationEligibilityId,TerminationEligibilityName,TerminationNotes")] Employment employment)
        {
            ModelState.Remove("OriginalHireDate");
            ModelState.Remove("EffectiveHireDate");
            ModelState.Remove("ProbationStartDate");
            ModelState.Remove("ProbationEndDate");
            ModelState.Remove("EmploymentStatusId");
            if (!employment.TerminationDate.HasValue)
                ModelState.AddModelError("TerminationDate", "The Termination Date field is required.");
            if (!employment.TerminationTypeId.HasValue || employment.TerminationTypeId == 0)
                ModelState.AddModelError("TerminationTypeId", "The Termination Type field is required.");
            if (ModelState.IsValid)
            {
                if (employment.OriginalHireDate > employment.TerminationDate.Value)
                    ModelState.AddModelError("TerminationDate", "The Termination Date can't be prior to Hire Date.");
                if (employment.EffectiveHireDate > employment.TerminationDate.Value)
                    ModelState.AddModelError("TerminationDate", "The Termination Date can't be prior to Re-Hire Date.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var model = db.Employment.FirstOrDefault(e => e.Id == employment.Id);
                    model.TerminationDate = employment.TerminationDate;
                    model.TerminationTypeId = employment.TerminationTypeId;
                    model.TerminationReasonId = employment.TerminationReasonId;
                    model.TerminationEligibilityId = employment.TerminationEligibilityId;
                    model.TerminationNotes = employment.TerminationNotes;
                    db.SaveChanges();
                    return RedirectToAction("IndexByUser", new { id = model.UserInformationId });
                }
                catch (Exception ex)
                {
                }

            }
            return GetErrors();
        }
        public override bool CheckBeforeDelete(int id)
        {
            var employment = db.Employment.Include(u => u.EmploymentHistory)
                         .Include(u=>u.PayInformationHistory)   
                         .FirstOrDefault(c => c.Id == id);
            if (employment.EmploymentHistory != null)
            {
                if (employment.EmploymentHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                    return false;
            }
            if (employment.PayInformationHistory != null)
            {
                if (employment.PayInformationHistory.Where(t => t.DataEntryStatus == 1).Count() > 0)
                    return false;
            }
            return true;
        }
    }
}
