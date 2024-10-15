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
    public class PositionController : TimeAideWebControllers<Position>
    {

        public ActionResult IndexByPosition()
        {
            try
            {
                AllowView();
                var entitySet = db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId);
                return PartialView(entitySet.OrderByDescending(e => e.CreatedDate).ToList());
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, typeof(Position).Name, "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }

        }
        public override ActionResult Details(int? id)
        {
            try
            {
                Position model = null;
                AllowView();
                if (id == 0)
                    model = db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId).OrderByDescending(o => o.Id).FirstOrDefault();
                else
                    model = db.Find<Position>(id.Value, SessionHelper.SelectedClientId);

                if (model == null)
                    model = new Position();

                //return View(city);
                ViewBag.Label = ViewBag.Label + " - Detail";
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "Position", "Index");
                return View("~/Views/Shared/Error.cshtml", handleErrorInfo);
            }
        }

        public override ActionResult Create()
        {
           
            ViewBag.DefaultPayScaleId = new SelectList(db.GetAll<PayScale>(SessionHelper.SelectedClientId), "Id", "PayScaleName");
            ViewBag.DefaultEEOCategoryId = new SelectList(db.GetAll<EEOCategory>(SessionHelper.SelectedClientId), "Id", "EEOCategoryName");
            return PartialView();
        }
        public override ActionResult Edit(int? id)
        {
           
            var model = db.Position.Where(w => w.Id == id).FirstOrDefault();
            ViewBag.DefaultPayScaleId = new SelectList(db.GetAll<PayScale>(SessionHelper.SelectedClientId), "Id", "PayScaleName",model.DefaultPayScaleId);
            ViewBag.DefaultEEOCategoryId = new SelectList(db.GetAll<EEOCategory>(SessionHelper.SelectedClientId), "Id", "EEOCategoryName",model.DefaultEEOCategoryId);
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
        [HttpPost]
        public JsonResult CreateEdit(Position model)
        {
            string status = "Success";
            string message = "Successfully Added/Updated!";
            Position positionEntity = null;
            try
            {
                var isAlreadyExist = db.GetAllByCompany<Position>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                        .Where(w=> (w.Id != model.Id) && (w.PositionName.ToLower() == model.PositionName.ToLower()))
                                        .Count();
                if (isAlreadyExist > 0)
                {
                    status = "Error";
                    message = "Position Name is already Exists";
                }
                else
                {
                    if (model.Id == 0)
                    {
                        positionEntity = new Position();
                        db.Position.Add(positionEntity);
                    }
                    else
                    {
                        positionEntity = db.Position.Find(model.Id);
                        positionEntity.ModifiedBy = SessionHelper.LoginId;
                        positionEntity.ModifiedDate = DateTime.Now;
                    }
                    positionEntity.PositionName = model.PositionName;
                    positionEntity.PositionDescription = model.PositionDescription;
                    positionEntity.PositionCode = model.PositionCode;
                    positionEntity.DefaultPayScaleId = model.DefaultPayScaleId;
                    positionEntity.DefaultEEOCategoryId = model.DefaultEEOCategoryId;
                   // if (model.IsAllCompanies) positionEntity.CompanyId = null;
                    positionEntity.CompanyId = model.IsAllCompanies ? null :(int?)SessionHelper.SelectedCompanyId;
                    db.SaveChanges();
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
        public ActionResult CreateAdhocMasterData(Position model)
        {
            string status = "Success";
            string message = "";

            if (ModelState.IsValid)
            {
                try
                {
                    db.Position.Add(model);
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    status = "Error";
                    message = ex.Message;
                }

            }
            else
            {
                status = "Error";
                message = "Missing Required field(s)";

            }

            return Json(new { status = status, message = message, id = model.Id, text = model.PositionName });
        }
        [HttpPost]
        public JsonResult ConfirmDelete(int id)
        {
            string status = "Success";
            string message = "Successfully Deleted!";
            var positionEntity = db.Position.Find(id);
            try
            {
                positionEntity.ModifiedBy = SessionHelper.LoginId;
                positionEntity.ModifiedDate = DateTime.Now;
                positionEntity.DataEntryStatus = 0;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                status = "Error";
                message = ex.Message;
            }
            return Json(new { status = status, message = message });
        }

    }
}
