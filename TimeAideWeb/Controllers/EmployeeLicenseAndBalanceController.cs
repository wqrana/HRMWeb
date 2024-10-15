using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Models.ViewModel;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class EmployeeLicenseAndBalanceController : TimeAideWebSecurotyBaseControllers
    {
        public EmployeeLicenseAndBalanceController() : base(typeof(EmployeeLicensesAndBalances).Name)
        {
        }
        // GET: EmployeeLicenseAndBalance
        public ActionResult IndexByUser(int id)
        {
            try
            {
                AllowView();
                return PartialView("Index");
            }
           catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "IndexByUser");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
            
        }
        public ActionResult AccrualRuleDetail(int id)
        {
            try
            {
                AllowView();
               var model= db.SP_GetEmployeeAccrualRules<EmployeeAccrualRuleViewModel>(id);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "AccrualRuleDetail");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult CreateAccrualRule()
        {
            try
            {
                AllowAdd();
                ViewBag.AccrualTypeId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName");
                ViewBag.AccrualRuleId = new SelectList(db.GetAllByCompany<AccrualRule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w=>w.Id==0), "Id", "AccrualRuleName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "CreateAccrualRule");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult CreateDefaultAccrualRules()
        {
            try
            {
                AllowAdd();
                ViewBag.AccrualTypeList = db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "CreateAccrualRule");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult EditAccrualRule(int id)
        {
            try
            {
                AllowEdit();
               var entity= db.EmployeeAccrualRule.Find(id);
                ViewBag.AccrualTypeId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName", entity.AccrualTypeId);
                ViewBag.AccrualRuleId = new SelectList(db.GetAllByCompany<AccrualRule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.AccrualTypeId == entity.AccrualTypeId), "Id", "AccrualRuleName", entity.AccrualRuleId);
                
                return PartialView(entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "EditAccrualRule");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult CreateEditAccrualRule(EmployeeAccrualRuleViewModel model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            EmployeeAccrualRule employeeAccrualRuleEntity = null;
            EmployeeAccrualBalance employeeAccrualBalanceEntity = null;
            try
            {
                var isAlreadyExist=db.EmployeeAccrualRule.Where(w => w.UserInformationId == model.UserInformationId
                                            && w.AccrualTypeId == model.AccrualTypeId && w.StartOfRuleDate == model.StartOfRuleDate
                                            && w.Id!= model.EmployeeAccrualRuleId && w.DataEntryStatus==1)
                                            .Count();
                if (isAlreadyExist == 0)
                {
                    if (model.EmployeeAccrualRuleId == 0)
                    {
                        employeeAccrualRuleEntity = new EmployeeAccrualRule();
                        employeeAccrualRuleEntity.UserInformationId = model.UserInformationId;
                        db.EmployeeAccrualRule.Add(employeeAccrualRuleEntity);
                        if (model.BalanceStartDate != null && model.AccruedHours != null)
                        {
                            employeeAccrualBalanceEntity = new EmployeeAccrualBalance();
                            employeeAccrualBalanceEntity.UserInformationId = model.UserInformationId;
                            employeeAccrualBalanceEntity.AccrualTypeId = model.AccrualTypeId;
                            employeeAccrualBalanceEntity.BalanceStartDate = model.BalanceStartDate.Value;
                            employeeAccrualBalanceEntity.AccruedHours = model.AccruedHours;
                            db.EmployeeAccrualBalance.Add(employeeAccrualBalanceEntity);
                        }
                    }
                    else
                    {
                        employeeAccrualRuleEntity = db.EmployeeAccrualRule.Find(model.EmployeeAccrualRuleId);
                        employeeAccrualRuleEntity.ModifiedBy = SessionHelper.LoginId;
                        employeeAccrualRuleEntity.ModifiedDate = DateTime.Now;
                    }
                    employeeAccrualRuleEntity.AccrualTypeId = model.AccrualTypeId;
                    employeeAccrualRuleEntity.AccrualRuleId = model.AccrualRuleId;
                    employeeAccrualRuleEntity.StartOfRuleDate = model.StartOfRuleDate;
                    employeeAccrualRuleEntity.AccrualDailyHours = model.AccrualDailyHours;
                    db.SaveChanges();
                }
                else
                {
                    status = "Error";
                    message = "Accrual type is already added for selected start of date.";

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
        [HttpPost]
        public JsonResult CreateDefaultAccrualRules(EmployeeLicensesAndBalances model)
        {
            string status = "Success";
            string message = "Successfully Added!";
            EmployeeAccrualRule employeeAccrualRuleEntity = null;
            EmployeeAccrualBalance employeeAccrualBalanceEntity = null;
            using (var createDefaultTrans = db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var employeeAccrualRule in model.EmployeeAccrualRules)
                    {
                        var isAlreadyExist = db.EmployeeAccrualRule.Where(w => w.UserInformationId == employeeAccrualRule.UserInformationId
                                                      && w.AccrualTypeId == employeeAccrualRule.AccrualTypeId && w.StartOfRuleDate == employeeAccrualRule.StartOfRuleDate
                                                      && w.Id != employeeAccrualRule.EmployeeAccrualRuleId && w.DataEntryStatus == 1)
                                                    .Count();
                        if (isAlreadyExist == 0)
                        {
                            if (employeeAccrualRule.EmployeeAccrualRuleId == 0)
                            {
                                employeeAccrualRuleEntity = new EmployeeAccrualRule();
                                employeeAccrualRuleEntity.UserInformationId = employeeAccrualRule.UserInformationId;
                                db.EmployeeAccrualRule.Add(employeeAccrualRuleEntity);
                                if (employeeAccrualRule.AccruedHours != null)
                                {
                                    employeeAccrualBalanceEntity = new EmployeeAccrualBalance();
                                    employeeAccrualBalanceEntity.UserInformationId = employeeAccrualRule.UserInformationId;
                                    employeeAccrualBalanceEntity.AccrualTypeId = employeeAccrualRule.AccrualTypeId;
                                    employeeAccrualBalanceEntity.BalanceStartDate = employeeAccrualRule.StartOfRuleDate;
                                    employeeAccrualBalanceEntity.AccruedHours = employeeAccrualRule.AccruedHours;
                                    db.EmployeeAccrualBalance.Add(employeeAccrualBalanceEntity);
                                }
                            }

                            employeeAccrualRuleEntity.AccrualTypeId = employeeAccrualRule.AccrualTypeId;
                            employeeAccrualRuleEntity.AccrualRuleId = employeeAccrualRule.AccrualRuleId;
                            employeeAccrualRuleEntity.StartOfRuleDate = employeeAccrualRule.StartOfRuleDate;
                            employeeAccrualRuleEntity.AccrualDailyHours = employeeAccrualRule.AccrualDailyHours;
                            db.SaveChanges();
                        }
                        else
                        {
                            var accTypeName = db.AccrualType.Find(employeeAccrualRule.AccrualTypeId).AccrualTypeName;
                           // status = "Error";
                            message = $"Accrual type {accTypeName} is already added for selected start of date.";
                            throw new Exception(message);
                        }
                    }
                    createDefaultTrans.Commit();
                }
                catch (Exception ex)
                {
                    createDefaultTrans.Rollback();
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }
        public JsonResult AjaxGetAccrualTypeRules(int id)
        {
            var accrualTypeRuleList = db.GetAllByCompany<AccrualRule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).Where(w => w.AccrualTypeId == id)
                                .Select(s => new { id = s.Id, name = s.AccrualRuleName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = accrualTypeRuleList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public JsonResult AjaxCheckForAccrualInitalBalance(int id, int accrualTypeId)
        {
            var isExist = db.EmployeeAccrualBalance.Where(w => w.UserInformationId == id
                                        && w.AccrualTypeId == accrualTypeId && w.DataEntryStatus == 1).Count();
            var isRequired = isExist == 0 ? true : false;
            JsonResult jsonResult = new JsonResult()
            {
                Data = isRequired,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public ActionResult AccrualRuleDelete(int id)
        {
            var model = db.EmployeeAccrualRule.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult ConfirmAccrualRuleDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var deleteEntity = db.EmployeeAccrualRule.Find(id);
            try
            {
                deleteEntity.ModifiedBy = SessionHelper.LoginId;
                deleteEntity.ModifiedDate = DateTime.Now;
                deleteEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
        public ActionResult EmployeeLicensesChangeHistory(int id, string tableName)
        {
            try
            {
                AllowChangeHistory();
                var FormName = tableName;
                var FormNameShort = tableName;
                if (FormName.Length > 20)
                {
                    FormNameShort = FormName.Substring(0, 20);
                }
                var entitySet = db.AuditLogDetail.Where(d => d.AuditLog.ReferenceId == id && d.AuditLog.TableName.Contains(FormNameShort) && d.ClientId == SessionHelper.SelectedClientId).ToList();
                
                var refrenceObject = db.Database.SqlQuery<BaseEntity>("Select * from " + FormName + " where " + FormName + "Id = " + id.ToString() + "").FirstOrDefault();
                if (refrenceObject != null)
                {
                    ViewBag.ReferenceObject = refrenceObject;
                    ViewBag.TableName = FormName;
                    ViewBag.RefrenceId = id;
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
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "EmployeeLicensesAndAccrual", "EmployeeAccrualRuleChangeHistory");
                return View("~/Views/Shared/Unauthorized.cshtml", handleErrorInfo);
            }
        }
        public ActionResult AccrualBalanceDetail(int id)
        {
            try
            {
                AllowView();
                var model = db.SP_GetEmployeeAccrualBalances<EmployeeAccrualBalanceViewModel>(id);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "AccrualBalanceDetail");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult CreateAccrualBalance()
        {
            try
            {
                AllowAdd();
                ViewBag.AccrualTypeId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName");
               
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "CreateAccrualBalance");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult EditAccrualBalance(int id)
        {
            try
            {
                AllowEdit();
                var entity = db.EmployeeAccrualBalance.Find(id);
                ViewBag.AccrualTypeId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName", entity.AccrualTypeId);
                

                return PartialView(entity);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(EmployeeLicensesAndBalances).Name, "EditAccrualBalance");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        [HttpPost]
        public JsonResult CreateEditAccrualBalance(EmployeeAccrualBalanceViewModel model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";           
            EmployeeAccrualBalance employeeAccrualBalanceEntity = null;
            try
            {
                var isAlreadyExist = db.EmployeeAccrualBalance.Where(w => w.UserInformationId == model.UserInformationId
                                              && w.AccrualTypeId == model.AccrualTypeId && w.BalanceStartDate == model.BalanceStartDate
                                              && w.Id != model.EmployeeAccrualBalanceId && w.DataEntryStatus==1)
                                            .Count();
                if (isAlreadyExist == 0)
                {
                    if (model.EmployeeAccrualBalanceId == 0)
                    {
                        employeeAccrualBalanceEntity = new EmployeeAccrualBalance();
                        employeeAccrualBalanceEntity.UserInformationId = model.UserInformationId;
                        db.EmployeeAccrualBalance.Add(employeeAccrualBalanceEntity);
                       
                    }
                    else
                    {
                        employeeAccrualBalanceEntity = db.EmployeeAccrualBalance.Find(model.EmployeeAccrualBalanceId);
                        employeeAccrualBalanceEntity.ModifiedBy = SessionHelper.LoginId;
                        employeeAccrualBalanceEntity.ModifiedDate = DateTime.Now;
                    }
                    employeeAccrualBalanceEntity.AccrualTypeId = model.AccrualTypeId;
                    employeeAccrualBalanceEntity.BalanceStartDate = model.BalanceStartDate;
                    employeeAccrualBalanceEntity.AccruedHours = model.AccruedHours;
                    db.SaveChanges();
                }
                else
                {
                    status = "Error";
                    message = "Accrual Balance is already added for selected start of date.";

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
        public ActionResult AccrualBalanceDelete(int id)
        {
            var model = db.EmployeeAccrualBalance.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult ConfirmAccrualBalanceDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var deleteEntity = db.EmployeeAccrualBalance.Find(id);
            try
            {
                deleteEntity.ModifiedBy = SessionHelper.LoginId;
                deleteEntity.ModifiedDate = DateTime.Now;
                deleteEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }
    }
}