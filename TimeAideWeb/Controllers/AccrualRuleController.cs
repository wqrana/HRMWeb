using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Data;
using TimeAide.Web.Models;

namespace TimeAide.Web.Controllers
{
    public class AccrualRuleController : TimeAideWebControllers<AccrualRule>
    {
        public override ActionResult Index()
        {
            try
            {               
                AllowView();               
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(Position).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult  IndexByAccrualRule()
        {
            try
            {
                AllowView();         
                
                var entitySet = db.GetAllByCompany<AccrualRule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                var model = entitySet.OrderByDescending(e => e.CreatedDate).ToList();
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(Position).Name, "IndexByCompany");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
        public ActionResult AccrualRuleTierList(int? id)
        {
            try
            {
                AllowView();

                var entitySet = db.GetAllByCompany<AccrualRuleTier>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                                                    .Where(w => w.AccrualRuleId == id).OrderBy(o => o.TierNo).ToList();
               
                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(Position).Name, "AccrualRuleTierList");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
        public ActionResult AccrualRuleTierCreate(int id)
        {
            try
            {
                AllowAdd();
                ViewBag.AccrualTypeExcessId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName");
                //ViewBag.DefaultEEOCategoryId = new SelectList(db.GetAll<EEOCategory>(SessionHelper.SelectedClientId), "Id", "EEOCategoryName");
                var accrualRuleEntity = db.AccrualRule.Find(id);
                var maxTierNo = db.AccrualRuleTier.Where(w => w.AccrualRuleId == id && w.DataEntryStatus == 1).Count();

                var model = new AccrualRuleTier()
                {
                    TierNo = maxTierNo+1,
                    AccrualRuleId = id,
                    AccrualRule = accrualRuleEntity
                };

                return PartialView("AccrualRuleTierCreateEdit",model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRule", "Create");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public ActionResult AccrualRuleTierEdit(int id)
        {
            try
            {
                AllowEdit();
                var model = db.AccrualRuleTier.Find(id);

                ViewBag.AccrualTypeExcessId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName",model.AccrualTypeExcess);
                //ViewBag.DefaultEEOCategoryId = new SelectList(db.GetAll<EEOCategory>(SessionHelper.SelectedClientId), "Id", "EEOCategoryName");
             
                return PartialView("AccrualRuleTierCreateEdit", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRuleTier", "Edit");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }


        public ActionResult AccrualRuleWorkedHoursTierList(int? id)
        {
            try
            {
                AllowView();

                var entitySet = db.GetAllByCompany<AccrualRuleWorkedHoursTier>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                                                    .Where(w => w.AccrualRuleTierId == id).OrderBy(o => o.TierNo).ToList();

                return PartialView(entitySet);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(Position).Name, "AccrualRuleWorkedHoursTierList");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
        public override ActionResult Details(int? id)
        {
            try
            {
                AccrualRule model = null;
                AllowView();
                if (id == 0)
                    model = db.GetAllByCompany<AccrualRule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderByDescending(o => o.Id).FirstOrDefault();
                else
                    model = db.Find<AccrualRule>(id.Value, SessionHelper.SelectedClientId);

                if (model == null)
                    model = new AccrualRule();

                //return View(city);
                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRule", "Details");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public override ActionResult Create()
        {
            try
            {
                AllowAdd();
                ViewBag.AccrualTypeId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName");
                //ViewBag.DefaultEEOCategoryId = new SelectList(db.GetAll<EEOCategory>(SessionHelper.SelectedClientId), "Id", "EEOCategoryName");
                return PartialView();
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRule", "Create");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }
        public override ActionResult Edit(int? id)
        {

            try
            {
                AllowEdit();
                var model = db.AccrualRule.Find(id??0);
                ViewBag.AccrualTypeId = new SelectList(db.GetAllByCompany<AccrualType>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId), "Id", "AccrualTypeName",model.AccrualTypeId);

                ViewBag.CanBeAssignedToCurrentCompany = false;
                if (!model.CompanyId.HasValue)
                {
                    model.IsAllCompanies = true;
                }
                var companies = (model as BaseCompanyObjects).GetRefferredCompanies();
                if (companies.Count == 0 || (companies.Count == 1 && companies.FirstOrDefault() == SessionHelper.SelectedCompanyId))
                {
                    ViewBag.CanBeAssignedToCurrentCompany = true;
                }
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRule", "Create");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }             

           
        }
        [HttpPost]
        public JsonResult CreateEdit(AccrualRule model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            int id = 0;
            bool useYearsWorked = false;
            AccrualRule accrualEntity = null;
            try
            {
                var isAlreadyExist = db.GetAllByCompany<AccrualRule>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                        .Where(w => (w.Id != model.Id) && (w.AccrualRuleName.ToLower() == model.AccrualRuleName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Accrual-Rule Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        accrualEntity = new AccrualRule();
                        db.AccrualRule.Add(accrualEntity);
                    }
                    else
                    {
                        accrualEntity = db.AccrualRule.Find(model.Id);
                        accrualEntity.ModifiedBy = SessionHelper.LoginId;
                        accrualEntity.ModifiedDate = DateTime.Now;
                    }
                    useYearsWorked = accrualEntity.UseYearsWorked;
                    accrualEntity.AccrualRuleName = model.AccrualRuleName;
                    accrualEntity.AccrualRuleDescription = model.AccrualRuleDescription;
                    accrualEntity.ReferenceTypeId = model.ReferenceTypeId;
                    accrualEntity.BeginningOfYearDate = model.BeginningOfYearDate;
                    accrualEntity.AccrualPeriodTypeId = model.AccrualPeriodTypeId;
                    accrualEntity.AccrualTypeId = model.AccrualTypeId;
                    accrualEntity.AccrualTypeUnavailable = model.AccrualTypeUnavailable;
                    accrualEntity.AccumulationTypeId = model.AccumulationTypeId;
                    accrualEntity.AccumulationMultiplier = model.AccumulationMultiplier;
                    accrualEntity.UseYearsWorked = model.UseYearsWorked;
                    accrualEntity.CompanyId = model.IsAllCompanies ? null : (int?)SessionHelper.SelectedCompanyId;
                    using (var accrualRuleDBTrans = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.SaveChanges();
                            id = accrualEntity.Id;
                            if (id > 0)
                            {
                                if (model.Id == 0)
                                {
                                    var accrualRuleTierDefault = new AccrualRuleTier()
                                    {
                                        TierNo = 1,
                                        AccrualRuleId = id,
                                        TierDescription = "Default Tier",
                                        YearsWorkedFrom = 0,
                                        YearsWorkedTo = 100,
                                        CompanyId = accrualEntity.CompanyId
                                    };
                                    db.AccrualRuleTier.Add(accrualRuleTierDefault);
                                }
                                else if(useYearsWorked!= model.UseYearsWorked)
                                {
                                    var workHoursTierList = db.AccrualRuleWorkedHoursTier.Where(w => w.AccrualRuleId == id && w.DataEntryStatus == 1);
                                    var accrualRuleTierList = db.AccrualRuleTier.Where(w => w.AccrualRuleId == id && w.DataEntryStatus == 1);
                                    foreach(var workHoursTier in workHoursTierList)
                                    {
                                        workHoursTier.DataEntryStatus = 0;
                                        workHoursTier.ModifiedBy = SessionHelper.LoginId;
                                        workHoursTier.ModifiedDate = DateTime.Now;
                                    }
                                    db.SaveChanges();
                                    foreach (var accrualRuleTier in accrualRuleTierList)
                                    {
                                        accrualRuleTier.DataEntryStatus = 0;
                                        accrualRuleTier.ModifiedBy = SessionHelper.LoginId;
                                        accrualRuleTier.ModifiedDate = DateTime.Now;
                                    }
                                    db.SaveChanges();
                                    var accrualRuleTierDefault = new AccrualRuleTier()
                                    {
                                        TierNo = 1,
                                        AccrualRuleId = id,
                                        TierDescription = "Default Tier",
                                        YearsWorkedFrom = 0,
                                        YearsWorkedTo = 100,
                                        CompanyId = accrualEntity.CompanyId
                                    };
                                    db.AccrualRuleTier.Add(accrualRuleTierDefault);
                                }
                                db.SaveChanges();
                            }
                            else
                            {
                                throw new Exception("Error Insterting record");
                            }

                            accrualRuleDBTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            accrualRuleDBTrans.Rollback();
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                            status = "Error";
                            message = ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new {id=id, status = status, message = message });
        }
        [HttpPost]
        public JsonResult CreateEditTier(AccrualRuleTier model)
        {
            int id = 0;
            string status = "Success";
            string message = "Successfully Added/Updated!";
                       
            AccrualRuleTier accrualRuleTierEntity = null;
            try
            {
                var isAlreadyExist = db.GetAllByCompany<AccrualRuleTier>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                        .Where(w => (w.Id != model.Id)&&(w.AccrualRuleId==model.AccrualRuleId) && (w.TierDescription.ToLower() == model.TierDescription.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    throw new Exception("Accrual-Rule Tier is already Exists");
                }
                isAlreadyExist = db.GetAllByCompany<AccrualRuleTier>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                        .Where(w => (w.Id != model.Id) && (w.AccrualRuleId == model.AccrualRuleId) 
                                                    &&( (model.YearsWorkedFrom>=w.YearsWorkedFrom && model.YearsWorkedFrom<=w.YearsWorkedTo) 
                                                    || (model.YearsWorkedTo >= w.YearsWorkedFrom && model.YearsWorkedTo <= w.YearsWorkedTo))
                                                    ).Count();

                if (isAlreadyExist > 0)
                {
                    throw new Exception("Years range is being overlapped with existing Tier.");
                }
                if (model.Id == 0)
                    {
                        accrualRuleTierEntity = new AccrualRuleTier();
                        accrualRuleTierEntity.AccrualRuleId = model.AccrualRuleId;
                        db.AccrualRuleTier.Add(accrualRuleTierEntity);
                    }
                    else
                    {
                        accrualRuleTierEntity = db.AccrualRuleTier.Find(model.Id);
                        accrualRuleTierEntity.ModifiedBy = SessionHelper.LoginId;
                        accrualRuleTierEntity.ModifiedDate = DateTime.Now;
                    }
                accrualRuleTierEntity.TierNo = model.TierNo;
                accrualRuleTierEntity.TierDescription = model.TierDescription;
                accrualRuleTierEntity.YearsWorkedFrom = model.YearsWorkedFrom;
                accrualRuleTierEntity.YearsWorkedTo = model.YearsWorkedTo;
                accrualRuleTierEntity.WaitingPeriodType = model.WaitingPeriodType;
                accrualRuleTierEntity.WaitingPeriodLength = model.WaitingPeriodLength;
                accrualRuleTierEntity.AllowedMaxHoursTypeId = model.AllowedMaxHoursTypeId;
                accrualRuleTierEntity.AllowedMaxHours = model.AllowedMaxHours;
                accrualRuleTierEntity.AccrualTypeExcess = model.AccrualTypeExcess;
                accrualRuleTierEntity.ResetAccruedHoursTypeId = model.ResetAccruedHoursTypeId;
                accrualRuleTierEntity.ResetHours = model.ResetHours;
                accrualRuleTierEntity.ResetDate = model.ResetDate;
                accrualRuleTierEntity.MinWorkedHoursType = model.MinWorkedHoursType;
                accrualRuleTierEntity.AccrualHours = model.AccrualHours;
                    //accrualRuleTierEntity.CompanyId = null;                    

                using (var accrualRuleTierDBTrans = db.Database.BeginTransaction())
                    {
                    try
                    {
                        db.SaveChanges();
                        id = accrualRuleTierEntity.Id;
                        if (id > 0)
                        {
                            var accrualRuleWorkHrTiers = db.AccrualRuleWorkedHoursTier.Where(w => w.AccrualRuleTierId == id && w.DataEntryStatus == 1);
                            if (!model.MinWorkedHoursType && accrualRuleWorkHrTiers.Count() > 0)
                            {
                                foreach (var accrualRuleWorkHrTier in accrualRuleWorkHrTiers)
                                {
                                    accrualRuleWorkHrTier.DataEntryStatus = 0;
                                    accrualRuleWorkHrTier.ModifiedBy = SessionHelper.LoginId;
                                    accrualRuleWorkHrTier.ModifiedDate = DateTime.Now;
                                    
                                }
                                db.SaveChanges();
                            }
                            else if (model.MinWorkedHoursType == true)
                            {
                                var existingRecIds = accrualRuleWorkHrTiers.Select(s => s.Id).ToArray();
                                List<int> deleteRecIds = new List<int>();
                                foreach(var existingId in existingRecIds)
                                {
                                    var checkId = 0;
                                    if(model.AccrualRuleWorkedHoursTiers!=null)
                                        checkId=  model.AccrualRuleWorkedHoursTiers.Where(w => w.Id == existingId).Select(w=>w.Id).FirstOrDefault();

                                    if (checkId == 0) deleteRecIds.Add(existingId);
                                }
                                  
                                foreach (var accrualRuleWorkHrTier in model.AccrualRuleWorkedHoursTiers)
                                {
                                    accrualRuleWorkHrTier.AccrualRuleId = model.AccrualRuleId;
                                    accrualRuleWorkHrTier.AccrualRuleTierId = id;
                                    if (accrualRuleWorkHrTier.Id == 0) //add new tier
                                    {
                                        db.AccrualRuleWorkedHoursTier.Add(accrualRuleWorkHrTier);
                                    }
                                    else
                                    {
                                        var existingTier = accrualRuleWorkHrTiers.Where(w => w.Id == accrualRuleWorkHrTier.Id).FirstOrDefault();
                                        if (existingTier != null)
                                        {
                                            existingTier.TierNo = accrualRuleWorkHrTier.TierNo;
                                            existingTier.TierDescription = accrualRuleWorkHrTier.TierDescription;
                                            existingTier.TierWorkedHoursMin = accrualRuleWorkHrTier.TierWorkedHoursMin;
                                            existingTier.TierWorkedHoursMax = accrualRuleWorkHrTier.TierWorkedHoursMax;
                                            existingTier.AccrualHours = existingTier.AccrualHours;
                                            existingTier.ModifiedBy = SessionHelper.LoginId;
                                            existingTier.ModifiedDate = DateTime.Now;
                                        }
                                    }
                                    
                                }
                                foreach (var delTierId in deleteRecIds)
                                {
                                    var deleteTier = accrualRuleWorkHrTiers.Where(w => w.Id == delTierId).FirstOrDefault();
                                    if (deleteTier != null)
                                    {
                                        deleteTier.DataEntryStatus = 0;
                                        deleteTier.ModifiedBy = SessionHelper.LoginId;
                                        deleteTier.ModifiedDate = DateTime.Now;
                                        //db.SaveChanges();
                                    }
                                }

                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            throw new Exception("Error Insterting Tier record");
                        }

                            accrualRuleTierDBTrans.Commit();
                        }
                        catch (Exception ex)
                        {
                            accrualRuleTierDBTrans.Rollback();
                            Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                            status = "Error";
                            message = ex.Message;
                        }
                    }
               
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                status = "Error";
                message = ex.Message;
            }

            return Json(new { accrualRuleId = model.AccrualRuleId, status = status, message = message });
        }

        public ActionResult TierDetail(int id)
        {
            AllowView();
            var model = db.AccrualRuleTier.Find(id);
            int accrualTypeId;
            int.TryParse(model.AccrualTypeExcess, out accrualTypeId);
            ViewBag.AccrualTypeExcessCode = "";
            var accrualTypeEntity = db.AccrualType.Find(accrualTypeId);
            if (accrualTypeEntity != null)
            {
                ViewBag.AccrualTypeExcessCode = accrualTypeEntity.AccrualTypeName;
            }
            return PartialView("AccrualRuleTierDetail", model);
        }
       public ActionResult DeleteTier(int id)
        {
            try
            {
                AllowDelete();
                var model = db.AccrualRuleTier.Find(id);
              
                return PartialView("AccrualRuleTierDelete", model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "AccrualRule", "DeleteTier");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        [HttpPost]
        public ActionResult ConfirmDeleteTier(int id)
        {
            {
                string status = "Success";
                string message = "Successfully Deleted!";
                var accrualRuleTierEntity = db.AccrualRuleTier.Find(id);
                using (var accrualRuleTierDBTrans = db.Database.BeginTransaction())
                {
                    try
                    {
                       var tierCount= db.AccrualRuleTier.Where(w => w.AccrualRuleId == accrualRuleTierEntity.AccrualRuleId 
                                                        && w.DataEntryStatus == 1).Count();
                        if (tierCount == 1)
                        {
                            throw new Exception("At Least one tier is needed. Tier can't be deleted!");
                        }
                        accrualRuleTierEntity.ModifiedBy = SessionHelper.LoginId;
                        accrualRuleTierEntity.ModifiedDate = DateTime.Now;
                        accrualRuleTierEntity.DataEntryStatus = 0;

                        var accrualRuleWorkHrTiers = db.AccrualRuleWorkedHoursTier.Where(w => w.AccrualRuleTierId == id && w.DataEntryStatus == 1);
                        if (accrualRuleWorkHrTiers.Count() > 0)
                        {
                            foreach (var accrualRuleWorkHrTier in accrualRuleWorkHrTiers)
                            {
                                accrualRuleWorkHrTier.DataEntryStatus = 0;
                                accrualRuleWorkHrTier.ModifiedBy = SessionHelper.LoginId;
                                accrualRuleWorkHrTier.ModifiedDate = DateTime.Now;

                            }
                            
                        }
                        db.SaveChanges();
                        accrualRuleTierDBTrans.Commit();
                    }

                    catch (Exception ex)
                    {
                        accrualRuleTierDBTrans.Rollback();
                        Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                        status = "Error";
                        message = ex.Message;
                    }
                }
                return Json(new { status = status, message = message,accrualRuleId= accrualRuleTierEntity.AccrualRuleId });
            }
        }
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var accrualRuleEntity = db.AccrualRule.Find(id);
            using (var accrualRuleDBTrans = db.Database.BeginTransaction())
            {
                try
                {

                    accrualRuleEntity.ModifiedBy = SessionHelper.LoginId;
                    accrualRuleEntity.ModifiedDate = DateTime.Now;
                    accrualRuleEntity.DataEntryStatus = 0;
                    var accrualRuleTiers = db.AccrualRuleTier.Where(w => w.AccrualRuleId == id && w.DataEntryStatus == 1);
                    if (accrualRuleTiers.Count() > 0)
                    {
                        foreach (var accrualRuleTier in accrualRuleTiers)
                        {
                            accrualRuleTier.DataEntryStatus = 0;
                            accrualRuleTier.ModifiedBy = SessionHelper.LoginId;
                            accrualRuleTier.ModifiedDate = DateTime.Now;

                            var accrualRuleWorkHrTiers = db.AccrualRuleWorkedHoursTier.Where(w => w.AccrualRuleTierId == accrualRuleTier.Id && w.DataEntryStatus == 1);
                            if (accrualRuleWorkHrTiers.Count() > 0)
                            {
                                foreach (var accrualRuleWorkHrTier in accrualRuleWorkHrTiers)
                                {
                                    accrualRuleWorkHrTier.DataEntryStatus = 0;
                                    accrualRuleWorkHrTier.ModifiedBy = SessionHelper.LoginId;
                                    accrualRuleWorkHrTier.ModifiedDate = DateTime.Now;
                                }
                            }
                        }

                    }           
                                        
                    db.SaveChanges();
                    accrualRuleDBTrans.Commit();
                }
                catch (Exception ex)
                {
                    accrualRuleDBTrans.Rollback();
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    status = "Error";
                    message = ex.Message;
                }

                return Json(new { status = status, message = message });
            }
        }

        }
}

