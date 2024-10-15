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
    public class CompanyController : TimeAideWebControllers<Company>
    {
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,CompanyCode,CompanyName,CompanyDescription,EIN,PortalWelcomeStatement,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Add<Company>(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

      

        // POST: Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,CompanyCode,CompanyName,CompanyDescription,EIN,PortalWelcomeStatement,ClientId,CreatedBy,CreatedDate,DataEntryStatus,ModifiedBy,ModifiedDate,IsDefaultPortalPicture,PortalPictureFilePath,DefaultLetterSigneeId,DefaultLetterTemplateId")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.SetUpdated<Company>();
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }


        public JsonResult AjaxGetCompany(EmployeePrivilegeViewModel model)
        {
            List<string> selectedClientList = (model.SelectedClientId ?? "").Split(',').ToList();
            var stateList = db.Company
                                .Where(w => selectedClientList.Contains(w.ClientId.ToString()) && w.DataEntryStatus == 1)
                                .Select(s => new { id = s.Id, name = s.CompanyName }).ToList();
            JsonResult jsonResult = new JsonResult()
            {
                Data = stateList,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }

        public override bool CheckBeforeDelete(int id)
        {
            

            var entity = db.Company.Include(u => u.UserInformations)
                                      .Include(u => u.EmailTemplate)
                         .FirstOrDefault(c => c.Id == id);
            if (entity.UserInformations.Where(t => t.DataEntryStatus == 1).Count() > 0 || entity.EmailTemplate.Where(t => t.DataEntryStatus == 1).Count() > 0)
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

        [HttpPost]
        public JsonResult UploadCompanyLogo()
        {
            string status = "Success";
            string message = "Company logo is Successfully uploaded!";
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase logoFile = Request.Files[0];
                    string companyId = Request.Form["CompanyId"];
                    var companyEntity = db.Company.Find(int.Parse(companyId));
                    if (companyEntity != null)
                    {
                        //var fileName = Path.GetFileName(logoFile.FileName);
                        //var fileExt = Path.GetExtension(logoFile.FileName);
                        //string companyLogo = "" + companyEntity.Id + "_"
                        //                     + companyId + "-" + fileName;
                        //string folderPath = Server.MapPath("~/Content/doc/CompanyLogo");
                        //if (!Directory.Exists(folderPath))
                        //{
                        //    Directory.CreateDirectory(folderPath);
                        //}
                        //string serverFilePath = Path.Combine(folderPath, companyLogo);
                        //logoFile.SaveAs(serverFilePath);
                        //string relativeFilePath = "/Content/doc/CompanyLogo/" + companyLogo;
                        //companyEntity.CompanyLogo = relativeFilePath;
                        //companyEntity.CompanyLogo = companyLogo;

                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(logoFile.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(logoFile.ContentLength);
                        }

                        companyEntity.CompanyLogo = imageData;
                        companyEntity.SetUpdated<Company>();
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
                    Helpers.ErrorLogHelper.InsertLog(Helpers.ErrorLogType.Error, ex, this.ControllerContext);
                    //retResult = new { status = "Error", message = ex.Message };
                    status = "Error";
                    message = ex.Message;
                }
            }
            return Json(new { status = status, message = message });
        }

        public ActionResult UploadDocument(int? id)
        {
            var model = db.Company.Find(id);
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult UploadDocument()
        {
            string status = "Success";
            string message = "Company Logo is Successfully uploaded!";
            // var model = db.EmployeeEducation.Find(id);
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpPostedFileBase logoFile = Request.Files[0];
                    string companyId = Request.Form["companyId"];
                    var company = db.Company.Find(int.Parse(companyId));
                    if (company != null)
                    {

                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(logoFile.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(logoFile.ContentLength);
                        }

                        company.CompanyLogo = imageData;
                        company.SetUpdated<Company>();
                        db.SaveChanges();
                    }
                    else
                    {
                        //retResult = new { status = "Error", message = "Invalid record data" };
                        status = "Error";
                        message = "Invalid company record data!";
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
    }
}
