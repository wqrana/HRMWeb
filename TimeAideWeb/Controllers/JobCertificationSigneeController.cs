
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TimeAide.Common.Helpers;
using TimeAide.Services.Helpers;
using TimeAide.Web.Models;
using TimeAide.Web.ViewModel;

namespace TimeAide.Web.Controllers
{
    public class JobCertificationSigneeController : TimeAideWebControllers<JobCertificationSignee>
    {

        [HttpPost]
        public JsonResult AjaxCreateOrEdit()
        {
            string status = "Success";
            string message = "Signature is Successfully uploaded!";
            JobCertificationSignee JobCertificationSigneeEntity = null;
            int JobCertificationSigneeId = int.Parse(Request.Form["JobCertificationSigneeId"]);
            string Name = Request.Form["Name"];
            string Position = Request.Form["Position"];
            bool IsAllCompanies = bool.Parse(Request.Form["IsAllCompanies"]);
            if (JobCertificationSigneeId == 0)
            {
                JobCertificationSigneeEntity = new JobCertificationSignee();
                db.JobCertificationSignee.Add(JobCertificationSigneeEntity);
            }
            else
            {
                JobCertificationSigneeEntity = db.JobCertificationSignee.Find(JobCertificationSigneeId);
            }
            try
            {
                if (Request.Files.Count > 0)
                {               
                    HttpPostedFileBase logoFile = Request.Files[0];                   
                     byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(logoFile.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(logoFile.ContentLength);
                    }                   
                     JobCertificationSigneeEntity.Signature = imageData;                   
                }
                JobCertificationSigneeEntity.Name = Name;
                JobCertificationSigneeEntity.Position = Position;
                if (IsAllCompanies) JobCertificationSigneeEntity.CompanyId = null;

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                //retResult = new { status = "Error", message = ex.Message };
                status = "Error";
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult UploadSignature()
        {
            string status = "Success";
            string message = "Signature is Successfully uploaded!";
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase logoFile = Request.Files[0];
                    string signeeId = Request.Form["JobCertificationSigneeId"];
                    var signeeEntity = db.JobCertificationSignee.Find(int.Parse(signeeId));
                    if (signeeEntity != null)
                    {
                       
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(logoFile.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(logoFile.ContentLength);
                        }

                        signeeEntity.Signature = imageData;
                        signeeEntity.SetUpdated<JobCertificationSignee>();
                        db.SaveChanges();
                      
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid Job Cert. record data!";
                    }

                }
                catch (Exception ex)
                {
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }
        public JsonResult AjaxGetJobCertificationSignee(bool IsAllCompanies, int? CompanyId)
        {
            var signeeList = new List<dynamic>();
            if (CompanyId == null)
            {
                signeeList = db.GetAllByCompany<JobCertificationSignee>(SessionHelper.SelectedCompanyId, SessionHelper.SelectedClientId)
                                    .Where(w => IsAllCompanies ? (w.CompanyId == null) : true)
                                    .Select(s => new { id = s.Id, name = s.Name }).ToList<dynamic>();
            }
            else
            {
                signeeList = db.GetAllByCompany<JobCertificationSignee>(CompanyId, SessionHelper.SelectedClientId)                                    
                                    .Select(s => new { id = s.Id, name = s.Name }).ToList<dynamic>();
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = signeeList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
            //return Json(cityList);
        }
    }
}
