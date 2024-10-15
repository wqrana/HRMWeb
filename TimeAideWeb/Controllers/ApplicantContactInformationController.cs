using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using TimeAide.Models.ViewModel;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;

namespace TimeAide.Web.Controllers
{
    public class ApplicantContactInformationController : BaseApplicantRoleRightsController<ApplicantInformation>
    {
             // GET: ApplicantContactInformation
        public ActionResult Details(int? id)
        {
            
            var model = db.ApplicantContactInformation.Find(id ?? 0);
           
            return PartialView(model);
        }

        public ActionResult CreateEditAddressInfo(int? id)
        {
            try
            {
                AllowAdd();
                ApplicantContactInformation model = null;
                if (id == null)
                {
                    model = new ApplicantContactInformation();
                }
                else
                {
                    model = db.ApplicantContactInformation.Find(id ?? 0);
                }

                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInformation", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
              
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AjaxCreateEditAddressInfo(ApplicantContactInformation model)
        {
            //dynamic retResult = new {id=0, status = "", message = "" };
            dynamic retResult = null;
            ApplicantContactInformation appEntity = null;

            try
            {
               
                if (model.Id == 0)
                {
                    appEntity = new ApplicantContactInformation();
                    db.ApplicantContactInformation.Add(appEntity);
                    appEntity.ApplicantInformationId = model.ApplicantInformationId;
                }
                else
                {
                    appEntity = db.ApplicantContactInformation.Find(model.Id);
                    appEntity.ModifiedBy = SessionHelper.LoginId;
                    appEntity.ModifiedDate = DateTime.Now;
                }


                appEntity.HomeAddress1 = model.HomeAddress1;
                appEntity.HomeAddress2 = model.HomeAddress2;
                appEntity.HomeCountryId = model.HomeCountryId;
                appEntity.HomeStateId = model.HomeStateId;
                appEntity.HomeCityId = model.HomeCityId;
                appEntity.HomeZipCode = model.HomeZipCode;

                appEntity.MailingAddress1 = model.MailingAddress1;
                appEntity.MailingAddress2 = model.MailingAddress2;
                appEntity.MailingCountryId = model.MailingCountryId;
                appEntity.MailingStateId = model.MailingStateId;
                appEntity.MailingCityId = model.MailingCityId;
                appEntity.MailingZipCode = model.MailingZipCode;
                
                db.SaveChanges();


                retResult = new { id = appEntity.Id, status = "Success", message = "Address(es) is/are added/Updated successfully!" };


            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //retResult.status = "Error";
                //retResult.message = ex.Message;
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }
            catch (Exception ex)
            {
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }

            return Json(retResult);
        }
        public ActionResult CreateEditContactInfo(int? id)
        {
            try
            {
                AllowAdd();
                var model = db.ApplicantContactInformation.Find(id ?? 0);
                return PartialView(model);
            }
            catch (AuthorizationException ex)
            {
                //if (Form.IsTab)
                //{
                //    return PartialView();
                //}
                Exception exception = new Exception(ex.ErrorMessage);
                HandleErrorInfo handleErrorInfo = new HandleErrorInfo(exception, "ApplicantInformation", "Edit");
                return PartialView("~/Views/ApplicantInformation/_ApplicantError.cshtml", handleErrorInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AjaxCreateEditContactInfo(ApplicantContactInformation model)
        {
            //dynamic retResult = new {id=0, status = "", message = "" };
            dynamic retResult = null;
            ApplicantContactInformation appEntity = null;

            try
            {

                if (model.Id == 0)
                {
                    appEntity = new ApplicantContactInformation();
                    db.ApplicantContactInformation.Add(appEntity);
                    appEntity.ApplicantInformationId = model.ApplicantInformationId;
                }
                else
                {
                    appEntity = db.ApplicantContactInformation.Find(model.Id);
                    appEntity.ModifiedBy = SessionHelper.LoginId;
                    appEntity.ModifiedDate = DateTime.Now;
                }

                appEntity.HomeNumber = model.HomeNumber;
                appEntity.CelNumber = model.CelNumber;
                appEntity.FaxNumber = model.FaxNumber;
                appEntity.OtherNumber = model.OtherNumber;

                appEntity.PersonalEmail = model.PersonalEmail;
                appEntity.WorkEmail = model.WorkEmail;
                appEntity.OtherEmail = model.OtherEmail;               

                db.SaveChanges();

                retResult = new { id = appEntity.Id, status = "Success", message = "Contact info. is added/Updated successfully!" };
                
            }
            catch (DbEntityValidationException ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //retResult.status = "Error";
                //retResult.message = ex.Message;
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                retResult = new { id = 0, status = "Error", message = ex.Message };
            }

            return Json(retResult);
        }
    }
}